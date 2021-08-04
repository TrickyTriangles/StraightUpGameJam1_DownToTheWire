using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStates;

public enum Direction
{
    NONE,
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Game Objects")]
    [SerializeField] private GameManager game_manager;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audio_source;
    [SerializeField] private Rigidbody2D rb;    
    [SerializeField] private WeaponSlot weapon;
    [SerializeField] private PotionSlot potion;
    [SerializeField] private Transform reader_pivot;
    [SerializeField] private ObjectReader reader;

    [Header("Colliders")]
    [SerializeField] private Collider2D floor_collider;
    [SerializeField] private Collider2D reader_collider;
    [SerializeField] private Collider2D hit_box;

    [Header("Stats")]
    [SerializeField] private int max_health;
    [SerializeField] private int initial_health;
    [SerializeField] private float walk_speed;
    private int health;
    private int key_count;

    [SerializeField] public Direction direction;
    private StatusEffect poison_effect;
    [SerializeField] private Vector2 move_direction;
    public bool paused;

    // State information
    private PlayerState next_state;
    private Coroutine active_state_routine;
    private Coroutine invuln_routine;

    #region Events and Subscriber Methods

    private Action<int, int> HealthValueUpdated;
    public void Subscribe_HealthValueUpdated(Action<int, int> sub) { HealthValueUpdated += sub; }
    public void Unsubscribe_HealthValueUpdated(Action<int, int> sub) { HealthValueUpdated -= sub; }

    private Action DirectionChanged;
    public void Subscribe_DirectionChanged(Action sub) { DirectionChanged += sub; }
    public void Unsubscribe_DirectionChanged(Action sub) { DirectionChanged -= sub; }

    private Action KeyValueUpdated;
    public void Subscribe_KeyValueUpdated(Action sub) { KeyValueUpdated += sub; }
    public void Unsubscribe_KeyValueUpdated(Action sub) { KeyValueUpdated -= sub; }

    private Action EndCurrentState;
    public void Subscribe_EndCurrentState(Action sub) { EndCurrentState += sub; }
    public void Unsubscribe_EndCurrentState(Action sub) { EndCurrentState -= sub; }

    private Action<string> StateHasChanged;
    public void Subscribe_StateHasChanged(Action<string> sub) { StateHasChanged += sub; }

    private Action OnDeath;
    public void Subscribe_OnDeath(Action sub) { OnDeath += sub; }
    public void Unsubscribe_OnDeath(Action sub) { OnDeath -= sub; }

    #endregion

    #region Properties

    public Rigidbody2D rigidbody2d
    {
        get { return rb; }
    }

    public int CurrentHealth
    {
        get { return health; }
    }

    public int CurrentMaxHealth
    {
        get { return max_health; }
    }

    public float WalkSpeed
    {
        get { return walk_speed; }
    }

    public Direction CurrentDirection
    {
        get { return direction; }
    }

    public bool IsFullHealth
    {
        get { return (health == max_health); }
    }

    public Animator animator
    {
        get { return anim; }
    }

    public AudioSource AudioSource
    {
        get { return audio_source; }
    }

    public Collider2D HitBox
    {
        get { return hit_box; }
    }

    public bool HasKey
    {
        get { return (key_count > 0); }
    }

    public int KeyCount
    {
        get { return key_count; }
    }

    public bool CanInteract
    {
        get { return reader.CanInteract; }
    }

    #endregion

    private void GameManager_RoomTransitionBeginCallback(Direction m_direction, float transition_time)
    {
        floor_collider.enabled = false;
        reader_collider.enabled = false;
        hit_box.enabled = false;

        SetNextState(new RoomTransition(m_direction, transition_time));
    }

    private void GameManager_RoomTransitionEndCallback()
    {
        floor_collider.enabled = true;
        reader_collider.enabled = true;
        hit_box.enabled = true;
    }

    private void Start()
    {
        health = Mathf.Clamp(initial_health, 1, max_health);

        if (game_manager != null)
        {
            game_manager.Subscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBeginCallback);
            game_manager.Subscribe_RoomTransitionEndCallback(GameManager_RoomTransitionEndCallback);
        }

        // Initialize player state machine
        StartCoroutine(ProcessStateMachine());

        // Update all UI elements to reflect our initial health values
        HealthValueUpdated?.Invoke(health, max_health);
    }

    private void Update()
    {
        Direction new_facing = MovementUtils.CheckMovementDirection(move_direction);

        if (new_facing != Direction.NONE)
        {
            direction = MovementUtils.CheckMovementDirection(move_direction);
            SetReaderOrientation();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + (move_direction * walk_speed * Time.fixedDeltaTime));
    }

    public void SetMoveDirection(Vector2 direction)
    {
        move_direction = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();

            if (interactable != null && !reader.Interacting)
            {
                reader.SetInteracting(true);
                interactable.Interact(this, () => { reader.SetInteracting(false); });
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RoomTrigger"))
        {
            NextRoomTrigger new_room = collision.gameObject.GetComponent<NextRoomTrigger>();

            if (new_room != null && game_manager != null)
            {
                EndCurrentState?.Invoke();
                game_manager.MoveToRoom(new_room.NextRoom, new_room.MoveDirection);
            }
        }

        if (collision.gameObject.CompareTag("EventTrigger"))
        {
            IInteractable interactable = collision.gameObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(this, () => { return; });
            }
        }
    }

    public void SetReaderOrientation()
    {
        Direction facing = MovementUtils.CheckMovementDirection(move_direction);

        if (reader_pivot != null)
        {
            switch (facing)
            {
                case Direction.NORTH:
                    reader_pivot.localPosition = new Vector3();
                    reader_pivot.localRotation = Quaternion.identity;
                    break;
                case Direction.EAST:
                    reader_pivot.localPosition = new Vector3(0.2f, -0.5f, 0f);
                    reader_pivot.localRotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
                case Direction.SOUTH:
                    reader_pivot.localPosition = new Vector3(0f, -0.6f, 0f);
                    reader_pivot.localRotation = Quaternion.Euler(0f, 0f, 180f);
                    break;
                case Direction.WEST:
                    reader_pivot.localPosition = new Vector3(-0.2f, -0.5f, 0f);
                    reader_pivot.localRotation = Quaternion.Euler(0f, 0f, 90f);
                    break;
                default:
                    break;
            }
        }
    }

    public void ActivatePoisonEffect()
    {
        poison_effect = new PoisonEffect();
        poison_effect.ActivateEffect(this);
    }

    public bool Attack()
    {
        if (weapon != null && !paused && !reader.CanInteract)
        {
            weapon.UseWeapon();
            return true;
        }

        return false;
    }

    public void UsePotion()
    {
        if (potion != null && !paused)
        {
            potion.UsePotion();
        }
    }

    void IDamageable.Damage(DamageProfile damage_profile)
    {
        // Prevents us from killing ourselves with the poison effect
        if (damage_profile.attacker == gameObject)
        {
            if (health == 1)
            {
                return;
            }
            else
            {
                health -= damage_profile.power;
                HealthValueUpdated?.Invoke(health, max_health);
                return;
            }
        }

        health -= damage_profile.power;

        if (damage_profile.attacker != gameObject && audio_source != null)
        {
            AudioClip clip = CommonResources.GetAudioClip("PlayerHit");
            if (clip != null) { audio_source.PlayOneShot(clip, 1f); }
        }

        if (health < 1)
        {
            if (damage_profile.attacker == gameObject) // Disallow the player's poison effect to kill themself
            {
                health = 1;              
            }
            else // Damage source came from something outside the player, so we kill them
            {
                SetNextState(new Dead());

                if (invuln_routine != null)
                {
                    StopCoroutine(invuln_routine);
                }

                floor_collider.enabled = false;
                reader_collider.enabled = false;
                hit_box.enabled = false;

                OnDeath?.Invoke();
            }
        }
        else
        {
            SetNextState(new Hit(damage_profile));
        }

        // Update UI elements to reflect new values
        HealthValueUpdated?.Invoke(health, max_health);
    }

    public void AwardPotions(int quantity)
    {
        if (potion != null)
        {
            potion.AwardPotion(quantity);
        }
    }

    public void RestoreMaxHealth()
    {
        health = max_health;
        HealthValueUpdated?.Invoke(health, max_health);
    }

    public void UseKey()
    {
        key_count--;
        KeyValueUpdated?.Invoke();
    }

    public void AwardKeys(int quantity)
    {
        key_count += quantity;
        KeyValueUpdated?.Invoke();
    }

    public void SetInvuln(float duration)
    {
        if (invuln_routine != null)
        {
            StopCoroutine(invuln_routine);
        }

        invuln_routine = StartCoroutine(HandleInvulnTimer(duration));
    }

    public void SetNextState(PlayerState received_state)
    {
        next_state = received_state;

        if (active_state_routine != null)
        {
            StopCoroutine(active_state_routine);
            active_state_routine = null;
        }
    }

    private IEnumerator ProcessStateMachine()
    {
        next_state = new Idle();

        while (true)
        {
            if (!paused)
            {
                if (active_state_routine == null)
                {
                    active_state_routine = StartCoroutine(next_state.ProcessState(this, SetNextState));
                    StateHasChanged?.Invoke(next_state.ToString());
                }
            }

            yield return null;
        }
    }

    private IEnumerator HandleInvulnTimer(float duration)
    {
        if (hit_box != null)
        {
            hit_box.enabled = false;

            float timer = 0f;
            float blink_timer = 0f;

            bool is_sprite_visible = true;
            Color base_color = sprite.color;
            Color transparent = new Color(0f, 0f, 0f, 0f);

            while (timer < duration)
            {
                timer += Time.deltaTime;
                blink_timer += Time.deltaTime;

                if (blink_timer > 0.02f)
                {
                    is_sprite_visible = !is_sprite_visible;
                    sprite.color = is_sprite_visible ? base_color : transparent;
                    blink_timer -= 0.05f;
                }

                yield return null;
            }

            sprite.color = base_color;
            hit_box.enabled = true;
        }
    }
}

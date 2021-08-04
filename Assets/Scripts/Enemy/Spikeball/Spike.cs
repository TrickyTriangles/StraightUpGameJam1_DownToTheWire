using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private GameObject attacker;
    private float lifetime = 5f;
    private float move_speed = 7f;

    private void GameManager_RoomTransitionBegin(Direction d, float f)
    {
        CommonResources.game_manager.Unsubscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(SpikeRoutine());
    }

    public void SetAngle(float angle)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

            damageable.Damage(new DamageProfile(gameObject, 1));
        }

        CommonResources.game_manager.Unsubscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            damageable.Damage(new DamageProfile(gameObject, 1));
        }

        CommonResources.game_manager.Unsubscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);
        Destroy(gameObject);
    }

    private IEnumerator SpikeRoutine()
    {
        float timer = 0f;
        CommonResources.game_manager.Subscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);

        while (timer < lifetime)
        {
            timer += Time.deltaTime;

            rb.MovePosition(rb.position + ((Vector2)transform.right * Time.fixedDeltaTime * move_speed));

            yield return null;
        }

        CommonResources.game_manager.Unsubscribe_RoomTransitionBeginCallback(GameManager_RoomTransitionBegin);
        Destroy(gameObject);
    }
}

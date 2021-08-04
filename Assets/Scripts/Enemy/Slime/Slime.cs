using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Slime
{
    public class Slime : Mob, IDamageable
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Collider2D hurtbox;
        [SerializeField] private Animator anim;
        [SerializeField] private Transform sprite_transform;

        private StateMachine<SlimeBaseState> state_machine;
        private PlayerController player;
        private Vector3 start_pos;
        private int base_health = 2;
        private int health;

        #region Events and Subscriber Methods

        private Action<Vector2> HitWall;
        public void Subscribe_HitWall(Action<Vector2> sub) { HitWall += sub; }
        public void Unsubscribe_HitWall(Action<Vector2> sub) { HitWall -= sub; }

        #endregion

        #region Properties

        public Animator animator { get { return anim; } }
        public Rigidbody2D Rigidbody { get { return rb; } }
        public Transform SpriteTransform { get { return sprite_transform; } }
        public Vector3 SpriteOffset { get { return new Vector3(0f, 0.18f, 0f); } }
        public Collider2D Hitbox { get { return hitbox; } }
        public Collider2D Hurtbox { get { return hurtbox; } }

        #endregion

        private void Awake()
        {
            state_machine = new StateMachine<SlimeBaseState>(this, new SlimeBaseState());

            start_pos = transform.position;
            health = base_health;
        }

        public override void ResetMob()
        {
            player = CommonResources.Player;
            StopAllCoroutines();
            state_machine = new StateMachine<SlimeBaseState>(this, new SlimeBaseState());
            state_machine.SetNextState(new Hop(player.transform));

            transform.position = start_pos;
            health = base_health;
        }

        void IDamageable.Damage(DamageProfile damage_profile)
        {
            health -= damage_profile.power;

            if (health <= 0)
            {
                OnDeath?.Invoke(this);

                if (hitbox != null) { hitbox.enabled = false; }
                if (hurtbox != null) { hurtbox.enabled = false; }

                state_machine.SetNextState(new Death());
            }
            else
            {
                state_machine.SetNextState(new Hit(damage_profile, 0.3f, 7f));
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

                damageable.Damage(new DamageProfile(gameObject, 1));
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                Vector2 normal = collision.contacts[0].normal;

                HitWall?.Invoke(normal);
            }
        }
    }
}
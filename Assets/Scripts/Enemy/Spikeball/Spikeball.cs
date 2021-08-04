using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Spikeball
{
    public class Spikeball : Mob, IDamageable
    {
        [SerializeField] private GameObject spike_prefab;
        [SerializeField] private Animator anim;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Collider2D hurtbox;

        private StateMachine<SpikeBaseState> state_machine;
        private Vector3 start_pos;
        private int base_health = 3;
        private int health;
        public float move_speed = 2f;

        #region Events and Subscriber Methods

        private Action<Vector2> HitWall;
        public void Subscribe_HitWall(Action<Vector2> sub) { HitWall += sub; }
        public void Unsubscribe_HitWall(Action<Vector2> sub) { HitWall -= sub; }

        #endregion

        #region Properties

        public Animator animator { get { return anim; } }
        public Rigidbody2D Rigidbody { get { return rb; } }
        public Collider2D Hitbox { get { return hitbox; } }
        public Collider2D Hurtbox { get { return hurtbox; } }
        public GameObject SpikePrefab { get { return spike_prefab; } }

        #endregion

        private void Awake()
        {
            state_machine = new StateMachine<SpikeBaseState>(this, new SpikeBaseState());
            state_machine.SetNextState(new Float());

            start_pos = transform.position;
            health = base_health;
        }

        public override void ResetMob()
        {
            StopAllCoroutines();
            state_machine = new StateMachine<SpikeBaseState>(this, new SpikeBaseState());
            state_machine.SetNextState(new Float());

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
                state_machine.SetNextState(new Hit(damage_profile, 0.4f, 3f));
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

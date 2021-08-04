using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Bat
{
    public class Bat : Mob, IDamageable
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Collider2D hurtbox;
        [SerializeField] private Animator anim;
        [SerializeField] private GameObject fireball;

        private StateMachine<BatBaseState> state_machine;
        private PlayerController player;
        private float move_speed = 1f;       
        private Vector3 start_pos;
        private int base_health = 2;
        private int health;

        #region Properties

        public StateMachine<BatBaseState> StateMachine
        {
            get { return state_machine; }
        }

        public float MoveSpeed
        {
            get { return move_speed; }
        }

        public Collider2D HitBox
        {
            get { return hitbox; }
        }

        public Collider2D Hurtbox
        {
            get { return hurtbox; }
        }

        public Rigidbody2D Rigidbody
        {
            get { return rb; }
        }

        public Animator animator
        {
            get { return anim; }
        }

        public GameObject Fireball
        {
            get { return fireball; }
        }

        #endregion

        private void Awake()
        {
            player = CommonResources.Player;
            state_machine = new StateMachine<BatBaseState>(this, new BatBaseState());

            start_pos = transform.position;
            health = base_health;
        }

        public override void ResetMob()
        {
            StopAllCoroutines();
            state_machine = new StateMachine<BatBaseState>(this, new BatBaseState());
            state_machine.SetNextState(new Float(this));

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
    }
}

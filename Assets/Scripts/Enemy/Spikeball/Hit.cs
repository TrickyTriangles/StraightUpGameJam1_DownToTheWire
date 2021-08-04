using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Spikeball
{
    public class Hit : SpikeBaseState
    {
        private DamageProfile damage_profile;
        private float hit_time;
        private float hit_speed;

        public Hit(DamageProfile dp, float m_hit_time, float m_hit_speed)
        {
            damage_profile = dp;
            hit_time = m_hit_time;
            hit_speed = m_hit_speed;
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            float timer = 0f;
            Vector3 direction = -(damage_profile.attacker.gameObject.transform.position - subject.gameObject.transform.position).normalized;
            Spikeball slime = subject as Spikeball;

            CommonResources.PlaySound("EnemyHit", 0.6f);
            slime.animator.Play("Hit", 0, 0f);
            slime.Hitbox.enabled = false;
            slime.Hurtbox.enabled = false;

            while (timer < hit_time)
            {
                timer += Time.deltaTime;
                slime.Rigidbody.MovePosition(slime.Rigidbody.position + ((Vector2)direction * Time.deltaTime * hit_speed));

                yield return null;
            }

            slime.Hitbox.enabled = true;
            slime.Hurtbox.enabled = true;

            next_state_callback(new Float());
        }
    }
}

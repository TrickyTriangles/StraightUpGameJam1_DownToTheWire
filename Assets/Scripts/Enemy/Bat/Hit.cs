using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Bat
{
    public class Hit : BatBaseState
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
            Bat bat = subject as Bat;

            bat.HitBox.enabled = false;
            bat.Hurtbox.enabled = false;
            bat.animator.Play("Hit", 0, 0f);
            CommonResources.PlaySound("EnemyHit", 0.6f);

            float timer = 0f;
            Vector3 direction = -(damage_profile.attacker.gameObject.transform.position - subject.gameObject.transform.position).normalized;

            while (timer < hit_time)
            {
                timer += Time.deltaTime;
                bat.Rigidbody.MovePosition(bat.Rigidbody.position + ((Vector2)direction.normalized * Time.deltaTime * hit_speed));

                yield return null;
            }

            bat.HitBox.enabled = true;
            bat.Hurtbox.enabled = true;

            next_state_callback(new Float(bat));
        }
    }
}
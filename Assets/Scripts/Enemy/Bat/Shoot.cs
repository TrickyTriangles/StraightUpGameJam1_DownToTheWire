using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Bat
{
    public class Shoot : BatBaseState
    {
        private Transform target;

        public Shoot(Transform m_target)
        {
            target = m_target;
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            Bat bat = subject as Bat;
            bat.animator.StopPlayback();

            Fireball fireball = GameObject.Instantiate(bat.Fireball, bat.transform.position, Quaternion.identity).GetComponent<Fireball>();

            if (fireball != null)
            {
                fireball.SetTarget(target.position);
            }

            yield return new WaitForSeconds(0.6f);

            next_state_callback(new Float(bat));
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Bat
{
    public class Death : BatBaseState
    {
        public Death() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            Bat bat = subject as Bat;
            bat.animator.Play("Hit", 0, 0f);

            CommonResources.PlaySound("EnemyHit", 0.6f);
            GameObject death_particle = CommonResources.GetPrefab("EnemyDeath");
            if (death_particle != null)
            {
                GameObject.Instantiate(death_particle, bat.transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(0.2f);
            GameObject.Destroy(subject.gameObject);
        }
    }
}
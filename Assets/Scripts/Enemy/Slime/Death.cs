using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Slime
{
    public class Death : SlimeBaseState
    {
        public Death() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            Slime slime = subject as Slime;
            slime.animator.Play("Hit", 0, 0f);

            CommonResources.PlaySound("EnemyHit", 0.6f);
            GameObject death_particle = CommonResources.GetPrefab("EnemyDeath");
            if (death_particle != null)
            {
                GameObject.Instantiate(death_particle, slime.transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(0.2f);
            GameObject.Destroy(subject.gameObject);
        }
    }
}
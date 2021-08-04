using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Spikeball
{
    public class Shoot : SpikeBaseState
    {
        public Shoot() { }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            Spikeball spike = subject as Spikeball;
            float offset = 45f;

            for (int i = 0; i < 4; i++)
            {
                GameObject.Instantiate(spike.SpikePrefab, spike.transform.position, Quaternion.Euler(0f, 0f, offset + (90 * i)));
            }

            yield return new WaitForSeconds(1f);
            next_state_callback(new Float());
        }
    }
}

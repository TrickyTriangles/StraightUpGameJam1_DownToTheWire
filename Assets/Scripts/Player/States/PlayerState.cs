using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public abstract class PlayerState
    {
        public virtual IEnumerator ProcessState(PlayerController player, Action<PlayerState> next_state_callback)
        {
            yield return null;
        }
    }
}
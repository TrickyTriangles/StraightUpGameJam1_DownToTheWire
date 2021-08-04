using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class Dead : PlayerState
    {
        public override IEnumerator ProcessState(PlayerController player, Action<PlayerState> next_state_callback)
        {
            player.animator.Play("Dead", 0, 0f);
            player.SetMoveDirection(new Vector2());

            BGM.Instance.Stop(3f);

            while (true)
            {
                yield return null;
            }
        }
    }
}

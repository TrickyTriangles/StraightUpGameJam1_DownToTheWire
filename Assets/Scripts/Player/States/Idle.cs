using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class Idle : PlayerState
    {
        private void SetAnimation(PlayerController player)
        {
            if (player.animator != null)
            {
                switch (player.direction)
                {
                    case Direction.NORTH:
                        player.animator.Play("IdleNorth", 0, 0f);
                        break;
                    case Direction.EAST:
                        player.animator.Play("IdleEast", 0, 0f);
                        break;
                    case Direction.SOUTH:
                        player.animator.Play("IdleSouth", 0, 0f);
                        break;
                    case Direction.WEST:
                        player.animator.Play("IdleWest", 0, 0f);
                        break;
                    default:
                        break;
                }
            }
        }

        public override IEnumerator ProcessState(PlayerController player, Action<PlayerState> next_state_callback)
        {
            player.SetMoveDirection(new Vector2());
            SetAnimation(player);

            while (Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0f)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    player.UsePotion();
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    next_state_callback(new Attack());
                    yield break;
                }

                yield return null;
            }

            next_state_callback(new Walk());
        }
    }
}

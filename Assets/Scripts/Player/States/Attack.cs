using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class Attack : PlayerState
    {
        private void SetAnimation(PlayerController player)
        {
            switch (player.direction)
            {
                case Direction.NONE:
                    break;
                case Direction.NORTH:
                    player.animator.Play("AttackNorth", 0, 0f);
                    break;
                case Direction.EAST:
                    player.animator.Play("AttackEast", 0, 0f);
                    break;
                case Direction.SOUTH:
                    player.animator.Play("AttackSouth", 0, 0f);
                    break;
                case Direction.WEST:
                    player.animator.Play("AttackWest", 0, 0f);
                    break;
                default:
                    break;
            }
        }

        public override IEnumerator ProcessState(PlayerController player, Action<PlayerState> next_state_callback)
        {
            if (!player.CanInteract)
            {
                SetAnimation(player);
                player.SetMoveDirection(new Vector2());

                if (player.Attack())
                {
                    CommonResources.PlaySound("Slash", 1f);
                }

                float timer = 0f;

                while (timer < 0.1f)
                {
                    timer += Time.deltaTime;

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        player.UsePotion();
                    }

                    yield return null;
                }

            }

            next_state_callback(new Idle());
        }
    }
}

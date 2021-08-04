using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class Walk : PlayerState
    {
        PlayerController pc;
        Direction facing;

        public override IEnumerator ProcessState(PlayerController player, Action<PlayerState> next_state_callback)
        {
            pc = player;
            facing = pc.CurrentDirection;
            SetAnimation(facing);

            Vector3 direction = new Vector3();

            direction.x = Input.GetAxisRaw("Horizontal");
            direction.y = Input.GetAxisRaw("Vertical");

            while (direction.magnitude > 0.1f)
            {
                player.SetMoveDirection(new Vector2(direction.x, direction.y));

                direction.x = Input.GetAxisRaw("Horizontal");
                direction.y = Input.GetAxisRaw("Vertical");

                if (facing != pc.CurrentDirection)
                {
                    SetAnimation(pc.CurrentDirection);
                    facing = pc.CurrentDirection;
                }

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

            player.SetNextState(new Idle());
        }

        private void SetAnimation(Direction direction)
        {
            if (pc.animator != null)
            {
                switch (direction)
                {
                    case Direction.NORTH:
                        pc.animator.Play("WalkNorth", 0, 0f);
                        break;
                    case Direction.EAST:
                        pc.animator.Play("WalkEast", 0, 0f);
                        break;
                    case Direction.SOUTH:
                        pc.animator.Play("WalkSouth", 0, 0f);
                        break;
                    case Direction.WEST:
                        pc.animator.Play("WalkWest", 0, 0f);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
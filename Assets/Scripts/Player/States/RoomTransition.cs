using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class RoomTransition : PlayerState
    {
        private Direction direction;
        private float transition_time;
        private float offset = 3f;

        public RoomTransition(Direction m_direction, float m_transition_time)
        {
            direction = m_direction;
            transition_time = m_transition_time;
        }

        public override IEnumerator ProcessState(PlayerController player, Action<PlayerState> next_state_callback)
        {
            Vector3 dir = new Vector3();
            Vector3 start_pos = player.transform.position;

            player.SetMoveDirection(new Vector2());

            switch (direction)
            {
                case Direction.NORTH:
                    dir = Vector3.up;
                    break;
                case Direction.EAST:
                    dir = Vector3.right;
                    break;
                case Direction.SOUTH:
                    dir = Vector3.down;
                    break;
                case Direction.WEST:
                    dir = Vector3.left;
                    break;
                default:
                    break;
            }

            Vector3 end_pos = start_pos + (dir * offset);
            float timer = 0f;

            while (timer < transition_time)
            {
                timer += Time.unscaledDeltaTime;
                player.transform.position = Vector3.Lerp(start_pos, end_pos, timer / transition_time);
                yield return null;
            }

            next_state_callback(new Idle());
        }
    }
}

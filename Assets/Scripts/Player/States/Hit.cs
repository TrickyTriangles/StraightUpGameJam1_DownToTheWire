using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStates
{
    public class Hit : PlayerState
    {
        private DamageProfile damage_profile;

        public Hit(DamageProfile m_dp)
        {
            damage_profile = m_dp;
        }

        private void SetAnimation(PlayerController player)
        {
            switch (player.direction)
            {
                case Direction.NONE:
                    break;
                case Direction.NORTH:
                    player.animator.Play("HitNorth", 0, 0f);
                    break;
                case Direction.EAST:
                    player.animator.Play("HitEast", 0, 0f);
                    break;
                case Direction.SOUTH:
                    player.animator.Play("HitSouth", 0, 0f);
                    break;
                case Direction.WEST:
                    player.animator.Play("HitWest", 0, 0f);
                    break;
                default:
                    break;
            }
        }

        public override IEnumerator ProcessState(PlayerController player, Action<PlayerState> next_state_callback)
        {
            float timer = 0f;

            SetAnimation(player);
            Vector3 move_direction = damage_profile.position_hit - player.transform.position;

            player.HitBox.enabled = false;
            player.SetMoveDirection(-move_direction.normalized);

            while (timer < 0.3f)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            player.HitBox.enabled = true;
            player.SetInvuln(1f);
            next_state_callback(new Idle());
        }
    }
}

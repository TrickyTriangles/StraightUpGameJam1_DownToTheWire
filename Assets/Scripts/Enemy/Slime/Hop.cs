using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Slime
{
    public class Hop : SlimeBaseState
    {
        private Slime slime;
        private Transform target;
        private Vector2 move_direction;
        private float move_speed = 2f;
        private float hop_time = 1.5f;
        private Vector2 sprite_offset;

        public Hop(Transform m_target)
        {
            target = m_target;
        }

        private void Slime_HitWall(Vector2 normal)
        {
            move_direction = Vector2.Reflect(move_direction, normal);

            sprite_offset = new Vector2(0f, 0.186f);
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            slime = subject as Slime;
            slime.Subscribe_HitWall(Slime_HitWall);

            slime.SpriteTransform.localPosition = slime.SpriteOffset;
            slime.animator.Play("Move", 0, 0f);
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));

            while (true)
            {
                float timer = 0f;
                move_direction = (target.transform.position - slime.transform.position).normalized;

                slime.animator.Play("Jump", 0, 0f);

                while (timer < hop_time)
                {
                    timer = Mathf.Clamp(timer += Time.deltaTime, 0f, hop_time);
                    float ratio = timer / hop_time;

                    if (ratio < 0.15f || ratio > 0.85f)
                    {
                        slime.Hitbox.enabled = true;
                        slime.Hurtbox.enabled = true;
                    }
                    else
                    {
                        slime.Hitbox.enabled = false;
                        slime.Hurtbox.enabled = false;
                    }

                    slime.Rigidbody.MovePosition(slime.Rigidbody.position + (move_direction.normalized * move_speed * Time.fixedDeltaTime));
                    slime.SpriteTransform.localPosition = (Vector3)sprite_offset + new Vector3(0f, Mathf.Sin(Mathf.PI * ratio) * 3f, 0f);

                    yield return null;
                }

                slime.animator.Play("Move", 0, 0f);
                yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));
            }
            
        }

        public override void EndOfStateCleanup()
        {
            slime.Unsubscribe_HitWall(Slime_HitWall);
        }
    }
}
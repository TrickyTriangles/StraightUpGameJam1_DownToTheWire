using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Spikeball
{
    public class Float : SpikeBaseState
    {
        private Spikeball spike;
        private Vector2 move_direction;

        public Float()
        {
            move_direction = UnityEngine.Random.insideUnitCircle.normalized;
        }

        private void Spikeball_HitWall(Vector2 normal)
        {
            move_direction = Vector2.Reflect(move_direction, normal);
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            spike = subject as Spikeball;
            spike.Subscribe_HitWall(Spikeball_HitWall);
            spike.animator.Play("Move", 0, 0f);

            float timer = 0f;
            float delay = UnityEngine.Random.Range(2f, 3f);

            while (timer < delay)
            {
                timer += Time.deltaTime;
                spike.Rigidbody.MovePosition(spike.Rigidbody.position + (move_direction * spike.move_speed * Time.fixedDeltaTime));

                yield return null;
            }

            next_state_callback(new Shoot());
        }

        public override void EndOfStateCleanup()
        {
            spike.Unsubscribe_HitWall(Spikeball_HitWall);
        }
    }
}

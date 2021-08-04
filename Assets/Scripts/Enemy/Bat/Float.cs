using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Bat
{
    public class Float : BatBaseState
    {
        private Bat self;
        private PlayerController player;

        public Float(Bat m_self)
        {
            self = m_self;
            player = CommonResources.Player;
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            bool can_shoot = false;

            while (!can_shoot)
            {
                float timer = 0f;
                //Vector3 target = subject.transform.position + (Vector3)(UnityEngine.Random.insideUnitCircle * 2f);
                Vector3 direction = player.transform.position - subject.transform.position;

                self.animator.Play("Idle", 0, 0f);

                while (timer < 1f)
                {
                    timer += Time.deltaTime;
                    self.Rigidbody.MovePosition(self.Rigidbody.position + ((Vector2)direction.normalized * Time.deltaTime * 1f));

                    if (Vector3.Distance(player.transform.position, subject.transform.position) < 2f)
                    {
                        can_shoot = true;
                        break;
                    }

                    yield return null;
                }

                float chance = UnityEngine.Random.Range(0f, 1f);

                if (chance > 0.8f)
                {
                    can_shoot = true;
                }

                yield return null;
            }

            yield return new WaitForSeconds(1f);
            next_state_callback(new Shoot(player.transform));
        }
    }
}

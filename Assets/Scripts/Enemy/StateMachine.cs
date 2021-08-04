using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine<T> where T : BaseState
    {
        private MonoBehaviour subject;
        private T base_state;
        private T next_state;
        private Coroutine active_state_routine;

        public StateMachine(MonoBehaviour m_subject, T m_base_state)
        {
            subject = m_subject;
            base_state = m_base_state;

            subject.StartCoroutine(ProcessStateMachine());
        }

        public void SetNextState(BaseState received_state)
        {
            next_state.EndOfStateCleanup();
            next_state = (T)received_state;

            if (active_state_routine != null)
            {
                subject.StopCoroutine(active_state_routine);
                active_state_routine = null;
            }
        }

        public IEnumerator ProcessStateMachine()
        {
            next_state = base_state;

            while (true)
            {
                if (active_state_routine == null)
                {
                    active_state_routine = subject.StartCoroutine(next_state.ProcessState(subject, SetNextState));
                }

                yield return null;
            }
        }
    }
}
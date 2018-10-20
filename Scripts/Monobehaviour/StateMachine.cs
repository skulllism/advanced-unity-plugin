using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace AdvancedUnityPlugin
{
    /*
   * @brief the scriptable finite state machine
   * @details this is just FSM but all of states can be reuse
   * @author Kay
   * @date 2018-05-31
   * @version 0.0.1
   * */
    public class StateMachine : MonoBehaviour
    {
        public abstract class State
        {
            public string ID;

            public abstract bool IsTransition(out string ID);

            public abstract void OnEnter();

            public abstract void OnFixedUpdate();

            public abstract void OnUpdate();

            public abstract void OnLateUpdate();

            public abstract void OnExit();
        }

        public readonly List<State> states = new List<State>();

        public State current { private set; get; }

        public bool IsState(string ID)
        {
            return current.ID == ID;
        }

        public void TransitionToState(string ID)
        {
            if (current != null)
                current.OnExit();

            current = GetState(ID);

            current.OnEnter();
        }

        private void Update()
        {
            if (current == null)
                return;

            string transition = null;
            if (current.IsTransition(out transition))
            {
                TransitionToState(transition);
                return;
            }

            current.OnUpdate();
        }

        private void LateUpdate()
        {
            if (current == null)
                return;

            current.OnLateUpdate();
        }

        private void FixedUpdate()
        {
            if (current == null)
                return;

            current.OnFixedUpdate();
        }

        private State GetState(string ID)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].ID == ID)
                    return states[i];
            }

            Debug.Log("Not found ID : " + ID);
            return null;
        }
    }
}

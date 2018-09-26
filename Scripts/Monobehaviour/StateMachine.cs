using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public string initStateName;

        public State[] states;

        public State current { private set; get; }

        public bool IsState(string stateName)
        {
            return current.id == stateName;
        }

        private void Start()
        {
            TransitionToState(initStateName);
        }

        public void TransitionToState(string id)
        {
            if (current)
                current.OnExit();

            current = GetState(id);

            current.OnEnter();
            //Debug.Log(id);
        }

        private void FixedUpdate()
        {
            if (!current)
                return;

            current.OnUpdate();
        }

        private State GetState(string id)
        {
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i].id == id)
                    return states[i];
            }

            return null;
        }
    }
}

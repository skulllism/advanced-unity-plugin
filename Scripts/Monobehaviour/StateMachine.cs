﻿using System.Collections;
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

        private State current;

        private void Awake()
        {
            TransitionToState(initStateName);
        }

        public void TransitionToState(string id)
        {
            if (current)
                current.OnExit();

            current = GetState(id);

            current.OnEnter();
        }

        private void Update()
        {
            if (!current)
                return;

            string next;
            if (current.IsTransition(out next))
            {
                TransitionToState(next);
                return;
            }

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

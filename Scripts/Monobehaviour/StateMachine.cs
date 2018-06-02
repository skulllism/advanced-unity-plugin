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
        public State[] states;

        private State[] clones;

        private State current;

        private void Awake()
        {
            clones = PrototypeScriptableObject.SetClones(gameObject, states);
        }

        public void TransitionToState(string ID)
        {
            if (current)
                current.OnExit(gameObject);

            current = GetState(ID);

            current.OnEnter(gameObject);
        }

        private void Update()
        {
            if (!current)
                return;

            string next;
            if (current.IsTransition(gameObject, out next))
                TransitionToState(next);
        }

        private State GetState(string ID)
        {
            for (int i = 0; i < clones.Length; i++)
            {
                if (clones[i].ID == ID)
                    return clones[i];
            }

            return null;
        }
    }
}

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
    public abstract class MonobehaviourStateMachine : MonoBehaviour
    {
        public readonly List<IState> states = new List<IState>();

        public IState current { private set; get; }

        public bool IsState(string ID)
        {
            return current.ID == ID;
        }

        public void TransitionToState(string ID)
        {
            if (current != null)
            {
                #if DEBUG_PLAYER_TRANSITION
                if (name == "Player")
                {
                    Debug.Log(name + "'s Transition : " + current.ID + " → " + ID);
                }
                #endif
                #if DEBUG_NONPLAYER_TRANSITION
                if (name != "Player")
                {
                    Debug.Log(name + "'s Transition : " + current.ID + " → " + ID);
                }
                #endif
                current.OnExit();
            }

            current = GetState(ID);

            Debug.Assert(current != null, ID);
            //Debug.Log(current.ID);
            current.OnEnter();
        }

        protected virtual void Update()
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

        private IState GetState(string ID)
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

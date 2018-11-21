using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class AdvancedStateMachine : StateMachine
    {
        [Serializable]
        public abstract class AdvancedTransition : MonoBehaviour
        {
            public string ID;
            public string stateID;

            public abstract bool IsTransition();
        }

        [Serializable]
        public class AdvancedState : State
        {
            public List<AdvancedTransition> transitions = new List<AdvancedTransition>();
     
            public UnityEvent onEnter;
            public UnityEvent onUpdate;
            public UnityEvent onFixedUpdate;
            public UnityEvent onLateUpdate;
            public UnityEvent onExit;

            private AdvancedStateMachine advancedStateMachine;

            public void Init(AdvancedStateMachine advancedStateMachine)
            {
                this.advancedStateMachine = advancedStateMachine;
            }

            public override bool IsTransition(out string ID)
            {
                for (int i = 0; i < transitions.Count; i++)
                {
                    Debug.Assert(transitions[i] != null , transitions[i].ID);

                    if (transitions[i].IsTransition())
                    {
                        ID = transitions[i].stateID;
                        return true;
                    }
                }

                ID = null;
                return false;
            }

            public override void OnEnter()
            {
                onEnter.Invoke();
            }

            public override void OnExit()
            {
                onExit.Invoke();
            }

            public override void OnFixedUpdate()
            {
                onFixedUpdate.Invoke();
            }

            public override void OnLateUpdate()
            {
                onLateUpdate.Invoke();
            }

            public override void OnUpdate()
            {
                onUpdate.Invoke();
            }
        }

        public string initialStateID;
        public List<AdvancedState> advancedStates = new List<AdvancedState>();

        private void Awake()
        {
            foreach (var state in advancedStates)
            {
                state.Init(this);
                states.Add(state);
            }
        }

        private void Start()
        {
            TransitionToState(initialStateID);
        }

        public bool IsState(StringVariable ID)
        {
            return IsState(ID.runtimeValue);
        }

        public void TransitionToState(StringVariable ID)
        {
            TransitionToState(ID.runtimeValue);
        }
    }
}
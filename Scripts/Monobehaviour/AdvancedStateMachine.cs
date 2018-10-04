using System;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class AdvancedStateMachine : StateMachine
    {
        [Serializable]
        public class AdvancedDecision
        {
            public bool isTrue;
            public Condition condition;
        }

        [Serializable]
        public class AdvancedTransition
        {
            public string ID;

            public StringVariable stateID;
            public AdvancedDecision[] decisions;

            public virtual bool IsTransition()
            {
                for (int i = 0; i < decisions.Length; i++)
                {
                    if (decisions[i].condition.Invoke() != decisions[i].isTrue)
                        return false;
                }
                return true;
            }
        }

        [Serializable]
        public class AdvancedState : State
        {
            public StringVariable[] transitions;

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
                for (int i = 0; i < transitions.Length; i++)
                {
                    AdvancedTransition transition = advancedStateMachine.GetTransition(transitions[i]);

                    Debug.Assert(transition != null);
                    if (transition.IsTransition())
                    {
                        ID = transition.stateID.runtimeValue;
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

        public StringVariable initialStateID;
        public AdvancedState[] advancedStates;
        public AdvancedTransition[] advancedTransitions;

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

        public AdvancedTransition GetTransition(StringVariable ID)
        {
            foreach (var transition in advancedTransitions)
            {
                if (transition.ID == ID.runtimeValue)
                    return transition;
            }

            return null;
        }
    }
}
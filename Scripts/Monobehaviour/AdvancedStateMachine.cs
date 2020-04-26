using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class AdvancedStateMachine : MonoBehaviourStateMachine
    {
        public string initialStateID;
        public List<AdvancedState> advancedStates = new List<AdvancedState>();

        private void Awake()
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
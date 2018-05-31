using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/State")]
    public class State : ScriptableObject
    {
        [Header("Action")]
        public Action[] onEnters;
        public Action[] onUpdates;
        public Action[] onExits;

        [Header("Transition")]
        public Transition[] transitions;

        public void OnEnter(Actor actor)
        {
            for (int i = 0; i < onEnters.Length; i++)
            {
                onEnters[i].OnAction(actor);
            }
        }

        public void OnUpdate(Actor actor)
        {
            for (int i = 0; i < onUpdates.Length; i++)
            {
                onUpdates[i].OnAction(actor);
            }
        }

        public void OnExit(Actor actor)
        {
            for (int i = 0; i < onExits.Length; i++)
            {
                onEnters[i].OnAction(actor);
            }
        }
    }
}


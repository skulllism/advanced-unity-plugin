using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Transition")]
    public class Transition : ScriptableObject
    {
        [Header("Decision")]
        public Decision[] decisions;
        public bool isTrue;

        [Header("Target")]
        public State state;

        public bool CanBeTransit(Actor actor , out State state)
        {
            state = this.state;

            for (int i = 0; i < decisions.Length; i++)
            {
                if (decisions[i].Decide(actor) != decisions[i].isTrue)
                    return false;
            }

            return true;
        }
    }
}


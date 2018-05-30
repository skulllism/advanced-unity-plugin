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
        public Decision[] transitions;
    }
}


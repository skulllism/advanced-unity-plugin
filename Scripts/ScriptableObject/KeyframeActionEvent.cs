using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class KeyframeActionEvent : KeyframeEvent
    {
        public GameObject gameObj;

        public Action[] onKeyframeAction;

        private Action[] cloneActions;

        private void Awake()
        {
            cloneActions = PrototypeScriptableObject.SetClones(gameObj, onKeyframeAction);
        }

        public override void OnKeyframeEvent()
        {
            for (int i = 0; i < cloneActions.Length; i++)
            {
                cloneActions[i].OnAction(gameObj);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class TermActionEvent : TermEvent
    {
        public GameObject gameObj;

        public Action[] startKeyframeActions;
        public Action[] onTermActions;
        public Action[] endKeyframeActions;

        private Action[] cloneStartKeyframeActions;
        private Action[] cloneOnTermActions;
        private Action[] cloneEndKeyframeActions;

        private void Awake()
        {
            cloneStartKeyframeActions = PrototypeScriptableObject.SetClones(gameObj, startKeyframeActions);
            cloneOnTermActions = PrototypeScriptableObject.SetClones(gameObj, onTermActions);
            cloneEndKeyframeActions = PrototypeScriptableObject.SetClones(gameObj, endKeyframeActions);
        }

        //public override void OnStartTermEvent()
        //{
        //    for (int i = 0; i < cloneStartKeyframeActions.Length; i++)
        //    {
        //        cloneStartKeyframeActions[i].OnAction(gameObj);
        //    }
        //}

        public override void OnTermEvent()
        {
            for (int i = 0; i < cloneOnTermActions.Length; i++)
            {
                cloneOnTermActions[i].OnAction(gameObj);
            }
        }

        //public override void OnEndTermEvent()
        //{
        //    for (int i = 0; i < cloneEndKeyframeActions.Length; i++)
        //    {
        //        cloneEndKeyframeActions[i].OnAction(gameObj);
        //    }
        //}
    }
}
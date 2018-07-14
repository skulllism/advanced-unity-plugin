using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AdvancedUnityPlugin
{
    public class AnimationActionEvent : AnimationEvent
    {
        public GameObject gameObj;

        public Action[] onStartActions;
        public Action[] onFinishActions;

        private Action[] cloneOnStartActions;
        private Action[] cloneOnFinishActions;

        private void Awake()
        {
            cloneOnStartActions = PrototypeScriptableObject.SetClones(gameObj,onStartActions);
            cloneOnFinishActions = PrototypeScriptableObject.SetClones(gameObj, onFinishActions);
        }

        public override void OnStart()
        {
            base.OnStart();
            foreach (var action in cloneOnStartActions)
            {
                action.OnAction(gameObj);
            }
        }

        public override void OnFinish()
        {
            base.OnFinish();
            foreach (var action in cloneOnFinishActions)
            {
                action.OnAction(gameObj);
            }
        }
    }
}

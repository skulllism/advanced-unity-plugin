using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class KeyframeEvent : MonoBehaviour
    {
        public GameObject gameObj;

        [Header("Event Keyframe")]
        public int eventKeyframe;

        [Header("Keyframe Actions")]
        public Action[] onKeyframeAction;

        [Header("Keyframe UnityEvent")]
        public UnityEvent onKeyframeUnityEvent;

        private Action[] cloneActions;

        private void Awake()
        {
            cloneActions = PrototypeScriptableObject.SetClones(gameObj, onKeyframeAction);
        }

        public virtual void OnKeyframeEvent()
        {
            for (int i = 0; i < cloneActions.Length; i++)
            {
                cloneActions[i].OnAction(gameObj);
            }

            onKeyframeUnityEvent.Invoke();
        }
    }
}

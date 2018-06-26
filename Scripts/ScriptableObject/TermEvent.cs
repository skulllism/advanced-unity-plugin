using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class TermEvent : MonoBehaviour
    {
        public GameObject gameObj;
        [Header("Start Keyframe Action")]
        public float startFrame;
        public Action[] startKeyframeActions;
        public UnityEvent startKeyframeUnityEvent;

        [Header("Term Actions")]
        public Action[] onTermActions;

        [Header("Term UnityEvent")]
        public UnityEvent onTermUnityEvent;

        [Header("End Keyframe Action")]
        public float endFrame;
        public Action[] endKeyframeActions;
        public UnityEvent endKeyframeUnityEvent;

        private Action[] cloneStartKeyframeActions;
        private Action[] cloneOnTermActions;
        private Action[] cloneEndKeyframeActions;

        private void Awake()
        {
            cloneStartKeyframeActions = PrototypeScriptableObject.SetClones(gameObj, startKeyframeActions);
            cloneOnTermActions = PrototypeScriptableObject.SetClones(gameObj, onTermActions);
            cloneEndKeyframeActions = PrototypeScriptableObject.SetClones(gameObj, endKeyframeActions);
        }

        public virtual void OnStartTermEvent()
        {
            for (int i = 0; i < cloneStartKeyframeActions.Length; i++)
            {
                cloneStartKeyframeActions[i].OnAction(gameObj);
            }

            startKeyframeUnityEvent.Invoke();
        }

        public virtual void OnTermEvent()
        {
            for (int i = 0; i < cloneOnTermActions.Length; i++)
            {
                cloneOnTermActions[i].OnAction(gameObj);
            }

            onTermUnityEvent.Invoke();
        }

        public virtual void OnEndTermEvent()
        {
            for (int i = 0; i < cloneEndKeyframeActions.Length; i++)
            {
                cloneEndKeyframeActions[i].OnAction(gameObj);
            }

            endKeyframeUnityEvent.Invoke();
        }
    }
}
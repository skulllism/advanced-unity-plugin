using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    /*
 * @brief assembly prototype state
 * @details you can assemble Action, Transition on each slots. all of Prototype ScriptableObject would be clone
 * @author Kay
 * @date 2018-05-31
 * @version 0.0.1
 * */
    public class AdvancedState : State
    {
        public GameObject gameObj;

        [Header("Action")]
        public Action[] onEnters;
        public Action[] onUpdates;
        public Action[] onExits;

        public UnityEvent onEnter;
        public UnityEvent onUpdate;
        public UnityEvent onExit;

        [Header("Transition")]
        public Transition[] transitions;

        private Action[] cloningOnEnters;
        private Action[] cloningOnUpdates;
        private Action[] cloningOnExits;
        private Transition[] cloningTransitions;

        protected virtual void Awake()
        {
            cloningOnEnters = PrototypeScriptableObject.SetClones(gameObj, onEnters);
            cloningOnUpdates = PrototypeScriptableObject.SetClones(gameObj, onUpdates);
            cloningOnExits = PrototypeScriptableObject.SetClones(gameObj, onExits);

            cloningTransitions = PrototypeScriptableObject.SetClones(gameObj, transitions);
        }

        public override void OnEnter()
        {
            for (int i = 0; i < cloningOnEnters.Length; i++)
            {
                cloningOnEnters[i].OnAction(gameObj);
            }
            onEnter.Invoke();
        }

        public override void OnUpdate()
        {
            for (int i = 0; i < cloningOnUpdates.Length; i++)
            {
                cloningOnUpdates[i].OnAction(gameObj);
            }
            onUpdate.Invoke();
        }

        public override void OnExit()
        {
            for (int i = 0; i < cloningOnExits.Length; i++)
            {
                cloningOnExits[i].OnAction(gameObj);
            }
            onExit.Invoke();
        }

        public override bool IsTransition(out string next)
        {
            for (int i = 0; i < cloningTransitions.Length; i++)
            {
                if (cloningTransitions[i].CanBeTransit(gameObj, out next))
                    return true;
            }

            next = null;
            return false;
        }
    }
}


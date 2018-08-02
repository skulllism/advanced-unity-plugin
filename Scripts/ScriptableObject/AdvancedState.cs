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

        private Action[] cloningOnEnters;
        private Action[] cloningOnUpdates;
        private Action[] cloningOnExits;

        protected virtual void Awake()
        {
            cloningOnEnters = PrototypeScriptableObject.SetClones(gameObj, onEnters);
            cloningOnUpdates = PrototypeScriptableObject.SetClones(gameObj, onUpdates);
            cloningOnExits = PrototypeScriptableObject.SetClones(gameObj, onExits);
        }

        public override void OnEnter()
        {
            for (int i = 0; i < cloningOnEnters.Length; i++)
            {
                cloningOnEnters[i].OnAction(gameObj);
            }
        }

        public override void OnUpdate()
        {
            for (int i = 0; i < cloningOnUpdates.Length; i++)
            {
                cloningOnUpdates[i].OnAction(gameObj);
            }
        }

        public override void OnExit()
        {
            for (int i = 0; i < cloningOnExits.Length; i++)
            {
                cloningOnExits[i].OnAction(gameObj);
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    /*
 * @brief assembly prototype state
 * @details you can assemble Action, Transition on each slots. all of Prototype ScriptableObject would be clone
 * @author Kay
 * @date 2018-05-31
 * @version 0.0.1
 * */
    public class State : MonoBehaviour
    {
        public string id;

        [Header("Action")]
        public Action[] onEnters;
        public Action[] onUpdates;
        public Action[] onExits;

        [Header("Transition")]
        public Transition[] transitions;

        private Action[] cloningOnEnters;
        private Action[] cloningOnUpdates;
        private Action[] cloningOnExits;
        private Transition[] cloningTransitions;

        public virtual void Init(GameObject gameObj)
        {
            cloningOnEnters = PrototypeScriptableObject.SetClones(gameObj, onEnters);
            cloningOnUpdates = PrototypeScriptableObject.SetClones(gameObj, onUpdates);
            cloningOnExits = PrototypeScriptableObject.SetClones(gameObj, onExits);

            cloningTransitions = PrototypeScriptableObject.SetClones(gameObj, transitions);
        }

        public virtual void OnEnter(GameObject gameObj)
        {
            for (int i = 0; i < cloningOnEnters.Length; i++)
            {
                cloningOnEnters[i].OnAction(gameObj);
            }
        }

        public virtual void OnUpdate(GameObject gameObj)
        {
            for (int i = 0; i < cloningOnUpdates.Length; i++)
            {
                cloningOnUpdates[i].OnAction(gameObj);
            }
        }

        public virtual void OnExit(GameObject gameObj)
        {
            for (int i = 0; i < cloningOnExits.Length; i++)
            {
                cloningOnExits[i].OnAction(gameObj);
            }
        }

        public virtual bool IsTransition(GameObject gameObj, out string next)
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


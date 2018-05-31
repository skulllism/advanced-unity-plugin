using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    namespace ScriptableObject
    {
        /*
     * @brief assembly prototype state
     * @details you can assemble Action, Transition on each slots. all of Prototype ScriptableObject would be clone
     * @author Kay
     * @date 2018-05-31
     * @version 0.0.1
     * */
        [CreateAssetMenu(menuName = "AdvancedUnityPlugin/State")]
        public class State : PrototypeScriptableObject
        {
            public string ID;

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

            public override void Init(ScriptableGameobject obj)
            {
                cloningOnEnters = SetClones(obj, onEnters);
                cloningOnUpdates = SetClones(obj, onUpdates);
                cloningOnExits = SetClones(obj, onExits);

                cloningTransitions = SetClones(obj, transitions);
            }

            public void OnEnter(ScriptableGameobject obj)
            {
                for (int i = 0; i < cloningOnEnters.Length; i++)
                {
                    cloningOnEnters[i].OnAction(obj);
                }
            }

            public void OnUpdate(ScriptableGameobject obj)
            {
                for (int i = 0; i < cloningOnUpdates.Length; i++)
                {
                    cloningOnUpdates[i].OnAction(obj);
                }
            }

            public void OnExit(ScriptableGameobject obj)
            {
                for (int i = 0; i < cloningOnExits.Length; i++)
                {
                    cloningOnExits[i].OnAction(obj);
                }
            }

            public bool IsTransition(ScriptableGameobject obj, out string next)
            {
                for (int i = 0; i < cloningTransitions.Length; i++)
                {
                    if (cloningTransitions[i].CanBeTransit(obj, out next))
                        return true;
                }

                next = null;
                return false;
            }

            public override PrototypeScriptableObject Clone()
            {
                return Instantiate(this);
            }
        }
    }
}


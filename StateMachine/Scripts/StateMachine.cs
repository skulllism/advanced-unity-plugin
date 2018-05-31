using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    namespace ScriptableObject
    {
        /*
       * @brief the scriptable finite state machine
       * @details this is just FSM but all of states can be reuse
       * @author Kay
       * @date 2018-05-31
       * @version 0.0.1
       * */
        [CreateAssetMenu(menuName = "AdvancedUnityPlugin/StateMachine")]
        public class StateMachine : ScriptableComponent
        {
            [Header("State")]
            public State[] states;

            private State[] clones;

            private State current;

            private ScriptableGameobject obj;

            public override void Init(ScriptableGameobject obj)
            {
                this.obj = obj;

                clones = SetClones(obj, states);
            }

            public override PrototypeScriptableObject Clone()
            {
                return Instantiate(this);
            }

            public override void OnReceiveMessage(string message, object[] args)
            {
                if (message == "TransitionToState")
                    TransitionToState(args[0] as string);
            }

            public void TransitionToState(string ID)
            {
                if (current)
                    current.OnExit(obj);

                current = GetState(ID);

                current.OnEnter(obj);
            }

            public override void ManualUpdate(ScriptableGameobject obj)
            {
                if (!current)
                    return;

                string next;
                if (current.IsTransition(obj, out next))
                    TransitionToState(next);
            }

            private State GetState(string ID)
            {
                for (int i = 0; i < clones.Length; i++)
                {
                    if (clones[i].ID == ID)
                        return clones[i];
                }

                return null;
            }
        }
    }
}

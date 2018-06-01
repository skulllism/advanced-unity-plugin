using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
        /*
    * @brief this is a logic that states can transition
    * @details you can assemble Decision on slot. state would be transit by your decision
    * @author Kay
    * @date 2018-05-31
    * @version 0.0.1
    * */
        [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Transition")]
        public class Transition : PrototypeScriptableObject
        {
            [Header("Decision")]
            public Decision[] decisions;

            [Header("Target State ID")]
            public string stateID;

            private Decision[] clones;

            public bool CanBeTransit(ScriptableGameobject obj, out string stateID)
            {
                stateID = this.stateID;

                for (int i = 0; i < decisions.Length; i++)
                {
                    if (decisions[i].Decide(obj) != decisions[i].isTrue)
                        return false;
                }

                return true;
            }

            public override PrototypeScriptableObject Clone()
            {
                return Instantiate(this);
            }

            public override void Init(ScriptableGameobject obj)
            {
                clones = SetClones(obj, decisions);
            }
        }
}


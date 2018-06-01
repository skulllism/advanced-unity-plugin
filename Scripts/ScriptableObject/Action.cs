using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Advanced
{

        /*
    * @brief this is a Action that invoke on state
    * @author Kay
    * @date 2018-05-31
    * @version 0.0.1
    * */
        public abstract class Action : PrototypeScriptableObject
        {
            public abstract void OnAction(ScriptableGameobject actor);
        }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    namespace ScriptableObject
    {
        /*
    * @brief this is the logic that can be used for any decision
    * @author Kay
    * @date 2018-05-31
    * @version 0.0.1
    * */

        public abstract class Decision : PrototypeScriptableObject
        {
            public bool isTrue;
            public abstract bool Decide(ScriptableGameobject actor);
        }
    }
}


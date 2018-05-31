using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    namespace ScriptableObject
    {
        /*
         * @brief component of ScriptableGameObject
         * @author Kay
         * @date 2018-05-31
         * @version 0.0.1
         * */
        public abstract class ScriptableComponent : PrototypeScriptableObject
        {
            public string ID;

            public abstract void OnReceiveMessage(string message, object[] args);
            public abstract void ManualUpdate(ScriptableGameobject obj);
        }
    }
}

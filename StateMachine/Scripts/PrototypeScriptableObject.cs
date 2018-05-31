using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    namespace ScriptableObject
    {
        /*
         * @brief this ScriptableObject can be cloning
         * @author Kay
         * @date 2018-05-31
         * @version 0.0.1
         * */
        public abstract class PrototypeScriptableObject : UnityEngine.ScriptableObject
        {
            public abstract PrototypeScriptableObject Clone();

            public abstract void Init(ScriptableGameobject obj);

            public static T[] SetClones<T>(ScriptableGameobject obj, T[] origin) where T : PrototypeScriptableObject
            {
                T[] clones = new T[origin.Length];

                for (int i = 0; i < origin.Length; i++)
                {
                    Debug.Assert(origin[i] != null, "[AUP] origin prototype is null");
                    clones[i] = origin[i].Clone() as T;
                    clones[i].Init(obj);
                }

                return clones;
            }
        }
    }
}


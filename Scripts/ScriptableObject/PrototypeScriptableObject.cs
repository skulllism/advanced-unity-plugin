using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
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

            public abstract void Init(GameObject gameObj);

            public static T[] SetClones<T>(GameObject gameObj, T[] origin) where T : PrototypeScriptableObject
            {
                T[] clones = new T[origin.Length];

                for (int i = 0; i < origin.Length; i++)
                {
                    Debug.Assert(origin[i] != null, "[AUP] origin prototype is null");
                    clones[i] = origin[i].Clone() as T;
                    clones[i].Init(gameObj);
                }

                return clones;
            }
        }
}


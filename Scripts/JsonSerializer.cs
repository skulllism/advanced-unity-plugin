using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class JsonSerializer<T>
    {
        public string Serialize(T obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}

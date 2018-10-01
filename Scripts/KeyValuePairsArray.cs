using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [System.Serializable]
    public class KeyValuePairsArray
    {
       public KeyValuePairs[] keyValuePairsArray;

        public KeyValuePairsArray(KeyValuePairs[] keyValuePairsArray)
        {
            this.keyValuePairsArray = keyValuePairsArray;
        }
    }

}
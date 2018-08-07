using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class Equipable 
    {
        public string category;
        public string timeStamp;
        public string itemID;

        public Equipable(string category , string itemID , string timeStamp)
        {
            this.category  = category;
            this.itemID    = itemID;
            this.timeStamp = timeStamp;
        }
    }
}
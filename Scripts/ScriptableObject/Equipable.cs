using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class Equipable 
    {
        public string equipType;
        public string itemID;

        public Equipable(string equipType , string itemID)
        {
            this.equipType = equipType;
            this.itemID = itemID;
        }
    }
}
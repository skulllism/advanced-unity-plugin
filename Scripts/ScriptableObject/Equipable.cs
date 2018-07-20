using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Equipment/Equipable")]
    public class Equipable : ScriptableObject
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
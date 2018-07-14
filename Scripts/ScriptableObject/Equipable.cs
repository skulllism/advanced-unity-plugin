using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    [CreateAssetMenu(menuName = "AdvancedUnityPlugin/Equipment/Equipable")]
    public class Equipable : ScriptableObject
    {
        public string itemType;
        public string itemID;
    }
}
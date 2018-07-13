using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedUnityPlugin
{
    public class Slot : MonoBehaviour
    {
        public string equipableType { set; get; }
        public Equipable equipped { set; get; }

        public void Equip(Equipable equipable)
        {
            if (equipable.itemType != equipableType)
                return;

            equipped = equipable;
        }

        public Equipable Unequip()
        {
            if (equipped == null)
                return null;

            Equipable tmp = equipped;
            equipped = null;

            return tmp;
        }
    }
}
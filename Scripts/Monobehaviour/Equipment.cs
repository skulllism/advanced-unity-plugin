using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace AdvancedUnityPlugin
{
    public class Equipment : MonoBehaviour
    {
        public class EquipableUnityEvent : UnityEvent<Equipable> { }

        public EquipableUnityEvent onEquip;
        public EquipableUnityEvent onUnequip;

        public List<Equipable> prefabs;

        public Equipable Default;

        public Transform origin;

        private readonly Dictionary<string, Equipable> equipables = new Dictionary<string, Equipable>();

        private Equipable currentEquipped;

        private Equipable GetOrigin(string itemID)
        {
            foreach (var origin in prefabs)
            {
                if (origin.itemID == itemID)
                    return origin;
            }

            Debug.LogError("Not found origin : " + itemID);
            return null;
        }

        private Equipable GetEquipable(string itemID)
        {
            Equipable equipable = null;
            if (equipables.TryGetValue(itemID, out equipable))
            {
                return equipable;
            }

            //On demand
            equipable = Instantiate(GetOrigin(itemID));
            equipable.transform.SetParent(origin, false);
            equipables.Add(equipable.itemID, equipable);

            return equipable;
        }

        public Equipable GetCurrent()
        {
            if (!currentEquipped)
                Equip(Default);

            return currentEquipped;
        }

        public void Equip(Equipable equipable)
        {
            Debug.Assert(Default != null);

            if (equipable == null)
            {
                Equip(Default);
                return;
            }

            if (currentEquipped)
                Unequip();

            currentEquipped = equipable;
            onEquip.Invoke(currentEquipped);
        }

        public void Equip(string itemID)
        {
            Equip(GetEquipable(itemID));
        }

        public void Unequip()
        {
            if (currentEquipped == null)
                return;

            onUnequip.Invoke(currentEquipped);
            currentEquipped = null;
        }
    }
}
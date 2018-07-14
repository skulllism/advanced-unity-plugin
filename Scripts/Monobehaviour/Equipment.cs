using UnityEngine;
using System.Collections.Generic;
using System;

namespace AdvancedUnityPlugin
{
    public class Equipment : MonoBehaviour
    {
        [Serializable]
        public class SlotField
        {
            public string equipableType;
            public Slot[] slots;
        }

        public class Cursor
        {
            public Slot selected { private set; get; }

            public void Select(Slot slot)
            {
                selected = slot;
            }
        }

        public EquipmentEvent onEquip;
        public EquipmentEvent onUnequip;
        public EquipmentEvent onSelectCursor;

        public SlotField[] slotFields;
        public Equipable[] equipables;

        public Dictionary<string, Cursor> cursors = new Dictionary<string, Cursor>();

        private void Awake()
        {
            foreach (var field in slotFields)
            {
                foreach (var slot in field.slots)
                {
                    slot.equipableType = field.equipableType;
                }

                Cursor cursor = new Cursor();
                cursor.Select(field.slots[0]);
                cursors.Add(field.equipableType, cursor);
            }
        }

        private void OnDestroy()
        {
            foreach (var field in slotFields)
            {
                foreach (var slot in field.slots)
                {
                    slot.equipableType = null;
                    slot.equipped = null;
                }
            }
        }

        private Equipable GetEquipableByName(string name)
        {
            foreach (var equipable in equipables)
            {
                if (equipable.name == name)
                    return equipable;
            }

            return null;
        }

        private Equipable GetEquipableByitemID(string itemID)
        {
            foreach (var equipable in equipables)
            {
                if (equipable.itemID == itemID)
                    return equipable;
            }

            return null;
        }

        private Cursor GetCursor(string itemType)
        {
            return cursors[itemType];
        }

        public void Select(string itemType, Slot slot)
        {
            GetCursor(itemType).Select(slot);

            onSelectCursor.Raise(new Equipable[1] { slot.equipped });
        }

        public void Equip(Equipable equipable)
        {
            Slot selected = GetCursor(equipable.itemType).selected;

            if (selected.equipped != null)
                Unequip(equipable.itemType);

            selected.Equip(equipable);

            onEquip.Raise(new Equipable[1] { equipable });
        }

        public void Equip(string equipableName)
        {
            Equip(GetEquipableByName(equipableName));
        }

        public void EquipByItemID(string itemID)
        {
            Equip(GetEquipableByitemID(itemID));
        }

        public void Unequip(string itemType)
        {
            Equipable equipped = GetCursor(itemType).selected.Unequip();

            onUnequip.Raise(new Equipable[1] { equipped });
        }
    }

}
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
            public EquipmentSlot[] slots;
        }

        public class Cursor
        {
            public EquipmentSlot selected { private set; get; }

            public void Select(EquipmentSlot slot)
            {
                selected = slot;
            }
        }

        public EquipmentEvent onEquip;
        public EquipmentEvent onUnequip;
        public EquipmentEvent onSelectCursor;

        public SlotField[] slotFields;
        public List<Equipable> equipables;

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

        public Cursor GetCursor(string itemType)
        {
            return cursors[itemType];
        }

        private SlotField GetField(string equipType)
        {
            foreach (var field in slotFields)
            {
                if (field.equipableType == equipType)
                    return field;
            }

            return null;
        }

        private bool IsAlreadyEquip(Equipable equipable, out EquipmentSlot alreadyEquipped)
        {
            foreach (var slot in GetField(equipable.equipType).slots)
            {
                if (!slot.equipped)
                    continue;

                if (slot.equipped.itemID == equipable.itemID)
                {
                    alreadyEquipped = slot;
                    return true;
                }
            }

            alreadyEquipped = null;
            return false;
        }

        public void AddEquipable(Equipable equipable)
        {
            equipables.Add(equipable);
        }

        public void RemoveEquipable(Equipable equipable)
        {
            equipables.Remove(equipable);
        }

        public void Select(string itemType, EquipmentSlot slot)
        {
            GetCursor(itemType).Select(slot);

            onSelectCursor.Raise(slot.equipped);
        }

        public void Equip(Equipable equipable)
        {
            EquipmentSlot selected = GetCursor(equipable.equipType).selected;
            Equip(selected, equipable);
        }

        public void Equip(string equipableName)
        {
            Equip(GetEquipableByName(equipableName));
        }

        public void Equip(EquipmentSlot slot , Equipable equipable)
        {
            if (slot.equipped != null)
                Unequip(equipable.equipType);

            EquipmentSlot alreadyEquipped;
            if (IsAlreadyEquip(equipable, out alreadyEquipped))
                alreadyEquipped.Unequip();

            slot.Equip(equipable);

            onEquip.Raise(equipable);
        }

        public void EquipByItemID(string itemID)
        {
            Equip(GetEquipableByitemID(itemID));
        }

        public void Unequip(string itemType)
        {
            Equipable equipped = GetCursor(itemType).selected.Unequip();

            onUnequip.Raise(equipped);
        }
    }

}
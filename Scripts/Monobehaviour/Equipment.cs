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
            public string equipableCategory;
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

        public EquipmentEvent onSlotEquip;
        public EquipmentEvent onSlotUnequip;
        public EquipmentEvent onCursorEquip;
        public EquipmentEvent onCursorUnequip;

        public EquipmentEvent onSelectCursor;

        public SlotField[] slotFields;
        private List<Equipable> equipables = new List<Equipable>();

        public Dictionary<string, Cursor> cursors = new Dictionary<string, Cursor>();

        private void Awake()
        {
            foreach (var field in slotFields)
            {
                foreach (var slot in field.slots)
                {
                    slot.equipableCategory = field.equipableCategory;
                }

                Cursor cursor = new Cursor();
                cursor.Select(field.slots[0]);
                cursors.Add(field.equipableCategory, cursor);
            }
        }

        private void OnDestroy()
        {
            foreach (var field in slotFields)
            {
                foreach (var slot in field.slots)
                {
                    slot.equipableCategory = null;
                    slot.equipped = null;
                }
            }
        }

        public Equipable GetEquipableByitemTimeStamp(string itemTimeStamp)
        {
            foreach (var equipable in equipables)
            {
                if (equipable.timeStamp == itemTimeStamp)
                    return equipable;
            }

            return null;
        }

        public Cursor GetCursor(string equipCategory)
        {
            return cursors[equipCategory];
        }

        private SlotField GetField(string equipCategory)
        {
            foreach (var field in slotFields)
            {
                if (field.equipableCategory == equipCategory)
                    return field;
            }

            return null;
        }

        private bool IsAlreadyEquip(Equipable equipable, out EquipmentSlot alreadyEquipped)
        {
            foreach (var slot in GetField(equipable.category).slots)
            {
                if (slot.equipped == null)
                    continue;

                if (slot.equipped.timeStamp == equipable.timeStamp)
                {
                    alreadyEquipped = slot;
                    return true;
                }
            }

            alreadyEquipped = null;
            return false;
        }

        public void AddEquipable(string equipType, string itemID , string timeStamp)
        {
            Equipable equipable = new Equipable(equipType, itemID , timeStamp);

            if (equipables.Contains(equipable))
                return;

            equipables.Add(equipable);
        }

        public void RemoveEquipable(string itemTimeStamp)
        {
            Equipable equipable = GetEquipableByitemTimeStamp(itemTimeStamp);

            if (equipable == null)
                return;

            RemoveEquipableOfSlot(equipable);

            equipables.Remove(equipable);
        }

        private void RemoveEquipableOfSlot(Equipable equipable)
        {
            foreach (var field in slotFields)
            {
                if (field.equipableCategory != equipable.category)
                    continue;
                
                foreach (var slot in field.slots)
                {
                    if(slot.equipped == equipable)
                    {
                        Unequip(slot);
                        break;
                    }
                }
            }
        }

        public void Select(string equipType, EquipmentSlot slot)
        {
            GetCursor(equipType).Select(slot);

            onSelectCursor.Raise(slot.equipped);
        }

        public void EquipByItemTimeStamp(string itemTimeStamp)
        {
            Equip(GetEquipableByitemTimeStamp(itemTimeStamp));
        }

        public void Equip(Equipable equipable)
        {
            EquipmentSlot selected = GetCursor(equipable.category).selected;
            Equip(selected, equipable);

            onCursorEquip.Raise(equipable);
        }

        public void Equip(EquipmentSlot slot , Equipable equipable)
        {
            if (slot.equipped != null)
                Unequip(slot);

            EquipmentSlot alreadyEquipped;
            if (IsAlreadyEquip(equipable, out alreadyEquipped))
                alreadyEquipped.Unequip();

            slot.Equip(equipable);

            onSlotEquip.Raise(equipable);
        }

        public void Unequip(string category)
        {
            Equipable equipped = GetCursor(category).selected.Unequip();

            onCursorUnequip.Raise(equipped);
        }

        public void Unequip(EquipmentSlot slot)
        {
            Equipable equipped = slot.Unequip();

            onSlotUnequip.Raise(equipped);
        }
    }
}
using Assets.Scripts.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField]
        private GameObject slotHolder;

        private List<Slot> slots;


        private void Awake()
        {
            slots = new List<Slot>();

            foreach (Slot slot in slotHolder.GetComponentsInChildren<Slot>())
            {
                slots.Add(slot);
            }    
        }

        public void UseItemInSlot(InputAction.CallbackContext context)
        {
            int.TryParse(context.control.name, out var number);
            if (number is > 0 and < 10)
            {
                UseItemInSlot(number - 1);
            }
        }

        public void UseItemInSlot(int slotIndex)
        {
            Debug.Log("Test");
            Slot slot = GetSlotByIndex(slotIndex);
            if (slot.Item == null)
            {
                return;
            }

            slot.Item.UseItem(GameManager.Instance.Player);
            slot.ClearSlot();
        }

        public bool AddItem(ItemType itemType)
        {
            int freeIndex = GetFirstAvailableSlotIndex();
            if (freeIndex < 0)
            {
                return false;
            }

            GetSlotByIndex(freeIndex).SetItem(itemType);

            return true;
        }

        public Slot GetSlotByIndex(int index)
        {
            return slots[index];
        }

        public List<Slot> Slots => slots;

        private int GetFirstAvailableSlotIndex()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].Item == null)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] protected int _coins;
    public int Coines => _coins;

    [Header("____________________________________")]
    [SerializeField] protected List<InventorySlot> _inventorySlots;
    [SerializeField] protected List<InventorySlot> _equipSlots;
    [SerializeField] protected List<InventorySlot> _beltSlots;
    public List<InventorySlot> InventorySlots => _inventorySlots;
    public List<InventorySlot> EquipSlots => _equipSlots;
    public List<InventorySlot> BeltSlots => _beltSlots;
    public int InventorySize => InventorySlots.Count;
    public int EquipSlotsCount => EquipSlots.Count;
    public int BeltSlotsCount => BeltSlots.Count;

    public static UnityAction OnEquipSlotChanged;
    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int bagSlots, int equpslots, int beltsslots,  int coins)
    {
        _coins = coins;

        CreatePlayerInventory(bagSlots, equpslots, beltsslots);        
    }
    public InventorySystem(int slot)
    {
        CreateInventory(slot);
    }
    private void CreatePlayerInventory(int bagslots, int equpslots, int beltsslots)
    {
        _inventorySlots = new List<InventorySlot>(bagslots);

        for (int i = 0; i < bagslots; i++)
        {
            _inventorySlots.Add(new InventorySlot());
        }
        _equipSlots = new List<InventorySlot>(equpslots);

        for (int i = 0; i < equpslots; i++)
        {
            _equipSlots.Add(new InventorySlot());
        }

        _beltSlots = new List<InventorySlot>(beltsslots);

        for (int i = 0; i < beltsslots; i++)
        {
            _beltSlots.Add(new InventorySlot());
        }

    }
    private void CreateInventory(int bagslots)
    {
        _inventorySlots = new List<InventorySlot>(bagslots);

        for (int i = 0; i < bagslots; i++)
        {
            _inventorySlots.Add(new InventorySlot());
        }
    }
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) // Существует ли предмет в инвинтаре
        {
            foreach (var slot in _inventorySlots)
            {
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }

        }

        if (HasFreeSlot(out InventorySlot freeSlot)) // Берем первый свободный слот
        {
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
        }
        return false;
    }
    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot) // проверяем если в наших слотах предмет что б его добавить
    {
        invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList(); //Если да, получаю их список
        return invSlot.Count == 0 ? false : true;
    }
    public bool HasFreeSlot(out InventorySlot freeSlot) //получаем первый свободный слот
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }
    public bool CheckInventoryRemaining(Dictionary<InventoryItemData, int> shoppingCart)
    {

        var clonedSystem = new InventorySystem(this.InventorySize);

        for (int i = 0; i < InventorySize; i++)
        {

            clonedSystem.InventorySlots[i].AssignItem(this.InventorySlots[i].ItemData, this.InventorySlots[i].StackSize);
        }
        foreach (var kvp in shoppingCart)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                if (!clonedSystem.AddToInventory(kvp.Key, 1)) return false;
            }
        }
        return true;
    }
    public Dictionary<InventoryItemData, int> GettAllItemsHeld()
    {
        var distinctItems = new Dictionary<InventoryItemData, int>();

        foreach (var item in _inventorySlots)
        {
            if (item.ItemData == null) continue;

            if (!distinctItems.ContainsKey(item.ItemData)) distinctItems.Add(item.ItemData, item.StackSize);
            else distinctItems[item.ItemData] += item.StackSize;
        }
        return distinctItems;
    }
    public void RemoveItemsFromInventory(InventoryItemData data, int amount)
    {
        //Удаляем определенное количество предметов из инвентаря
        if (ContainsItem(data, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot)
            {
                var stackSize = slot.StackSize;

                if (stackSize > amount) slot.RemoveFromStack(amount);
                else
                {
                    slot.RemoveFromStack(stackSize);
                    amount -= stackSize;
                }
                Debug.Log("remove item from slot");
                OnInventorySlotChanged?.Invoke(slot);
            }
        }
    }   
    public void SpendGold(int basketTotal)
    {
        _coins -= basketTotal;
    }
    public void GainGold(int price)
    {
        _coins += price;
    }
  



    

    



  


  
  
  
}

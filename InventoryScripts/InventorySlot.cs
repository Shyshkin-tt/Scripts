using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot, ISerializationCallbackReceiver
{

    public InventorySlot(InventoryItemData source, int amount) //Создание занимаемого слота
    {
        _itemData = source;
        _itemID = _itemData.ID;
        _tier = _itemData.Tier;
        _nameItem = _itemData.DisplayName;
        _stackSize = amount;
        _slotType = _itemData.SlotType;
        _itemType = _itemData.ItemType;

        _ip = _itemData.ItemPower;
        _hp = _itemData.Health;
        _mp = _itemData.Mana;
        _pd = _itemData.PhysicDamage;
        _md = _itemData.MagicDamage;
        _pdef = _itemData.PhysicDefence;
        _mdef = _itemData.MagicDefence;
        _hpRec = _itemData.HealthRecovery;
        _mpRec = _itemData.ManaRecovery;
    }

    public InventorySlot() //создает пустой слот инвентаря
    {
        ClearSlot();
    }  
    public void UpdateInventorySlot(InventoryItemData data, int amount) //Обновление слота
    {
        _itemData = data;
        _itemID = _itemData.ID;
        _tier = _itemData.Tier;
        _nameItem = _itemData.DisplayName;
        _stackSize = amount;
        _slotType = _itemData.SlotType;
        _itemType = _itemData.ItemType;

        _ip = _itemData.ItemPower;
        _hp = _itemData.Health;
        _mp = _itemData.Mana;
        _pd = _itemData.PhysicDamage;
        _md = _itemData.MagicDamage;
        _pdef = _itemData.PhysicDefence;
        _mdef = _itemData.MagicDefence;
        _hpRec = _itemData.HealthRecovery;
        _mpRec = _itemData.ManaRecovery;        
    }
    
    // проверка хватает ли места в слоте стака для количества которые мы туда хочем положить
    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - _stackSize;

        return EnoughRoomLeftInStack(amountToAdd);
    }

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {       
        if (_itemData == null || _itemData != null && _stackSize + amountToAdd <= _itemData.MaxStackSize) return true;
        else return false;
    }

    public bool SplitStack(out InventorySlot splitStack)
    {
        if (_stackSize <= 1)//Достаточно ли что б разделить
        {
            splitStack = null;
            return false;
        }
        int halfStack = Mathf.RoundToInt(_stackSize / 2); //деление стака пополам
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(_itemData, halfStack); //создание копии с половиной стака
        return true;
    }
    public void OnBeforeSerialize()
    {

    }
    public void OnAfterDeserialize()
    {
        if (_itemID == -1) return;// если предмет отсутствует, то ничего не делаем
        var db = Resources.Load<Database>("Database");// загружаем базу данных
        _itemData = db.GetItem(_itemID);// получаем данные о предмете по его ID
        
    }
    
   
}


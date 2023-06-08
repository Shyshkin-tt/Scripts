using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot, ISerializationCallbackReceiver
{

    public InventorySlot(InventoryItemData source, int amount) //�������� ����������� �����
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

    public InventorySlot() //������� ������ ���� ���������
    {
        ClearSlot();
    }
    //public void AssignSlot(InventorySlot invSlot) // ��������� ������� � ����
    //{
    //    if (_itemData == invSlot.ItemData) AddToStack(invSlot._stackSize); // �������� �� ������� � ����� �������� 
    //    else
    //    {
    //        _itemData = invSlot._itemData;
    //        _itemID = _itemData.ID;
    //        _tier = _itemData.Tier;
    //        _nameItem = _itemData.DisplayName;
    //        _stackSize = 0;
    //        _slotType = _itemData.SlotType;
    //        _itemType = _itemData.ItemType;

    //        _ip = _itemData.ItemPower;
    //        _hp = _itemData.Health;
    //        _mp = _itemData.Mana;
    //        _pd = _itemData.PhysicDamage;
    //        _md = _itemData.MagicDamage;
    //        _pdef = _itemData.PhysicDefence;
    //        _mdef = _itemData.MagicDefence;
    //        _hpRec = _itemData.HealthRecovery;
    //        _mpRec = _itemData.ManaRecovery;
    //        AddToStack(invSlot._stackSize);
           
    //    }
    //}
    public void UpdateInventorySlot(InventoryItemData data, int amount) //���������� �����
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
    
    // �������� ������� �� ����� � ����� ����� ��� ���������� ������� �� ���� ����� ��������
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
        if (_stackSize <= 1)//���������� �� ��� � ���������
        {
            splitStack = null;
            return false;
        }
        int halfStack = Mathf.RoundToInt(_stackSize / 2); //������� ����� �������
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(_itemData, halfStack); //�������� ����� � ��������� �����
        return true;
    }
    public void OnBeforeSerialize()
    {

    }
    public void OnAfterDeserialize()
    {
        if (_itemID == -1) return;// ���� ������� �����������, �� ������ �� ������
        var db = Resources.Load<Database>("Database");// ��������� ���� ������
        _itemData = db.GetItem(_itemID);// �������� ������ � �������� �� ��� ID
        
    }
    
    public void SetNameSlotOnHolder(string nameSlot, GameObject equipobj, InventorySlot_UI slotUI)
    {
        _nameSlot = nameSlot;
        _equipObject = equipobj;
        _equipSlotUI = slotUI;
    }
}


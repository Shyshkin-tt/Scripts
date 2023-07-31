using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// Класс для хранения данных предмета в слоте инвентаря
public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [NonSerialized] protected InventoryItemData _itemData;
    [SerializeField] protected string _nameSlot;
    [SerializeField] protected string _nameItem;
    [SerializeField] protected string _classItem;
    [SerializeField] protected GameObject _equipObject;
    [SerializeField] protected InventorySlot_UI _equipSlotUI;
    [SerializeField] protected int _itemID = -1;
    [SerializeField] protected int _tier;
    [SerializeField] protected int _stackSize;
    [SerializeField] protected string _itemType;
    [SerializeField] protected string _slotType;

    [Header("Stats")]
    [SerializeField] protected int _ip;
    [SerializeField] protected int _hp;
    [SerializeField] protected int _mp;
    [SerializeField] protected int _pd;
    [SerializeField] protected int _md;
    [SerializeField] protected int _pdef;
    [SerializeField] protected int _mdef;
    [SerializeField] protected int _hpRec;
    [SerializeField] protected int _mpRec;
    public InventoryItemData ItemData => _itemData;// Геттер для доступа к приватному полю itemData    
    public string NameSlot => _nameSlot;
    public string NameItem => _nameItem;
    public string ClassItem => _classItem;
    public GameObject OnEquip => _equipObject;
    public int StackSize => _stackSize;// Геттер для доступа к приватному полю stackSize
    public void ClearSlot()// Очищает слот
    {
        _itemData = null;
        _nameItem = "";
        _classItem = "";
        _itemID = -1;
        _stackSize = -1;
        _slotType = "";
        _itemType = "";

        _ip = -1;
        _hp = -1;
        _mp = -1;
        _pd = -1;
        _md = -1;
        _pdef = -1;
        _mdef = -1;
        _hpRec = -1;
        _mpRec = -1;
       
    }
    public void AssignItem(InventorySlot invSlot)// Присваивает предмет из InventorySlot
    {
        if (_itemData == invSlot.ItemData) AddToStack(invSlot._stackSize); // Если предмет уже есть в слоте, добавляет количество к стаку
        else
        {
            _itemData = invSlot._itemData; // Присваивает новый предмет
            _itemID = _itemData.ID; // Присваивает новый идентификатор предмета
            _nameItem = _itemData.DisplayName;
            _classItem = _itemData.ItemClass;
            _stackSize = 0;
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
            AddToStack(invSlot._stackSize);
        }
        
    }
    public void AssignItem(InventoryItemData data, int amount) // Присваивает предмет из InventoryItemData и количества
    {
        // Если предмет уже есть в слоте, добавляет количество к стаку
        if (_itemData == data) AddToStack(amount);
        else
        {
            _itemData = data; // Присваивает новый предмет
            _itemID = data.ID; // Присваивает новый идентификатор предмета
            _nameItem = data.DisplayName;
            _classItem = data.ItemClass;
            _stackSize = 0;
            _slotType = data.SlotType;
            _itemType = data.ItemType;

            _ip = data.ItemPower;
            _hp = data.Health;
            _mp = data.Mana;
            _pd = data.PhysicDamage;
            _md = data.MagicDamage;
            _pdef = data.PhysicDefence;
            _mdef = data.MagicDefence;
            _hpRec = data.HealthRecovery;
            _mpRec = data.ManaRecovery;
            AddToStack(amount);
        }
    }   

    public void AddToStack(int amount)// Добавляет количество предметов в стак
    {
        _stackSize += amount;
    }    
    public void RemoveFromStack(int amount)// Удаляет количество предметов из стака
    {
        _stackSize -= amount;
        if (_stackSize <= 0) ClearSlot();
    }

    public void SetSlot(string slot, GameObject equipslot, InventorySlot_UI slotUI)
    {
        _nameSlot = slot;
        _equipObject = equipslot;
        _equipSlotUI = slotUI;
    }

    public void LoadSlot(GameObject equipslot, InventorySlot_UI slotUI)
    {
        _equipObject = equipslot;
        _equipSlotUI = slotUI;
    }
    public void OnBeforeSerialize()
    {

    }
    public void OnAfterDeserialize()
    {
        if (_itemID == -1) return;// если предмет отсутствует, то ничего не делаем
        var db = Resources.Load<Database>("ItemDatabase");// загружаем базу данных
        _itemData = db.GetItem(_itemID);// получаем данные о предмете по его ID

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InventoryDisplay : InventoryController
{
      
    [SerializeField] private TextMeshProUGUI _playerGoldText;    

    [Header("Equipment slots")]
    public InventorySlot_UI _head;
    public InventorySlot_UI _armor;
    public InventorySlot_UI _shoes;
    public InventorySlot_UI _rHand;
    public InventorySlot_UI _lHand;
    public InventorySlot_UI _bag;
    public InventorySlot_UI _belt;

    [Header("Belt slots")]
    public InventorySlot_UI _slotOne;
    public InventorySlot_UI _slotTwo;
    public InventorySlot_UI _slotThree;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI _ip;
    [SerializeField] private TextMeshProUGUI _hp;
    [SerializeField] private TextMeshProUGUI _mp;
    [SerializeField] private TextMeshProUGUI _pdmg;
    [SerializeField] private TextMeshProUGUI _mdmg;
    [SerializeField] private TextMeshProUGUI _as;
    [SerializeField] private TextMeshProUGUI _pdef;
    [SerializeField] private TextMeshProUGUI _mdef;
    [SerializeField] private TextMeshProUGUI _hpRec;
    [SerializeField] private TextMeshProUGUI _mpRec;

    [Header("Belt and Bag")]
    public InventorySlot_UI[] _equipmentSlots;
    public InventorySlot_UI[] _slotsBelt;
    public InventorySlot_UI[] _slotsBag;// Сериализованное поле для хранения ссылок на ячейки инвентаря   


    private void OnEnable()
    {
        InventoryHolder.OnInventorySlotChanged += RefreshDisplay;
    }
    private void OnDisable()
    {
        InventoryHolder.OnInventorySlotChanged -= RefreshDisplay;
    }
    protected override void Start()
    {
        base.Start();
        RefreshDisplay();
    }

    private void Update()
    {
        UpdateStats();
    }
     private void RefreshDisplay()
    {
        if (_uiController != null)
        {
            _characterCharacteristics = _uiController._holder.Characteristics;
            _inventorySystem = _uiController._holder.Inventory;
            _experienceSystem = _uiController._holder.Experience;
            _spellSystem = _uiController._holder.Spells;
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;

        }
        else Debug.Log($"No inventory assigned to {this.gameObject}");

        // Обновляем отображение инвентаря      
        AssignSlot(_inventorySystem);
    }
    public override void AssignSlot(InventorySystem invToDisplay)// Метод для присваивания ячеек инвентаря отображению
    {

        inventorySlotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (_slotsBag.Length != _inventorySystem.InventorySize) Debug.Log($"No inventory assigned to {this.gameObject}");

        for (int i = 0; i < _inventorySystem.InventorySize; i++)
        {
            inventorySlotDictionary.Add(_slotsBag[i], _inventorySystem.InventorySlots[i]);
            _slotsBag[i].Init(_inventorySystem.InventorySlots[i]);
        }    

        equipSlotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (_equipmentSlots.Length != _inventorySystem.EquipSlotsCount) Debug.Log($"No inventory assigned to {this.gameObject}");

        for (int i = 0; i < _inventorySystem.EquipSlotsCount; i++)
        {
            inventorySlotDictionary.Add(_equipmentSlots[i], _inventorySystem.EquipSlots[i]);
            _equipmentSlots[i].Init(_inventorySystem.EquipSlots[i]);
        }

        beltSlotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (_slotsBelt.Length != _inventorySystem.BeltSlotsCount) Debug.Log($"No inventory assigned to {this.gameObject}");

        for (int i = 0; i < _inventorySystem.BeltSlotsCount; i++)
        {
            inventorySlotDictionary.Add(_slotsBelt[i], _inventorySystem.BeltSlots[i]);
            _slotsBelt[i].Init(_inventorySystem.BeltSlots[i]);
        }
    }   

    private void UpdateStats()
    {
        _playerGoldText.text = $"Coins: {_uiController._holder.Inventory.Coines}";
        _ip.text = $"{_uiController._holder.Characteristics.ItemPower}";
        _hp.text = $"{_uiController._holder.Characteristics.Health}";
        _mp.text = $"{_uiController._holder.Characteristics.Mana}";
        _pdmg.text = $"{_uiController._holder.Characteristics.PDmg}";
        _mdmg.text = $"{_uiController._holder.Characteristics.MDmg}";
        _as.text = $"{_uiController._holder.Characteristics.AttackSpeed}";
        _pdef.text = $"{_uiController._holder.Characteristics.PDef}";
        _mdef.text = $"{_uiController._holder.Characteristics.MDef}";
        _hpRec.text = $"{_uiController._holder.Characteristics.HPRec}";
        _mpRec.text = $"{_uiController._holder.Characteristics.MPRec}";
    }
}

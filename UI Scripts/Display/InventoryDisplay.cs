using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InventoryDisplay : InventoryController
{
    [SerializeField] private InventoryHolder _playerInventoryHolder;    
    [SerializeField] private TextMeshProUGUI _playerGoldText;    

    [Header("Equipment")]
    public InventorySlot_UI _head;
    public InventorySlot_UI _armor;
    public InventorySlot_UI _shoes;
    public InventorySlot_UI _rHand;
    public InventorySlot_UI _lHand;
    public InventorySlot_UI _bag;
    public InventorySlot_UI _belt;

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
        if (_playerInventoryHolder != null)
        {
            _inventorySystem = _playerInventoryHolder.Inventory;
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;

        }
        else Debug.Log($"No inventory assigned to {this.gameObject}");

        // Обновляем отображение инвентаря      
        AssignSlot(_inventorySystem);
    }
    public override void AssignSlot(InventorySystem invToDisplay)// Метод для присваивания ячеек инвентаря отображению
    {

        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (_slotsBag.Length != _inventorySystem.InventorySize) Debug.Log($"No inventory assigned to {this.gameObject}");

        for (int i = 0; i < _inventorySystem.InventorySize; i++)
        {
            slotDictionary.Add(_slotsBag[i], _inventorySystem.InventorySlots[i]);
            _slotsBag[i].Init(_inventorySystem.InventorySlots[i]);
        }    

        equipSlotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (_equipmentSlots.Length != _inventorySystem.EquipSlotsCount) Debug.Log($"No inventory assigned to {this.gameObject}");

        for (int i = 0; i < _inventorySystem.EquipSlotsCount; i++)
        {
            slotDictionary.Add(_equipmentSlots[i], _inventorySystem.EquipSlots[i]);
            _equipmentSlots[i].Init(_inventorySystem.EquipSlots[i]);
        }

        if (_slotsBelt.Length != _inventorySystem.BeltSlotsCount) Debug.Log($"No inventory assigned to {this.gameObject}");

        for (int i = 0; i < _inventorySystem.BeltSlotsCount; i++)
        {
            slotDictionary.Add(_slotsBelt[i], _inventorySystem.BeltSlots[i]);
            _slotsBelt[i].Init(_inventorySystem.BeltSlots[i]);
        }
    }   

    private void UpdateStats()
    {
        _playerGoldText.text = $"Coins: {_playerInventoryHolder.Inventory.Coines}";
        _ip.text = $"{_playerInventoryHolder.Inventory.ItemPower}";
        _hp.text = $"{_playerInventoryHolder.Inventory.Health}";
        _mp.text = $"{_playerInventoryHolder.Inventory.Mana}";
        _pdmg.text = $"{_playerInventoryHolder.Inventory.PDmg}";
        _mdmg.text = $"{_playerInventoryHolder.Inventory.MDmg}";
        _as.text = $"{_playerInventoryHolder.Inventory.AttackSpeed}";
        _pdef.text = $"{_playerInventoryHolder.Inventory.PDef}";
        _mdef.text = $"{_playerInventoryHolder.Inventory.MDef}";
        _hpRec.text = $"{_playerInventoryHolder.Inventory.HPRec}";
        _mpRec.text = $"{_playerInventoryHolder.Inventory.MPRec}";
    }
}

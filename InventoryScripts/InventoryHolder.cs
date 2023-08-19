using System;
using System.Collections.Generic;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    public Database _database;
    public SkinnedMeshRenderer playerSkin;
    public InventoryHolder _holder;
    [SerializeField] private UIController _uiPlayer;
    public UIController UIController => _uiPlayer;

    [SerializeField] private List<string> _classList = new List<string>(5); // Инициализируйте с нужным размером
    [SerializeField] private List<string> _itemList = new List<string>(5); // Инициализируйте с нужным размером

    private string _name = "";
    private string _location = "";
    private bool _inCombat;
    
    private int _ip = 0;
    private int _hp = 250;
    private int _mp = 100;
    private int _pd = 10;
    private int _md = 0;
    private float _as = 5;
    private float _ms = 6f;
    private int _pdef = 10;
    private int _mdef = 0;
    private int _hpValue = 0;
    private int _mpValue = 0;
    private int _hpRec = 2;
    private int _mpRec = 2;

    private Vector3 _spawnCoords;
    private Vector3 _curentCoordinats;

    private float _timeExitCombat;

    [Header("_____________SPELLS_________________")]
    private SpellData _spellQ;
    private float _cooldownQ;
    private SpellData _spellW;
    private float _cooldownW;
    private SpellData _spellE;
    private float _cooldownE;
    private SpellData _spellR;
    private float _cooldownR;

    [Header("____________INVENTORY_______________")]
    private int _coins = 0;

    public int _inventorySize;
    public int _equipSlots;
    public int _bagSlots;
    [Header("____________________________________")]

    public GameObject[] _equipOnPlayer;
    public InventorySlot_UI[] _slotUI;
    
    private int _totalXp;
    private List<ItemsXP> _itemClassList;
    private List<ItemsXP> _itemNameList;
    [Header("____________________________________")]
    [SerializeField] protected CharacterCharacteristics _characteristics;
    public CharacterCharacteristics Characteristics => _characteristics;
    public Vector3 CurrentCoordinats => _curentCoordinats;

    [Header("____________________________________")]
    [SerializeField] protected InventorySystem _inventory;
    public InventorySystem Inventory => _inventory;

    [Header("____________________________________")]
    [SerializeField] protected SpellSystem _spellSystem;
    public SpellSystem Spells => _spellSystem;

    [Header("____________________________________")]
    [SerializeField] protected ExperienceSystem _experience;
    public ExperienceSystem Experience => _experience;

    public static UnityAction OnInventorySlotChanged;
    

    protected virtual void Awake()
    {
        SaveLoadGameData.OnLoadCharacteristics += LoadCharacteristics;
        SaveLoadGameData.OnLoadInventory += LoadInventory;
        SaveLoadGameData.OnLoadPayerXP += LoadPlayerXP;

        InventorySystem.OnEquipSlotChanged += GetCurrentItems;
        ExperienceSystem.LvlUp += ChangeBonusStats;

        playerSkin = GetComponentInChildren<SkinnedMeshRenderer>();
        _uiPlayer = FindObjectOfType<UIController>();

        _characteristics = new CharacterCharacteristics(_name, _location, _inCombat, _timeExitCombat, _ip, _hp, _mp, _pd, _md, _as, _ms,
        _pdef, _mdef, _hpValue, _mpValue, _hpRec, _mpRec, _spawnCoords, _curentCoordinats, playerSkin, _holder);

        _inventory = new InventorySystem(_inventorySize, _equipSlots, _bagSlots,  _coins);

        _spellSystem = new SpellSystem(_spellQ, _cooldownQ, _spellW, _cooldownW, _spellE, _cooldownE);

        _experience = new ExperienceSystem(_totalXp, _itemClassList, _database);


        _characteristics.SetValue();

        if (Inventory.EquipSlots[0].NameSlot == null)
            SetNewSlot();
    }

    private void LoadCharacteristics(CharacteristicsSave data)
    {
        _characteristics = data.playerCharacteristics.Characteristics;
        Characteristics.SetSkinAndHolder(playerSkin, _holder);
    }

    private void ChangeBonusStats(ItemsXP item)
    {
        var oldStatsClass = Experience.GetBonusStatsClass(item.NameClass);
        Characteristics.BonusStatMinus(oldStatsClass);
        //var oldStatsName = Experience.GetBonusStatsName(item.ID);
        //Inventory.BonusStatMinus(oldStatsName);

        item.UpdateItemBonusStat();

        var newStatsClass = Experience.GetBonusStatsClass(item.NameClass);
        Characteristics.BonusStatAdd(newStatsClass);
        //var newStatsName = Experience.GetBonusStatsName(item.ID);
        //Inventory.BonusStatAdd(newStatsName);
    }
    private void GetCurrentItems()
    {
        _classList[0] = _inventory.EquipSlots[3].ClassItem;
        _classList[1] = _inventory.EquipSlots[5].ClassItem;
        _classList[2] = _inventory.EquipSlots[1].ClassItem;
        _classList[3] = _inventory.EquipSlots[4].ClassItem;
        _classList[4] = _inventory.EquipSlots[7].ClassItem;

        _itemList[0] = _inventory.EquipSlots[3].NameItem;
        _itemList[1] = _inventory.EquipSlots[5].NameItem;
        _itemList[2] = _inventory.EquipSlots[1].NameItem;
        _itemList[3] = _inventory.EquipSlots[4].NameItem;
        _itemList[4] = _inventory.EquipSlots[7].NameItem;
    }

    private void Start()
    {
        SaveAndLoadManager._characteristicsData.playerCharacteristics = new CharacteristicsSaveData(_characteristics);
        SaveAndLoadManager._inventoryData.playerInventory = new InventorySaveData(_inventory);
        SaveAndLoadManager._playerXPData.playerExperience = new ExperienceSaveData(_experience);

        UIController.LoadXpForItems?.Invoke();
    }

    private void Update()
    {
        _characteristics.GetCurentCoords(_holder);
        _curentCoordinats = _characteristics.CurrentCoordinats;

        if (Characteristics.InCombat) Characteristics.TimeExitCombatLeft();
        if (Characteristics.LastTimeHit <= 0) Characteristics.ExitCombat();
    }

    public void SetNameAndLoc(string name, string loc, Vector3 spawn)
    {
        _name = name;
        _location = loc;
        _spawnCoords = spawn;        
    }
    protected virtual void LoadInventory(InventorySave data)
    {        
        _inventory = data.playerInventory.InvSys;

        //

        if (_inventory.EquipSlots[0].OnEquip == null)
            LoadSlots();        

        foreach (var slot in Inventory.EquipSlots)
        {
            if (slot.ItemData != null)
            {
                EquipOnPlayer(slot);
            }
        }        
    }
    protected virtual void LoadPlayerXP(PlayerExperienceSave playerXP)
    {
        _experience = playerXP.playerExperience.XPSys;

        _experience.SetTotalXP(playerXP.playerExperience.XPSys.TotalXp);

        var data = _database.GetItemDatabase();

        foreach (InventoryItemData itemData in data)
        {
            bool itemClassFound = false;           

            foreach (var itemClass in Experience.ItemListClass)
            {
                if (itemData.ItemClass == itemClass.NameClass)
                {
                    itemClassFound = true;                    
                    break;
                }
            }
            if (!itemClassFound)
            {               
                Experience.AddNewClassItem(itemData);
            }
            

            //foreach (var itemName in Experience.ItemListName)
            //{
            //    if (itemData.DisplayName == itemName.NameItem)
            //    {
            //        itemNameFound = true;
            //        break;
            //    }
            //}
            //if (!itemNameFound)
            //{               
            //    Experience.AddNewItem(itemData);
            //    SaveAndLoadManager.SavePlayerXP();
            //}
        }        
    }
    private void LoadSlots()
    {
        _inventory.EquipSlots[0].LoadSlot(null, _slotUI[0]);
        _inventory.EquipSlots[1].LoadSlot(_equipOnPlayer[0], _slotUI[1]);
        _inventory.EquipSlots[2].LoadSlot(null, _slotUI[2]);
        _inventory.EquipSlots[3].LoadSlot(_equipOnPlayer[3], _slotUI[3]);
        _inventory.EquipSlots[4].LoadSlot(_equipOnPlayer[1], _slotUI[4]);
        _inventory.EquipSlots[5].LoadSlot(_equipOnPlayer[4], _slotUI[5]);
        _inventory.EquipSlots[6].LoadSlot(null, _slotUI[6]);
        _inventory.EquipSlots[7].LoadSlot(_equipOnPlayer[2], _slotUI[7]);
        _inventory.EquipSlots[8].LoadSlot(null, _slotUI[8]);
    }
    public void SetNewSlot()
    {
        _inventory.EquipSlots[0].SetSlot("Neck", null, _slotUI[0]);
        _inventory.EquipSlots[1].SetSlot("Head", _equipOnPlayer[0], _slotUI[1]);
        _inventory.EquipSlots[2].SetSlot("Spine", null, _slotUI[2]);
        _inventory.EquipSlots[3].SetSlot("RHand", _equipOnPlayer[3], _slotUI[3]);
        _inventory.EquipSlots[4].SetSlot("Armor", _equipOnPlayer[1], _slotUI[4]);
        _inventory.EquipSlots[5].SetSlot("LHand", _equipOnPlayer[4], _slotUI[5]);
        _inventory.EquipSlots[6].SetSlot("Belt", null, _slotUI[6]);
        _inventory.EquipSlots[7].SetSlot("Shoes", _equipOnPlayer[2], _slotUI[7]);
        _inventory.EquipSlots[8].SetSlot("Bag", null, _slotUI[8]);
    }   
    public void EquipOnPlayer(InventorySlot itemData)
    {
        if (itemData.OnEquip != null)
        {
            GameObject item = Instantiate(itemData.ItemData.ItemPrefab, itemData.OnEquip.transform);

            SkinnedMeshRenderer skinRender = item.GetComponentInChildren<SkinnedMeshRenderer>();
            InventorySystem.OnEquipSlotChanged?.Invoke();
            if (itemData.NameSlot != "RHand")
            {
                skinRender.bones = playerSkin.bones;
                skinRender.rootBone = playerSkin.rootBone;                
            }           
        }
    }
   
    public void RemoveFromPlayer(InventorySlot itemData)
    {      
        Destroy(itemData.OnEquip.transform.GetChild(0).gameObject);
    }  

    public void SaveFromHolder()
    {
        SaveAndLoadManager.SaveCharacteristics(Characteristics.Name);
        SaveAndLoadManager.SaveInventory(Characteristics.Name);
        SaveAndLoadManager.SavePlayerXP(Characteristics.Name);        
    }
    public void LoadFromHolder()
    {
        SaveAndLoadManager.LoadCharacteristics(_name);
        SaveAndLoadManager.LoadInventory(_name);
        SaveAndLoadManager.LoadPlayerXP(_name);
    }
    public void GetXP(int xp)
    {        
        Experience.AddTotalXp(xp);

        foreach (string itemClass in _classList)
        {
            Experience.GetXpForClass(itemClass, xp);
            
        }

        //foreach (string itemName in _itemList)
        //{
        //    Experience.GetXpForItem(itemName, xp);
        //}
    }

    public void GiveTestXP()
    {
        GetXP(2000);
    }
    public void GiveTestMoreXP()
    {
        GetXP(20000);
    }
    public void GiveTestLotXP()
    {
        GetXP(200000);
    }
    public void ExitCombat()
    {
        Characteristics.ExitCombat();
    }
}

[System.Serializable]
public struct CharacteristicsSaveData
{
    public CharacterCharacteristics Characteristics;

    public CharacteristicsSaveData(CharacterCharacteristics characteristics)
    {
        Characteristics = characteristics;
    }
}

[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem InvSys;

    public InventorySaveData(InventorySystem invSys)
    {
        InvSys = invSys;
    }
}

[System.Serializable]
public struct ExperienceSaveData
{
    public ExperienceSystem XPSys;

    public ExperienceSaveData(ExperienceSystem xpSys)
    {
        XPSys = xpSys;
    }
}
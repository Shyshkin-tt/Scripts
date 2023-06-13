using Gentleland.StemapunkUI.DemoAndExample;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    public Database database;
    private SkinnedMeshRenderer playerSkin;
    public GameObject _uiPlayer;
    InventoryHolder _holder;

    [SerializeField] private string _acc = "";
    [SerializeField] private string _name = "";
    [SerializeField] private string _location = "";
    private int _coins = 0;
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
    private int _hpRec = 5;
    private int _mpRec = 2;
    [SerializeField] private Vector3 _spawnCoords;
    [SerializeField] private Vector3 _curentCoordinats;

    public int _inventorySize;
    public int _equipSlots;
    public int _bagSlots;

    public GameObject[] _equipOnPlayer;
    public InventorySlot_UI[] _slotUI;

    [SerializeField] protected InventorySystem _inventory;
    public InventorySystem Inventory => _inventory;

    public static UnityAction OnInventorySlotChanged;

    protected virtual void Awake()
    {
        SaveLoadGameData.OnLoadData += LoadInventory;

        _holder = GetComponent<InventoryHolder>();
        playerSkin = GetComponentInChildren<SkinnedMeshRenderer>();

        _inventory = new InventorySystem(_inventorySize, _equipSlots, _bagSlots, _acc, _name, _location, _coins, _ip, _hp, _mp, _pd, _md, _as, _ms,
        _pdef, _mdef, _hpValue, _mpValue, _hpRec, _mpRec, _spawnCoords, _curentCoordinats, playerSkin, _holder);

        _inventory.SetValue();

        SetSlot();
        
    }

    public void SetNameAndLoc(string name, string loc)
    {
        _name = name;
        _location = loc;
    }

    private void Start()
    {
        SaveAndLoadManager._saveData.playerInventory = new InventorySaveData(_inventory);
        SaveAndLoadManager.LoadInventory();
    }

    private void Update()
    {
        _inventory.GetCurentCoords(_holder);
        _curentCoordinats = _inventory.CurrentCoordinats;
        //_location = SceneManager.GetActiveScene().name;
    }

    protected virtual void LoadInventory(SaveData data)
    {
        _inventory = data.playerInventory.InvSys;

        foreach (var slot in Inventory.EquipSlots)
        {
            if (slot.ItemData != null)
            {
                EquipOnPlayer(slot);
            }
        }
    }
    public void SetSlot()
    {
        _inventory.EquipSlots[0].SetNameSlotOnHolder("Neck", null, _slotUI[0]);
        _inventory.EquipSlots[1].SetNameSlotOnHolder("Head", _equipOnPlayer[0], _slotUI[1]);
        _inventory.EquipSlots[2].SetNameSlotOnHolder("Spine", null, _slotUI[2]);
        _inventory.EquipSlots[3].SetNameSlotOnHolder("RHand", _equipOnPlayer[3], _slotUI[3]);
        _inventory.EquipSlots[4].SetNameSlotOnHolder("Armor", _equipOnPlayer[1], _slotUI[4]);
        _inventory.EquipSlots[5].SetNameSlotOnHolder("LHand", _equipOnPlayer[4], _slotUI[5]);
        _inventory.EquipSlots[6].SetNameSlotOnHolder("Belt", null, _slotUI[6]);
        _inventory.EquipSlots[7].SetNameSlotOnHolder("Shoes", _equipOnPlayer[2], _slotUI[7]);
        _inventory.EquipSlots[8].SetNameSlotOnHolder("Bag", null, _slotUI[8]);

    }
    public void EquipOnPlayer(InventorySlot itemData)
    {        
        GameObject item = Instantiate(itemData.ItemData.ItemPrefab);
        item.transform.SetParent(itemData.OnEquip.transform, false);
        SkinnedMeshRenderer skinRender = item.GetComponentInChildren<SkinnedMeshRenderer>();        
        if (itemData.NameSlot != "RHand")
        {
            skinRender.bones = playerSkin.bones;
            skinRender.rootBone = playerSkin.rootBone;
        }        
    }

    public void RemoveFromPlayer(InventorySlot itemData)
    {      
        Destroy(itemData.OnEquip.transform.GetChild(0).gameObject);
    }

    public void SetCoordInHolder(Vector3 coords)
    {
        _spawnCoords = coords;
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : MonoBehaviour, IInteractable
{
    public int _slotsCount;

    [SerializeField] protected InventorySystem _inventory;
    public InventorySystem Inventory => _inventory;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public static UnityAction<InventorySystem, int> InventoryChest;

    protected virtual void Awake()
    {        
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void LoadInventory(SaveData data)
    {
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out ChestSaveData chestData))
        {
            this._inventory = chestData._invSystem;
            this.transform.position = chestData._position;
            this.transform.rotation = chestData._rotation;
        }
    }

    private void Start()
    {
        _inventory = new InventorySystem(_slotsCount);
        var chestSvaeData = new ChestSaveData(_inventory, transform.position, transform.rotation);

        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSvaeData);
    }   
    public void Interact(ActionController interactor, out bool interactSuccessful)
    {        
        InventoryChest?.Invoke(_inventory, 0);       
        interactSuccessful = true;
    }

    public void EndInteraction()
    {

    }
}

[System.Serializable]
public struct ChestSaveData
{
    public InventorySystem _invSystem;
    public Vector3 _position;
    public Quaternion _rotation;

    public ChestSaveData(InventorySystem invSys, Vector3 position, Quaternion rotation)
    {
        _invSystem = invSys;
        _position = position;
        _rotation = rotation;
    }
}
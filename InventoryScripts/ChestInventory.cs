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

    private void Awake()
    {
        SaveLoadGameData.OnLoadData += LoadChestLoot;
    }
    private void Start()
    {
        var chestSaveData = new ChestLootData(Inventory);
        SaveAndLoadManager._saveData.chestLootData.Add(GetComponent<UniqueID>().ID, chestSaveData);

        _inventory = new InventorySystem(_slotsCount);        
    }   
    private void LoadChestLoot(SaveData data)
    {
       if (data.chestLootData.TryGetValue(GetComponent<UniqueID>().ID, out ChestLootData chestData))
        {
           _inventory = chestData.chestLoot;
        }
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
public struct ChestLootData
{
    public InventorySystem chestLoot;

    public ChestLootData(InventorySystem loot)
    {
        chestLoot = loot;
    }
}
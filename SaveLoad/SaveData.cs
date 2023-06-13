using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public PlayerListData playerListData;
    public InventorySaveData playerInventory;
    public SerializableDictionary<string, ChestLootData> chestLootData;

    public SaveData()
    {
        playerListData = new PlayerListData();
        playerInventory = new InventorySaveData();
        chestLootData = new SerializableDictionary<string, ChestLootData>();
    }
}
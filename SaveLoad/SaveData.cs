using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{    
    public InventorySaveData playerInventory;
    public SerializableDictionary<string, ChestLootData> chestLootData;
    public SaveData()
    {        
        playerInventory = new InventorySaveData();
        chestLootData = new SerializableDictionary<string, ChestLootData>();
    }
}
public class ListSaveData
{
    public PlayerListData playerListData;

    public ListSaveData()
    {
        playerListData = new PlayerListData();
    }
}
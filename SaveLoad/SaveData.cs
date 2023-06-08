using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public InventorySaveData playerInventory;
    public CharacterSaveDataList playerCharList;
    public SerializableDictionary<string, ChestSaveData> chestDictionary;    
    public SaveData()
    {   
        playerInventory = new InventorySaveData();
        playerCharList = new CharacterSaveDataList();
        chestDictionary = new SerializableDictionary<string, ChestSaveData>();        
    }
}

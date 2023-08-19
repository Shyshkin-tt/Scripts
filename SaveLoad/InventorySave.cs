using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacteristicsSave
{
    public CharacteristicsSaveData playerCharacteristics;
    public CharacteristicsSave()
    {
        playerCharacteristics = new CharacteristicsSaveData();
    }
}
public class InventorySave
{    
    public InventorySaveData playerInventory;    
    public SerializableDictionary<string, ChestLootData> chestLootData;

    public InventorySave()
    {        
        playerInventory = new InventorySaveData();        
        chestLootData = new SerializableDictionary<string, ChestLootData>();
    }
}
public class ListSave
{
    public PlayerListData playerListData;

    public ListSave()
    {
        playerListData = new PlayerListData();
    }
}

public class PlayerExperienceSave
{
    public ExperienceSaveData playerExperience;

    public PlayerExperienceSave()
    {
        playerExperience = new ExperienceSaveData();
    }
}
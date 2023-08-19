using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveAndLoadManager : MonoBehaviour
{
    public static CharacteristicsSave _characteristicsData;
    public static InventorySave _inventoryData;
    public static ListSave _listSaveData;
    public static PlayerExperienceSave _playerXPData;

    private void Awake()
    {
        _characteristicsData = new CharacteristicsSave();
        _inventoryData = new InventorySave();
        _playerXPData = new PlayerExperienceSave();
        _listSaveData= new ListSave();

        SaveLoadGameData.OnLoadCharacteristics += LoadCharacteristics;
        SaveLoadGameData.OnLoadInventory += LoadInventory;
        SaveLoadGameData.OnLoadPayerXP += LoadPlayerXpData;
        SaveLoadGameData.OnLoadCharListData += LoadListData;
    }

    private void LoadCharacteristics(CharacteristicsSave saveCharacteristics)
    {
        _characteristicsData = saveCharacteristics;
    }
    public static void LoadInventory(InventorySave saveInventory)
    {        
        _inventoryData = saveInventory;
    }
    private void LoadPlayerXpData(PlayerExperienceSave playerXP)
    {
        _playerXPData = playerXP;        
    }
    private void LoadListData(ListSave listData)
    {
        _listSaveData = listData;
    }
    public static void SaveInventory(string name)
    {
        var saveInventory = _inventoryData;
       
        SaveLoadGameData.SaveCharacterInventory(saveInventory, name);
    }
    public static void SaveCharacteristics(string name)
    {
        var saveCharacteristics = _characteristicsData;

        SaveLoadGameData.SaveCharacterCharacteristics(saveCharacteristics, name);
    }
    public static void LoadInventory(string name)
    {        
        SaveLoadGameData.LoadCharacterInventory(name);
    }
    public static void LoadCharacteristics(string name)
    {
        SaveLoadGameData.LoadCharacterCharacteristics(name);
    }
    public static void SavePlayerXP(string name)
    {
        var saveData = _playerXPData;        

        SaveLoadGameData.SaveCharacterXP(saveData, name);
    }
    public static void LoadPlayerXP(string name)
    {
        SaveLoadGameData.LoadCharacterXP(name);        
    }

   
    public static void SaveCharList()
    {
        var saveData = _listSaveData;
        
        SaveLoadGameData.SaveCharacterList(saveData);
    }
    public static void LoadCharList()
    {        
        SaveLoadGameData.LoadCharacterList();
    }

    public static void DeleteChar(string name)
    {
        SaveLoadGameData.DeleteCharacter(name);
    }
}

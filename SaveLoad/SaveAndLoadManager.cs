using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveData _saveData;
    public static ListSaveData _listSaveData;
    public static PlayerExperienceSave _playerXPData;

    private void Awake()
    {
        _saveData = new SaveData();
        _playerXPData = new PlayerExperienceSave();
        _listSaveData= new ListSaveData();

        SaveLoadGameData.OnLoadData += LoadData;
        SaveLoadGameData.OnLoadPayerXP += LoadPlayerXpData;
        SaveLoadGameData.OnLoadCharListData += LoadListData;
    }
    public static void LoadData(SaveData saveData)
    {        
        _saveData = saveData;
    }
    private void LoadPlayerXpData(PlayerExperienceSave playerXP)
    {
        _playerXPData = playerXP;        
    }
    public static void SaveInventory()
    {
        var saveData = _saveData;
       
        SaveLoadGameData.SaveCharacterInventory(saveData);
    }
    public static void LoadInventory(string name)
    {        
        SaveLoadGameData.LoadCharacterInventory(name);
    }
    public static void SavePlayerXP()
    {
        var saveData = _playerXPData;        

        SaveLoadGameData.SaveCharacterXP(saveData);

        
    }
    public static void LoadPlayerXP()
    {
        SaveLoadGameData.LoadCharacterXP();        
    }

    private void LoadListData(ListSaveData listData)
    {
        _listSaveData = listData;
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
}

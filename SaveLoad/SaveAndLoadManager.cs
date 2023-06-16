using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveData _saveData;
    public static ListSaveData _listSaveData;

    private void Awake()
    {
        _saveData = new SaveData();
        _listSaveData= new ListSaveData();

        SaveLoadGameData.OnLoadData += LoadData;
        SaveLoadGameData.OnLoadCharListData += LoadListData;
    }

    private void LoadListData(ListSaveData listData)
    {
        _listSaveData = listData;
    }

    public static void LoadData(SaveData saveData)
    {        
        _saveData = saveData;
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

    public static void SaveInventory()
    {
        var saveData = _saveData;
       
        SaveLoadGameData.SaveCharacterInventory(saveData);
    }

    public static void LoadInventory(string name)
    {
        
        SaveLoadGameData.LoadCharacterInventory(name);
    }
}

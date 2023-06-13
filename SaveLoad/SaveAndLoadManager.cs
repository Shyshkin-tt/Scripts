using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveData _saveData;

    private void Awake()
    {
        _saveData = new SaveData();
        SaveLoadGameData.OnLoadData += LoadData;
        SaveLoadGameData.OnLoadCharListData += LoadData;
    }    
    public static void LoadData(SaveData saveData)
    {        
        _saveData = saveData;
    }
    public static void SaveCharList()
    {
        var saveData = _saveData;
        
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

    public static void LoadInventory()
    {
        SaveLoadGameData.LoadCharacterInventory();
    }
}

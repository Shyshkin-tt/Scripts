using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveGameManager : MonoBehaviour
{
    public static SaveData data;

    private void Awake()
    {
        data = new SaveData();
        SaveLoad.OnLoadGame += LoadData;
    }

    public static void LoadData(SaveData _data)
    {
        data = _data;
    }

    public static void SaveInventoryData()
    {
        var saveData = data;

        SaveLoad.SaveInventory(saveData);
    }

    public static void LoadInventoryData()
    {
        SaveLoad.LoadInventory();
    }

    public static void SavePlayerListData()
    {
        var saveData = data;
        SaveLoad.SavePlayerCharacterList(saveData);
    }
    public static void LoadPlayerListData()
    {
        //SaveLoad.LoadPlayerCharList();
    }
}

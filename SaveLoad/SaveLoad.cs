using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using UnityEngine.TextCore.Text;

public static class SaveLoad
{
    public static UnityAction OnSaveGame;
    public static UnityAction OnSaveCharlist;
    public static UnityAction<SaveData> OnLoadGame;
    public static UnityAction<SaveData> OnLoadCharList;   

    public static bool SaveInventory(SaveData data)
    {
        OnSaveGame?.Invoke();       

        string json = JsonUtility.ToJson(data, true);
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Inventory", json}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, null, null);

        Debug.Log("Saving game");

        return true;
    }

    public static SaveData LoadInventory()
    {
        SaveData data = new SaveData();
        return data;
    }

    public static void DeleteSaveData()
    {
               
    }
    public static bool SavePlayerCharacterList(SaveData data)
    {
        OnSaveCharlist?.Invoke();

        string json = JsonConvert.SerializeObject(data.playerCharList);

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Characters", json}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, null, null);
        return true;
    }
   
    public static SaveData LoadPlayerCharList(GetUserDataResult result)
    {
        SaveData data = new SaveData();

        if (result.Data != null && result.Data.ContainsKey("Characters"))
        {
            UserDataRecord userData = result.Data["Characters"];
            string json = userData.Value;

            List<Character> chars = JsonConvert.DeserializeObject<List<Character>>(json);


            //data.playerCharList = chars;
        }       

        return data;
    }
}

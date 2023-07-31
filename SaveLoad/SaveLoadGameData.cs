using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using UnityEngine.Events;
using System.IO;
using System;

public static class SaveLoadGameData
{   

    public static UnityAction OnSaveCharListData;
    public static UnityAction<ListSaveData> OnLoadCharListData;

    public static UnityAction OnSaveData;
    public static UnityAction<SaveData> OnLoadData;

    public static UnityAction OnSavePayerXP;
    public static UnityAction<PlayerExperienceSave> OnLoadPayerXP;


    public static void SaveCharacterList(ListSaveData data)
    {
        OnSaveCharListData?.Invoke();       

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Characters", JsonUtility.ToJson(data.playerListData)}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, null, OnSaveCharacterListFailure);        
    }   
    private static void OnSaveCharacterListFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }
    public static void LoadCharacterList()
    {        
        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, OnLoadCharacterListSuccess, OnLoadCharacterListFailure);
    }
    private static void OnLoadCharacterListSuccess(GetUserDataResult result)
    {
        ListSaveData data = new ListSaveData();

        if (result.Data != null && result.Data.ContainsKey("Characters"))
        {
            string json = result.Data["Characters"].Value;
            data.playerListData = JsonUtility.FromJson<PlayerListData>(json);
            OnLoadCharListData?.Invoke(data);
            
        }
    }
    private static void OnLoadCharacterListFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }
    public static void SaveCharacterInventory(SaveData data)
    {
        OnSaveData?.Invoke();       

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {data.playerInventory.InvSys.Name, JsonUtility.ToJson(data.playerInventory) }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, null, OnSaveCharacterInventoryFailure);
    }
    private static void OnSaveCharacterInventoryFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }
    public static void LoadCharacterInventory(string name)
    {
        
        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, result => OnLoadCharacterInventorySuccess(result, name), OnLoadCharacterInventoryFailure);
    }
    private static void OnLoadCharacterInventorySuccess(GetUserDataResult result, string name)
    {
        SaveData data = new SaveData();        

        if (result.Data != null && result.Data.ContainsKey(name))
        {
            string json = result.Data[name].Value;
            data.playerInventory = JsonUtility.FromJson<InventorySaveData>(json);
            OnLoadData?.Invoke(data);
        }
        else
        {
            Debug.Log("Key not found: " + name);
        }
    }
    private static void OnLoadCharacterInventoryFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }
    public  static void DeleteCharacter()
    {
        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, DeleteSuccess, null);
    }
    private static void DeleteSuccess(GetUserDataResult result)
    {
        
    }
    public static void SaveCharacterXP(PlayerExperienceSave playerXP)
    {
        OnSavePayerXP?.Invoke();

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Character experience", JsonUtility.ToJson(playerXP.playerExperience) }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, null, OnSaveCharacterXPFailure);
    }
    private static void OnSaveCharacterXPFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }

    public static void LoadCharacterXP()
    {

        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, result => OnLoadCharacterXPSuccess(result), OnLoadCharacterXPFailure);
    }

    private static void OnLoadCharacterXPFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }

    private static void OnLoadCharacterXPSuccess(GetUserDataResult result)
    {
        PlayerExperienceSave data = new PlayerExperienceSave();

        if (result.Data != null && result.Data.ContainsKey("Character experience"))
        {
            string json = result.Data["Character experience"].Value;
            data.playerExperience = JsonUtility.FromJson<ExperienceSaveData>(json);
            OnLoadPayerXP?.Invoke(data);
            
        }
    }
}


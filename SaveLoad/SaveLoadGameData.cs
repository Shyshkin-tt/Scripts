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
    public static UnityAction<SaveData> OnLoadCharListData;
    public static UnityAction OnSaveData;
    public static UnityAction<SaveData> OnLoadData;
    
    public static void SaveCharacterList(SaveData data)
    {
        OnSaveCharListData?.Invoke();       

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Characters", JsonUtility.ToJson(data.playerListData)}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnSaveCharacterListSuccess, OnSaveCharacterListFailure);
    }
    private static void OnSaveCharacterListSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Character list saved");
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
        SaveData data = new SaveData();

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

        string json = JsonUtility.ToJson(data.playerInventory, true);

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { data.playerInventory.InvSys.Name, json }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnSaveCharacterInventorySuccess, OnSaveCharacterInventoryFailure);
    }
    public static void LoadCharacterInventory()
    {
        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, OnLoadCharacterInventorySuccess, OnLoadCharacterInventoryFailure);
    }

    private static void OnLoadCharacterInventoryFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }

    private static void OnLoadCharacterInventorySuccess(GetUserDataResult result)
    {
        SaveData data = new SaveData();

        if (result.Data != null && result.Data.ContainsKey("Player Characters"))
        {
            string json = result.Data["Player Characters"].Value;
            data.playerListData = JsonUtility.FromJson<PlayerListData>(json);
            OnLoadData?.Invoke(data);
        }
    }

    
    private static void OnSaveCharacterInventorySuccess(UpdateUserDataResult result)
    {
        //Debug.Log("Character inventory saved");
    }
    private static void OnSaveCharacterInventoryFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }
}


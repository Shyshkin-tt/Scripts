using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
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
        string charListJson = JsonUtility.ToJson(data.playerListData);
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Characters", charListJson}
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
        if (result.Data != null && result.Data.ContainsKey("Characters"))
        {
            string charListJson = result.Data["Characters"].Value;
            PlayerListData listData = JsonUtility.FromJson<PlayerListData>(charListJson);

            // «агрузка и десериализаци€ списка персонажей
            ListSaveData data = new ListSaveData();
            data.playerListData = listData;
            OnLoadCharListData?.Invoke(data);
        }
    }
    private static void OnLoadCharacterListFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }
    public static void SaveCharacterInventory(SaveData data, string name)
    {
        OnSaveData?.Invoke();       

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {name, JsonUtility.ToJson(data.playerInventory) }
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
    public static void SaveCharacterXP(PlayerExperienceSave playerXP, string name)
    {
        OnSavePayerXP?.Invoke();

        string charXP = name + " XP";

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {charXP, JsonUtility.ToJson(playerXP.playerExperience) }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, null, OnSaveCharacterXPFailure);
    }
    private static void OnSaveCharacterXPFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }
    public static void LoadCharacterXP(string name)
    {

        var request = new GetUserDataRequest();
        PlayFabClientAPI.GetUserData(request, result => OnLoadCharacterXPSuccess(result, name), OnLoadCharacterXPFailure);
    }
    private static void OnLoadCharacterXPFailure(PlayFabError error)
    {
        Debug.Log(error.ErrorDetails);
    }
    private static void OnLoadCharacterXPSuccess(GetUserDataResult result, string name)
    {
        PlayerExperienceSave data = new PlayerExperienceSave();

        string charXP = name + " XP";

        if (result.Data != null && result.Data.ContainsKey(charXP))
        {
            string json = result.Data[charXP].Value;
            data.playerExperience = JsonUtility.FromJson<ExperienceSaveData>(json);
            OnLoadPayerXP?.Invoke(data);
            
        }
    }
    public static void DeleteCharacter(string name)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { name, null }
        }
        };
        PlayFabClientAPI.UpdateUserData(request, null, null);

        string charXP = name + " XP";

        var requestXP = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { charXP, null }
        }
        };
        PlayFabClientAPI.UpdateUserData(requestXP, null, null);
    }
}


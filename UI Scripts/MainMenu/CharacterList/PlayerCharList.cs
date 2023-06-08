using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

[System.Serializable]
public class PlayerCharList : MonoBehaviour
{  
    [SerializeField] private int _countCharacters;

    [SerializeField] protected CharCreator _charCreator;
    public CharCreator CharCreator => _charCreator;

    private void Awake()
    {
        // загрузка данных списка персонажей
        SaveLoad.OnLoadCharList += LoadCharList;

        // создание списка персонажей
        _charCreator = new CharCreator(_countCharacters);
    }

    private void LoadCharList(SaveData savedata)
    {
        
    }

    private void Start()
    {
        SaveGameManager.data.playerCharList = new CharacterSaveDataList(_charCreator);

        
    }
}


[System.Serializable]
public struct CharacterSaveDataList
{
    public CharCreator CharListSave;

    public string NamePlayer;
    public string GenderPlayer;
    public string LocationPlayer;

    public CharacterSaveDataList(CharCreator charList)
    {
        CharListSave = charList;

        NamePlayer = charList.PlayerName;
        GenderPlayer = charList.PlayerGender;
        LocationPlayer = charList.PlayerLocation;
    }
}
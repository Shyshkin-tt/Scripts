using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerCharList : MonoBehaviour
{
    public PlayerCharListScene _charListScene;
    private List<GameObject> _charListObjects = new List<GameObject>();

    private int _countCharacters;

    [SerializeField] protected CharCreator _charCreator;
    public CharCreator CharCreator => _charCreator;

    public static UnityAction ListChange;
    protected virtual void Awake()
    {   
        //SaveLoadGameData.OnLoadCharListData += LoadCharList;
        // создание списка персонажей
        _charCreator = new CharCreator(_countCharacters);       
    }
    private void OnEnable()
    {
        SaveLoadGameData.OnLoadCharListData += LoadCharList;
    }
    private void OnDisable()
    {
        SaveLoadGameData.OnLoadCharListData -= LoadCharList;
    }
    private void Start()
    {
        SaveAndLoadManager._saveData.playerListData = new PlayerListData(_charCreator);

        SaveAndLoadManager.LoadCharList();
    }   

    private void LoadCharList(SaveData data)
    {
        _charCreator = data.playerListData.ChararacterCreator;
        Debug.Log("LoadCharList");
        foreach (var chars in CharCreator.CharLists)
        {
            if(chars.Name != null)
            {
                AddCharList(chars);
            }
        }
    }
    public void AddCharList(Character data)
    {
        
        GameObject charList = Instantiate(_charListScene._charListPrefab, _charListScene._previewPanel.transform);

        var list = charList.GetComponent<Char_UI>();
        list.Sets(data.Name, data.Gender, data.Location);

        if (_charListScene._previewCharacter.transform.childCount == 0)
            _charListScene.CreateChars(data.Name, data.Gender, data.Location);
    }
}

[System.Serializable]
public struct PlayerListData
{
    public CharCreator ChararacterCreator;

    public PlayerListData(CharCreator chararacterCreator)
    {
        ChararacterCreator = chararacterCreator;
    }
}
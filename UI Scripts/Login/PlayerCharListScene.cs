using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using PlayFab.Internal;
using UnityEngine.Analytics;
using UnityEngine.WSA;
using System;

[System.Serializable]
public class PlayerCharListScene : MonoBehaviour
{
    EventSystem _system;

    public GameObject _sceneLoaderPrefab;
    public SceneLoader _sceneLoader;

    public Locations _loc;

    public  string _activCharacterPreview;
    public TextMeshProUGUI _activCharNick;     

    [Header("Character UI list")]
    public GameObject _charactersPanel;    
    public GameObject _previewPanel;    
    public GameObject _previewCharacter;
    
    [Header("Character create")]
    public GameObject _charCreate;
    public GameObject _charListPrefab;
    public TMP_InputField _inputNameCharacter;
    public TextMeshProUGUI _gender;
    public GameObject _malePrefab;
    public GameObject _femalePrefab;

    [Header("Warning panel")]
    public GameObject _warning;
    public TextMeshProUGUI _warningText;

    [Header("Player character list")]
    int _countChars;
    private List<Character> _characterPreviewList;

    [SerializeField] protected PlayerCharListCreator _characterList;
    public PlayerCharListCreator PlayerChar => _characterList;

    private void Awake()
    {        
        _system = EventSystem.current;
        CreateSceneLoader();
        
        SaveLoadGameData.OnLoadCharListData += LoadCharList;
        SaveLoadGameData.OnLoadData += SetSceneLoaderData;

        _characterPreviewList = new List<Character>();
        _characterList = new PlayerCharListCreator(_characterPreviewList, _countChars);
    }

    private void SetSceneLoaderData(SaveData data)
    {
        _sceneLoader.SetStats(data.playerInventory.InvSys.Location, data.playerInventory.InvSys.SpawnCoord, data.playerInventory.InvSys.Name);
    }

    private void Start()
    {
        _activCharNick.text = "";

        SaveAndLoadManager._listSaveData.playerListData = new PlayerListData(_characterList);
        SaveAndLoadManager.LoadCharList();

        SetInSceneLoaderCurrentData();
    }
    public void SetInSceneLoaderCurrentData()
    {
        foreach (var chars in _characterList.CharacterPreviewList)
        {
            if (_activCharacterPreview == chars.Name)
            {
                _sceneLoader.SetStats(chars.Location, chars.SpawnCoords, chars.Name);
            }
        }
    }

    //======= ON START =======\\
    private void CreateSceneLoader()
    {
        _sceneLoader = FindObjectOfType<SceneLoader>();

        if (_sceneLoader == null)
        {
            GameObject loader = Instantiate(_sceneLoaderPrefab);
            _sceneLoader = loader.GetComponent<SceneLoader>();
        }
        else
        {

        }
    }
    private void LoadCharList(ListSaveData data)
    {
        _characterList = data.playerListData.CharacterList;

        foreach (var character in _characterList.CharacterPreviewList)
        {
            if (character.Name != null)
            {
                LoadCharacterList(character.Name, character.Gender, character.Location);
            }
        }
    }
    public void LoadCharacterList(string name, string gender, string loc)
    {
        CreateUIList(name, gender, loc);

        if (_previewCharacter != null)
        {
            var charPrefab = SelectGender(gender);
            _sceneLoader.Char = charPrefab;
            GameObject charPreview = Instantiate(charPrefab, _previewCharacter.transform);
            
            GoToLoaderAndOffFadeAndActionScripts(charPreview);

            charPreview.name = name;

            SaveAndLoadManager.LoadInventory(name);
            SaveAndLoadManager.LoadPlayerXP(name);
        }           
    }  

    //======= ON CREATE =======\\
    public void CheckCharacterCount()
    {
        if (PlayerChar.CountChars < 1)
            _charCreate.SetActive(true);
        else if (PlayerChar.CountChars == 1)
        {
            _warningText.text = "Now you can create only one character";
            _warning.SetActive(true);
        }
    }
    public void CreateNewListInMenu()
    {
        string start = "Start location";
        var spawn = FindLocationCoordinates(start);

        if (_gender.text == "Female")
        {
            _warningText.text = "No female characters now";
            _warning.SetActive(true);
            ClearAfterCreate();
        }
        else
        {
            _characterList.AddCharToList(_inputNameCharacter.text, _gender.text, start, spawn);
            CreateUIList(_inputNameCharacter.text, _gender.text, start);
            CreateCharsGender(_inputNameCharacter.text, _gender.text, start);


            ClearAfterCreate();
        }
    }
    public void CreateUIList(string name, string gender, string loc)
    {
        if (_previewPanel != null)
        {
            GameObject charList = Instantiate(_charListPrefab, _previewPanel.transform);
            var list = charList.GetComponent<Char_UI>();
            list.Sets(name, gender, loc);
            charList.name = name;
        }
              
    }
    public void CreateCharsGender(string name, string gender, string loc)
    {
        ActivCharScreenSet(name);

        var charPrefab = SelectGender(gender);

        GameObject charPreview = Instantiate(charPrefab, _previewCharacter.transform);
        charPreview.name = name;

        var holder = charPreview.GetComponent<InventoryHolder>();

        GoToLoaderAndOffFadeAndActionScripts(charPreview);       
        SetNewChar(holder, name, loc);
        _sceneLoader.GetChar(charPrefab);

        UpdateCharList();

        //SaveChar(holder);
    }
    private void SetNewChar(InventoryHolder holder, string name, string loc)
    {
        var spawn = FindLocationCoordinates(loc);
        holder.Inventory.OnCreate(name, loc, spawn);

        _sceneLoader.SetStats(loc, spawn, name);
    }
    private Vector3 FindLocationCoordinates(string loc)
    {
        foreach (var location in _loc.locations)
        {
            if (location.locationName == loc)
            {
                return location.spawnPoints[0].spawnPoint.transform.position;
            }
        }
        return Vector3.zero;
    }
    public void ActivCharScreenSet(string name)
    {
        _activCharacterPreview = name;
        _activCharNick.text = name;
    }
    //========================\\

    //======= HELPFULL METHODS =======\\
    private GameObject SelectGender(string gender)
    {
        GameObject genderPrefab = null;

        if (gender == "Male")
        {
            genderPrefab = _malePrefab;
        }
        else if (gender == "Female")
        {
            genderPrefab = _femalePrefab;
        }
        return genderPrefab;
    }
    private void GoToLoaderAndOffFadeAndActionScripts(GameObject character)
    {
        character.GetComponent<FadeToMe>().enabled = false;
        character.GetComponent<ActionController>().enabled = false;
        var holder = character.GetComponent<InventoryHolder>();

        holder._uiPlayer.SetActive(false);

        
    }
    public void SetMale()
    {
        _gender.text = "Male";
    }
    public void SetFemale()
    {
        _gender.text = "Female";
    }
    private void ClearAfterCreate()
    {
        _inputNameCharacter.text = "Enter name";
        _gender.text = "Choose gender";
    }
    public void CleanWarningMessage()
    {
        _warningText.text = "";
    }  
    public void DeleteChar(int index, string name)
    {
        Transform child = _previewCharacter.transform.GetChild(0);
        DestroyImmediate(child.gameObject);
        PlayerChar.RemoveCharFromList(index);       

        SaveAndLoadManager.DeleteChar(name);
        UpdateCharList();
    }
    private void UpdateCharList()
    {
        SaveAndLoadManager._listSaveData.playerListData = new PlayerListData(_characterList);
        SaveAndLoadManager.SaveCharList();
    }
    public void SaveChar()
    {
        var holder = _previewCharacter.GetComponentInChildren<InventoryHolder>();
        holder.SaveFromHolder();
    }
    //========================\\

    //======= BUTTONS =======\\
    public void Loggout()
    {
        var loader = FindObjectOfType<SceneLoader>();
        Destroy(loader.transform.gameObject);

        SceneManager.LoadScene("Login");        
    }
    public void EntryInGame()
    {
        var scene = FindObjectOfType<SceneLoader>().Location;       

        SceneManager.LoadScene(scene);
    }
    //========================\\
}

[System.Serializable]
public struct PlayerListData
{
    public PlayerCharListCreator CharacterList;

    public PlayerListData(PlayerCharListCreator playerList )
    {
        CharacterList = playerList;
    }
}

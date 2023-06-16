using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.WSA;

[System.Serializable]
public class PlayerCharListScene : MonoBehaviour
{
    EventSystem _system;
    
    public GameObject _sceneLoader;
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
    [SerializeField] int _countChars;
    public List<Character> _characterPreviewList;

    private void Awake()
    {        
        _system = EventSystem.current;
        
        SaveLoadGameData.OnLoadCharListData += LoadCharList;

        _characterPreviewList = new List<Character>();

    }
    private void Start()
    {
        _activCharNick.text = "";

        SaveAndLoadManager._listSaveData.playerListData = new PlayerListData(_characterPreviewList);
        SaveAndLoadManager.LoadCharList();
        if (SaveAndLoadManager._listSaveData.playerListData.CharacterList == null) Debug.Log("no data in start");
        StartCoroutine(ListCheck());
    }
    private void Update()
    {
             
    }
    public void SetMale()
    {
        _gender.text = "Male";
    }
    public void SetFemale()
    {
        _gender.text = "Female";
    }
    public void CheckCharacterCount()
    {
        if (_countChars < 1)
            _charCreate.SetActive(true);
        else if (_countChars == 1)
        {
            _warningText.text = "Now you can create only one character";
            _warning.SetActive(true);
        }
    }
    public void ActivCharScreenSet(string name)
    {
        _activCharacterPreview = name;
        _activCharNick.text = name;
    }
    public void CreateNewListInMenu()
    {
        if (_gender.text == "Female")
        {
            _warningText.text = "No female characters now";
            _warning.SetActive(true);
            ClearAfterCreate();
        }
        else
        {
            _characterPreviewList.Add(new Character(_inputNameCharacter.text, _gender.text, "Start location"));
            CreateUIList(_inputNameCharacter.text, _gender.text, "Start location");
            CreateCharsGender(_inputNameCharacter.text, _gender.text, "Start location");
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
    private void ClearAfterCreate()
    {
        _inputNameCharacter.text = "Enter name";
        _gender.text = "Choose gender";
    }
    public void CreateCharsGender(string name, string gender, string loc)
    {
        ActivCharScreenSet(name);        

        if (gender == "Male")
        {
            GameObject charPreview = Instantiate(_malePrefab, _previewCharacter.transform);
            charPreview.name = name;
            var holder = _previewCharacter.GetComponentInChildren<InventoryHolder>();
            SetNewChar(name, loc);
            GoToLoaderAndOffFadeAndActionScripts(holder, _malePrefab);
           
        }
        else if(gender == "Female")
        {
            GameObject charPreview = Instantiate(_femalePrefab, _previewCharacter.transform);
            charPreview.name = name;
            var holder = _previewCharacter.GetComponentInChildren<InventoryHolder>();
            SetNewChar(name, loc);
            GoToLoaderAndOffFadeAndActionScripts(holder, _femalePrefab);
        }
        
        SaveAndLoadManager.SaveCharList();
        
    }
    private void LoadCharList(ListSaveData data)
    {
        _characterPreviewList = data.playerListData.CharacterList;

        foreach (var character in _characterPreviewList)
        {
            if(character.Name != null)
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
            if (gender == "Male")
            {
                GameObject charPreview = Instantiate(_malePrefab, _previewCharacter.transform);
                SaveAndLoadManager.LoadInventory(name);
                charPreview.name = name;
                var holder = _previewCharacter.GetComponentInChildren<InventoryHolder>();

                GoToLoaderAndOffFadeAndActionScripts(holder, _malePrefab);
            }
            else if (gender == "Female")
            {
                GameObject charPreview = Instantiate(_femalePrefab, _previewCharacter.transform);
                SaveAndLoadManager.LoadInventory(name);
                charPreview.name = name;
                var holder = _previewCharacter.GetComponentInChildren<InventoryHolder>();

                GoToLoaderAndOffFadeAndActionScripts(holder, _femalePrefab);
            }
        }       
    }

    private void SetNewChar(string name, string loc)
    {
        var holder = _previewCharacter.GetComponentInChildren<InventoryHolder>();

        var spawn = FindLocationCoordinates(loc);
        
        holder.Inventory.OnCreate(name, loc, spawn);

        StartCoroutine(SaveInventoryAfterCreate());
    }

    private void GoToLoaderAndOffFadeAndActionScripts(InventoryHolder holder, GameObject charPrefab)
    {
        _countChars += 1;

        var fade = holder.GetComponent<FadeToMe>();
        fade.enabled = false;
        var action = holder.GetComponent<ActionController>();
        action.enabled = false;

        StartCoroutine(AfterLoad(holder, charPrefab));
    }

    private IEnumerator AfterLoad(InventoryHolder holder, GameObject charPrefab)
    {
        var loaderInScene = FindObjectOfType<SceneLoader>();

        if (loaderInScene == null)
        {
            GameObject loader = Instantiate(_sceneLoader);
            var charPrefabInLoader = loader.GetComponent<SceneLoader>();

            charPrefabInLoader.Char = charPrefab;

            yield return new WaitForSeconds(0.5f);

            charPrefabInLoader.SetStats(holder.Inventory.Location, holder.Inventory.SpawnCoord, holder.Inventory.Name);
            holder.SetNameAndLoc(holder.Inventory.Name, holder.Inventory.Location, holder.Inventory.SpawnCoord);           
        }
        
        yield break;
    }

    private Vector3 FindLocationCoordinates(string loc)
    {
        foreach (var location in _loc.locations)
        {
            if (location.locationName == loc)
            {                
                return location.spawnPoints[0].spawnPoint; // ������������, ��� ��������� ������� ����� ������ ���� ����� ������
            }
        }
        return Vector3.zero;
    }

    public void DeleteChar(int index)
    {
        Transform child = _previewCharacter.transform.GetChild(0);
        DestroyImmediate(child.gameObject);
        _characterPreviewList.RemoveAt(index);
        _countChars -= 1;
        Debug.Log("FindLocationCoordinates");
        SaveAndLoadManager.SaveCharList();
    }
    public void CleanWarningMessage()
    {
        _warningText.text = "";
    }

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

    private IEnumerator SaveInventoryAfterCreate()
    {
        yield return new WaitForSeconds(.5f);
        SaveAndLoadManager.SaveInventory();
        yield break;
    }  
    private IEnumerator ListCheck()
    {
        yield return new WaitForSeconds(.5f);

        var loader = FindObjectOfType<SceneLoader>();

        foreach (var chars in _characterPreviewList)
        {
            if (chars.Name == loader.CharName)
            {
                if (chars.Location != loader.Location)
                {
                    chars.UpdateCharLoc(loader.Location);
                    var list = _previewPanel.gameObject.transform.Find(chars.Name);
                    var change = list.GetComponent<Char_UI>();
                    change._location.text = loader.Location;
                    SaveAndLoadManager.SaveCharList();
                }
            }
        }
        
        yield break;
    }
}

[System.Serializable]
public struct PlayerListData
{
    public List<Character> CharacterList;

    public PlayerListData(List<Character> playerList )
    {
        CharacterList = playerList;
    }
}

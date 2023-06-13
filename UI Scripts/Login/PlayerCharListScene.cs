using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CharacterPreviewList : MonoBehaviour
{
    public string Name;
    public string Gender;
    public string Location;
}
public class PlayerCharListScene : MonoBehaviour
{
    EventSystem _system;
    PlayerController _input;

    public string _activCharacterPreview;
    public TextMeshProUGUI _activCharNick;

    [Header("Top buttons")]
    public Button _exit;
    public Button _settings;
    public GameObject _logout;   

    [Header("Character player list")]
    public GameObject _charactersPanel;
    public PlayerCharList _playerCharList;
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
    private void Awake()
    {
        _input = new PlayerController();
        _system = EventSystem.current;
        _playerCharList = GetComponent<PlayerCharList>();
    }
    private void Start()
    {
        _activCharNick.text = "";
    }
    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
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
        if (_playerCharList.CharCreator.CountCharacters < 3)
            _charCreate.SetActive(true);
        else if (_playerCharList.CharCreator.CountCharacters == 3)
        {
            _warningText.text = "Now you can create only 3 character";
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
        _playerCharList.CharCreator.CreateNewCharList(_inputNameCharacter.text, _gender.text, "Start location");
        GameObject charList = Instantiate(_charListPrefab, _previewPanel.transform);
        var list = charList.GetComponent<Char_UI>();
        list.Sets(_inputNameCharacter.text, _gender.text, "Start location");
        if (_previewCharacter.transform.childCount > 0)
        {
            DeleteChar();
            CreateChars(_activCharacterPreview, _gender.text, "Start location");
        }
        else CreateChars(_activCharacterPreview, _gender.text, "Start location");

        ClearAfterCreate();
    }
    
    private void ClearAfterCreate()
    {
        _inputNameCharacter.text = "Enter name";
        _gender.text = "Choose gender";
    }
    public void CreateChars(string name, string gender, string loc)
    {
        if (gender == "Male")
        {
            
            GameObject charPreview = Instantiate(_malePrefab, _previewCharacter.transform);
            CharSets(charPreview, name, loc);
        }
        else if(gender == "Female")
        {
            // Нужен женский префаб
        }
    }

    private void CharSets(GameObject charPreview, string name, string loc)
    {
        var charSets = charPreview.GetComponent<InventoryHolder>();
        charSets.Inventory.SetName(name);
        charSets.Inventory.SetLocation(loc);
        charSets.SetNameAndLoc(name, loc);
        charSets._uiPlayer.SetActive(false);
        var fade = charPreview.GetComponent<FadeToMe>();
        fade.enabled = false;
        SaveAndLoadManager.SaveCharList();
    }
    public void DeleteChar()
    {
        Transform child = _previewCharacter.transform.GetChild(0);
        DestroyImmediate(child.gameObject);
        
    }
    public void CleanWarningMessage()
    {
        _warningText.text = "";
    }

    public void Loggout()
    {
        
        SceneManager.LoadScene("Login");
        
    }
}

   

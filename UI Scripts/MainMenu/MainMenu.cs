using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using static UnityEditor.PlayerSettings;
using static UnityEditor.ShaderData;

[System.Serializable]
public class CharacterPreviewList : MonoBehaviour
{
    public string Name;
    public string Gender;
    public string Location;
}
public class MainMenu : MonoBehaviour
{
    EventSystem _system;
    PlayerController _input;
    public Selectable _firstInput;

    [Header("Entry panel")]
    public Button _exit;
    public Button _settings;
    public GameObject _logout;

    [Header("Entry panel")]
    public GameObject _entryPanel;
    public TMP_InputField _inputEmail;
    public TMP_InputField _inputPass;
    public Button _entryButton;
    public Button _registryButton;
    public Button _ressetPassButton;

    [Header("Register panel")]
    public GameObject _registeryPanel;
    public TMP_InputField _inputRegisterEmail;
    public TMP_InputField _inputRegisterPass;
    public Button _registerConfirmButton;

    [Header("Recovery panel")]
    public GameObject _recoveryPanel;
    public TMP_InputField _inputRecoveryEmail;
    public Button _sendButton;

    [Header("Character player list")]
    public GameObject _charactersPanel;
    [SerializeField] private PlayerCharList _playerCharList;
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
        _firstInput.Select();
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
        EnterPanel();        
    }

    public void RegisterButton()
    {
        if (_inputRegisterPass.text.Length < 6)
        {
            _warning.SetActive(true);
            _warningText.text = "Password too short";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = _inputRegisterEmail.text,
            Password = _inputRegisterPass.text,

            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        _warning.SetActive(true);
        _warningText.text = "Account created";
        Debug.Log("Account created");
    }

    private void OnError(PlayFabError error)
    {
        _warning.SetActive(true);
        _warningText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }

    public void RrecoveryPassButton()
    {
        var request = new SendAccountRecoveryEmailRequest 
        { 
            Email = _inputRecoveryEmail.text, 
            TitleId = "B00EF" 
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPassRecovery, OnError);
    }

    private void OnPassRecovery(SendAccountRecoveryEmailResult result)
    {
        _recoveryPanel.SetActive(false);
        _entryPanel.SetActive(true);
        _warning.SetActive(true);
        _warningText.text = "Pass was send on your email";
        Debug.Log("Pass was send on your email");
    }

    private void EnterPanel()
    {
        if (_input.Player.Tab.triggered)
        {
            Selectable nex = _system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (nex != null)
            {
                nex.Select();
            }
        }
        if (_input.Player.Enter.triggered)
        {
            _entryButton.onClick.Invoke();
        }
    }
    public void EnterButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = _inputEmail.text,
            Password = _inputPass.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginIn, OnError);
    }

    private void LoginIn(LoginResult result)
    {
        _inputEmail.text = "";
        _inputPass.text = "";

        _entryPanel.SetActive(false);
        _charactersPanel.SetActive(true);
        _logout.gameObject.SetActive(true);
       
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
        if (_playerCharList.CharCreator.CountCharacters < 1)
            _charCreate.SetActive(true);
        else if (_playerCharList.CharCreator.CountCharacters == 1)
        {
            _warningText.text = "Now you can create only one character";
            _warning.SetActive(true);
        }
    }

    public void CreateNewListInMenu()
    {
        _playerCharList.CharCreator.CreateNewCharList(_inputNameCharacter.text, _gender.text, "Start location");
        GameObject charList = Instantiate(_charListPrefab, _previewPanel.transform);
        var list = _charListPrefab.GetComponent<Char_UI>();
        list.Sets(_inputNameCharacter.text, _gender.text, "Start location");
        ClearAfterCreate();
    }
    
    private void ClearAfterCreate()
    {
        _inputNameCharacter.text = "";
        _gender.text = "Choose gender";
    }
   
  
    //private void CreateChars(PlayerCharList list)
    //{
    //    if (list._gender == "Male")
    //    {
    //        GameObject charPreview = Instantiate(_malePrefab, _previewCharacter.transform);
    //        InventoryHolder charSets = charPreview.GetComponent<InventoryHolder>();
    //        charSets.Inventory.SetName(name);
    //        charSets.Inventory.SetLocation(list._location);
    //        charSets._uiPlayer.SetActive(false);
    //    }
    //    if (list._gender == "Female")
    //    {
    //        Debug.Log("Need female prefab");
    //    }
    //}
    public void CleanWarningMessage()
    {
        _warningText.text = "";
    }
}

   

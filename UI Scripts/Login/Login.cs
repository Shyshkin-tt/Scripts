using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{
    EventSystem _system;
    PlayerController _input;
    public Selectable _firstInput;

    [Header("Top buttons")]
    public Button _exit;
    public Button _settings;

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

    [Header("Warning panel")]
    public GameObject _warning;
    public TextMeshProUGUI _warningText;

    private void Awake()
    {
        _input = new PlayerController();
        _system = EventSystem.current;        
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
    private void LoginIn(LoginResult result)
    {        
        SceneManager.LoadScene("PlayerCharList");
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
    public void EnterButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = _inputEmail.text,
            Password = _inputPass.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, LoginIn, OnError);
    }
    public void CleanWarningMessage()
    {
        _warningText.text = "";
    }
}


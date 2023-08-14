using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static LocationData;
using UnityEngine.WSA;
using static Cinemachine.DocumentationSortingAttribute;
using Unity.VisualScripting;
using System;

public class PortalDisplay : MonoBehaviour
{
    InventoryHolder _holder;
    private string _type = "Tower";
    public Locations _portalLevels;

    public LocationData _startCity;

    public TMP_Dropdown _menu;
    private LocationData[] _level;

    [Header("Level info")]
    private string _currentScene;
    public TextMeshProUGUI _tierLevel;
    public TextMeshProUGUI _typeLevel;

    public Button _goButton;
    public Button _returnButton;

    [SerializeField] string _chooseLevel;
    [SerializeField] string _point;

    private void Awake()
    {
        _holder = GetComponentInParent<InventoryHolder>();

        SetDropdownMenu();

        OffCityButton();
    }

    private void OffCityButton()
    {
        if (_currentScene == "TownStart") _returnButton.gameObject.SetActive(false);
    }

    private void SetDropdownMenu()
    {
        _level = _portalLevels.locations;

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        _currentScene = SceneManager.GetActiveScene().name;

        TMP_Dropdown.OptionData currentScene = new TMP_Dropdown.OptionData(_currentScene);
        options.Add(currentScene);

        for (int i = 0; i < _level.Length; i++)
        {
            LocationData level = _level[i];

            // Если название сцены совпадает с названием текущей сцены, пропускаем ее
            if (level.locationName == _currentScene)
            {
                continue;
            }

            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(level.locationName);
            options.Add(option);

            
        }
        
        _menu.AddOptions(options);
        _menu.onValueChanged.AddListener(OnMenuValueChanged);
    }

    private void OnMenuValueChanged(int index) 
    {
        _chooseLevel = _menu.options[index].text;

        foreach (var levels in _level)
        {
            if (_chooseLevel == levels.locationName)
            {
                _point = levels.spawnPoints[0].nameSpawn;
            }
        }
    }

    public void GoButton()
    {
        Debug.Log("Go to level " +  _chooseLevel + " in point " + _point);
        LocationManager.Instance.MovePlayer(_chooseLevel, _point, _holder, _type);
    }
    public void ReturnToCityButton()
    {

        Debug.Log("Return to level " + _startCity.spawnPoints[2].nameSpawn);
        LocationManager.Instance.MovePlayer(_startCity.locationName, _startCity.spawnPoints[2].nameSpawn, _holder, "World");
    }

}

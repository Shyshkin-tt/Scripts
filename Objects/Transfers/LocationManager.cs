using Cinemachine;
using PlayFab.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LocationManager : MonoBehaviour
{
    private static LocationManager _instance;

    public Camera _playerCamera;

    [SerializeField] private InventoryHolder _holder;

    [SerializeField] SceneLoader _loader;

    public static LocationManager Instance => _instance ?? (_instance = Resources.Load<LocationManager>("LocationManager"));


    public Locations locationList;
    public Locations towerLevelList;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _loader = FindObjectOfType<SceneLoader>();

    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject charLoad = Instantiate(_loader.Char);
        _holder = FindObjectOfType<InventoryHolder>();
        charLoad.gameObject.transform.position = _loader.Position;
        SaveAndLoadManager.LoadInventory(_loader.CharName);
        SaveAndLoadManager.LoadPlayerXP(_loader.CharName);

        _holder._uiPlayer.SetActive(true);

        _holder.SetNameAndLoc(_loader.CharName, _loader.Location, _loader.Position);

        Camera cam = Instantiate(_playerCamera);
        var camLook = cam.GetComponentInChildren<CinemachineFreeLook>();
        camLook.Follow = charLoad.transform;
        camLook.LookAt = charLoad.transform;

        var action = charLoad.GetComponent<ActionController>();
        action._camera = cam;
    }

    public void MovePlayer(string sceneName, string spawnPointName, InventoryHolder holder, string mapType)
    {
        LocationData targetLocation = null;
        // Найти локацию по имени сцены
        if (mapType == "World")
        {
            foreach (LocationData locationData in locationList.locations)
            {
                if (locationData.locationName == sceneName)
                {
                    targetLocation = locationData;
                    break;
                }
            }
        }
        else if (mapType == "Tower")
        {
            foreach (LocationData locationData in towerLevelList.locations)
            {
                if (locationData.locationName == sceneName)
                {
                    targetLocation = locationData;
                    break;
                }
            }
        }

        // Если найдена локация
        if (targetLocation != null)
        {
            // Найти точку спавна по имени
            Vector3 spawnPoint = Vector3.zero;
            foreach (LocationData.SpawnPoint spawnPointData in targetLocation.spawnPoints)
            {
                if (spawnPointData.nameSpawn == spawnPointName)
                {
                    spawnPoint = spawnPointData.spawnPoint.transform.position;
                    break;
                }
            }

            _loader.SetStats(sceneName, spawnPoint, holder.Inventory.Name);

            holder.Inventory.SetLocation(sceneName);
            holder.Inventory.SetCoord(spawnPoint);

            SaveAndLoadManager.SaveInventory(_loader.CharName);
            SaveAndLoadManager.SavePlayerXP(_loader.CharName);
            // Загрузить сцену
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Location with name " + sceneName + " not found.");
        }
    }
}

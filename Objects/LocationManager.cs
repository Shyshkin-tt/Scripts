using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationManager : MonoBehaviour
{
    private static LocationManager _instance;
    public static LocationManager Instance => _instance ?? (_instance = Resources.Load<LocationManager>("LocationManager"));


    public Locations locationList;

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
    }
    public void MovePlayer(string sceneName, string spawnPointName)
    {
        // Найти локацию по имени сцены
        LocationData targetLocation = null;
        foreach (LocationData locationData in locationList.locations)
        {
            if (locationData.locationName == sceneName)
            {
                targetLocation = locationData;
                break;
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
                    spawnPoint = spawnPointData.spawnPoint;
                    break;
                }
            }

            // Переместить игрока в указанную точку спавна
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnPoint;
            }

            // Загрузить сцену
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Location with name " + sceneName + " not found.");
        }
    }
}
//private void Start()
//{
//    // Пример использования списка локаций и точек спавна
//    foreach (LocationData locationData in locationList.locations)
//    {
//        Debug.Log("Location: " + locationData.locationName);

//        Vector3[] spawnPointsCoordinates = locationData.GetSpawnPointsCoordinates();
//        foreach (Vector3 spawnPoint in spawnPointsCoordinates)
//        {
//            Debug.Log("Spawn Point: " + spawnPoint);
//        }
//    }
//}
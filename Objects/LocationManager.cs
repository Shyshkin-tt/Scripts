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
        // ����� ������� �� ����� �����
        LocationData targetLocation = null;
        foreach (LocationData locationData in locationList.locations)
        {
            if (locationData.locationName == sceneName)
            {
                targetLocation = locationData;
                break;
            }
        }

        // ���� ������� �������
        if (targetLocation != null)
        {
            // ����� ����� ������ �� �����
            Vector3 spawnPoint = Vector3.zero;
            foreach (LocationData.SpawnPoint spawnPointData in targetLocation.spawnPoints)
            {
                if (spawnPointData.nameSpawn == spawnPointName)
                {
                    spawnPoint = spawnPointData.spawnPoint;
                    break;
                }
            }

            // ����������� ������ � ��������� ����� ������
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnPoint;
            }

            // ��������� �����
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
//    // ������ ������������� ������ ������� � ����� ������
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
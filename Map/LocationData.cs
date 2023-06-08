using UnityEngine;

[CreateAssetMenu(menuName = "Map/Location Data")]
public class LocationData : ScriptableObject
{
    [System.Serializable]
    public class SpawnPoint
    {
        public string nameSpawn;
        public Vector3 spawnPoint;
    }

    public string locationName;
    public SpawnPoint[] spawnPoints;

    public Vector3[] GetSpawnPointsCoordinates()
    {
        Vector3[] coordinates = new Vector3[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            coordinates[i] = spawnPoints[i].spawnPoint;
        }
        return coordinates;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnPoints : MonoBehaviour
{
    public GameObject _spawnPointPrefab;
    public Collider[] _ground;
    public float spawnRadius;
    public int spawnCount;
    public GameObject parentContainer;

    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            DoSpawn();
        }
    }

    public void DoSpawn()
    {
        if (_spawnPointPrefab != null && _ground != null && _ground.Length > 0)
        {
            int randomIndex = Random.Range(0, _ground.Length);
            Collider targetCollider = _ground[randomIndex];

            Bounds colliderBounds = targetCollider.bounds;

            bool isSpawnPointValid = false;
            Vector3 spawnPosition = Vector3.zero;

            while (!isSpawnPointValid)
            {
                // Получаем случайную позицию в пределах границ коллайдера
                float randomX = Random.Range(colliderBounds.min.x, colliderBounds.max.x);
                float randomZ = Random.Range(colliderBounds.min.z, colliderBounds.max.z);
                Vector3 randomPoint = new Vector3(randomX, colliderBounds.max.y + 1f, randomZ);

                // Проверяем наличие объектов с бокс-коллайдером в радиусе точки спавна
                Collider[] colliders = Physics.OverlapSphere(randomPoint, spawnRadius);
                bool isPointOccupied = false;

                foreach (Collider collider in colliders)
                {
                    if (collider != targetCollider && collider is BoxCollider)
                    {
                        isPointOccupied = true;
                        break;
                    }
                }

                if (!isPointOccupied)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(randomPoint, Vector3.down, out hit, Mathf.Infinity))
                    {
                        spawnPosition = hit.point;
                        isSpawnPointValid = true;
                    }
                }
            }

            GameObject spawnedObject = Instantiate(_spawnPointPrefab, spawnPosition, Quaternion.identity);
            spawnedObject.transform.parent = parentContainer.transform;
        }
        else
        {
            Debug.LogError("Необходимо настроить objectToSpawn и targetColliders!");
        }
    }
}

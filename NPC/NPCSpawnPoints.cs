using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnPoints : MonoBehaviour
{
    [System.Serializable]
    public class SpawnSettings
    {
        public List<NPCData> npcList;
        public Collider groundCollider;
        public int spawnCount;
    }

    public List<SpawnSettings> spawnSettings;
    public GameObject spawnPointPrefab;
    public float spawnRadius;
    public GameObject parentContainer;

    private void Start()
    {
        foreach (var setting in spawnSettings)
        {
            for (int i = 0; i < setting.spawnCount; i++)
            {
                DoSpawn(setting);
            }
        }
    }

    public void DoSpawn(SpawnSettings settings)
    {
        if (spawnPointPrefab != null && settings.groundCollider != null)
        {
            Collider targetCollider = settings.groundCollider;

            Bounds colliderBounds = targetCollider.bounds;

            bool isSpawnPointValid = false;
            Vector3 spawnPosition = Vector3.zero;

            int maxAttempts = 50;  // ������������ ���������� �������
            int spawnAttempts = 0; // ������� �������

            while (!isSpawnPointValid && spawnAttempts < maxAttempts)
            {
                // �������� ��������� ������� � �������� ������ ����������
                float randomX = Random.Range(colliderBounds.min.x, colliderBounds.max.x);
                float randomZ = Random.Range(colliderBounds.min.z, colliderBounds.max.z);
                Vector3 randomPoint = new Vector3(randomX, colliderBounds.max.y + 1f, randomZ);

                // ��������� ������� �������� � ����-����������� � ������� ����� ������
                Collider[] colliders = Physics.OverlapSphere(randomPoint, spawnRadius);
                bool isPointOccupied = false;

                foreach (Collider collider in colliders)
                {
                    if (collider != targetCollider && collider is Collider)
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

                spawnAttempts++;
            }

            if (isSpawnPointValid)
            {
                GameObject spawnedObject = Instantiate(spawnPointPrefab, spawnPosition, Quaternion.identity);
                spawnedObject.transform.parent = parentContainer.transform;
                var spawn = spawnedObject.GetComponent<NPCSpawn>();

                spawn.SetNPCList(settings.npcList);
            }
            else
            {
                Debug.LogWarning("�� ������� ����� ����� ������ �� " + settings.groundCollider.name + " ����� " + maxAttempts + " �������.");
            }
        }
        else
        {
            Debug.LogError("���������� ��������� spawnPointPrefab � groundCollider!");
        }
    }

}

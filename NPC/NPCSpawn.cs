using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public GameObject _npc;
    public GameObject _spawnPoint;

    [SerializeField] private float _spawnTime;
    [SerializeField] private float _spawnTimeLeft;
    private void Awake()
    {
        
    }
    private void Start()
    {
        SpawnNPC();

        _spawnTime = 40f;
    }

    private void Update()
    {
        if (_spawnPoint.transform.childCount == 0)
        {
            _spawnTimeLeft += Time.deltaTime;
        }
    }
    
    public void SpawnNPC()
    {
        GameObject npc = Instantiate(_npc, _spawnPoint.transform.position, Quaternion.identity);
        npc.transform.parent = _spawnPoint.transform;

        var mob = npc.GetComponent<NPC>();
        mob._spawn = _spawnPoint;
    }

    public void StartSpawnDelay()
    {
        StartCoroutine(SpawnDelay());
    }

    public IEnumerator SpawnDelay()
    {        
        yield return new WaitForSeconds(_spawnTime);
        SpawnNPC();
        Debug.Log("Spanw NPC");
        _spawnTimeLeft = 0;
        yield break;
    }
}

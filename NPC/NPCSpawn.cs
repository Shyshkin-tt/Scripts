using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public GameObject _npc;
    public GameObject _spawnPoint;
    [SerializeField] private float _spawnTime;
    [SerializeField] private bool _needSpawn = false;
    [SerializeField] private bool _spawned;

    private void Awake()
    {
        _spawnTime = 40f;
    }
    private void Start()
    {
        SpawnNPC();
    }

    private void Update()
    {
        if (_spawnPoint.transform.childCount == 0 && !_spawned)
        {
            _needSpawn = true;
            StartCoroutine(SpawnDelay());
        }
        else
        {
            _needSpawn = false;            
        }
    }
    
    public void SpawnNPC()
    {
        GameObject npc = Instantiate(_npc, _spawnPoint.transform.position, Quaternion.identity);
        npc.transform.parent = _spawnPoint.transform;       
    }

    public IEnumerator SpawnDelay()
    {
        _spawned = true;
        yield return new WaitForSeconds(_spawnTime);
        SpawnNPC();
        Debug.Log("Spanw NPC");
        _spawned = false;

    }
}

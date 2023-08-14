using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public List<NPCData> _npcList;
    public GameObject _spawnPoint;

    [SerializeField] private float _spawnTime;
    [SerializeField] private float _spawnTimeLeft;

    private int[] _chance = { 45, 25, 15, 10, 5 };
    private int _total = 100;

    [SerializeField] private int _chanceSpawn;
    private void Awake()
    {
        
    }
    private void Start()
    {
        foreach (var chance in _chance)
        {
            _total += chance;
        }

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
        while (_spawnPoint.transform.childCount == 0)
        {
            _chanceSpawn = Random.Range(1, _total);           

            foreach (var npcData in _npcList)
            {
                if (_chanceSpawn <= npcData.SpawnChance)
                {
                    GameObject npc = Instantiate(npcData.NPCPrefab, _spawnPoint.transform.position, Quaternion.identity);
                    npc.transform.parent = _spawnPoint.transform;

                    var mob = npc.GetComponent<NPC>();
                    mob.SetSpawnNPC();
                    mob._spawn = _spawnPoint;
                    break;
                }
                else
                {
                    _chanceSpawn -= npcData.SpawnChance;
                }
            }
        }            
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

    public void SetNPCList(List<NPCData> npcList)
    {
        _npcList = npcList;
    }
}

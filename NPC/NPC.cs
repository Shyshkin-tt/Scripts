using System;
using System.Collections;
using UnityEngine.Events;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Unity.VisualScripting;


public class NPC : MonoBehaviour
{

    public Slider _hpSlider;
    public Canvas _canvas;

    private NavMeshAgent _navMesh;
    private Animator _animator;
    private Rigidbody _rb;
    private NPCMakeDamage _makeDamage;

    private float _timeAfterSpawn = 5f;

    //============= STATS ==============\\
    private int _npcTier;
    private int _npcLvl;

    private int _hp;
    private int _mp;
    private int _hpValue;
    private int _mpValue;
    private float _attackSpeed;

    private int _xp;

    private int _patrolZone;
    private float _patrolTime;
    private float _aggroZone;

    private float _patrolSpeed;
    private float _aggroSpeed;
    private float _returnSpeed;
    private float _currentSpeed;

    private int _minCoins;
    private int _maxCoins;
    
    //===============================\\
    public Coins _coinPrefab;
    private bool _coinsSpawned;
    public LootBage lootBage;
    private bool _trySpawnLootBag;
    public NPCData _npcData;
    private bool _giveXp;
    //===============================\\

    [SerializeField] protected NPCCreator _npcCreator;
    public NPCCreator NPCCreator => _npcCreator;

    [Header("Logic")]
    [SerializeField] private bool _move = false;
    [SerializeField] private bool _patrol = false;
    [SerializeField] private bool _isAggro = false;
    [SerializeField] private bool _returnToSpawn = false;
    [SerializeField] private bool _dead = false;    


    private float _lastAttack = 0f;

    private InventoryHolder _target;
    [SerializeField] private float _toTarget;
    [SerializeField] private Vector3 _targetPoint;

    public GameObject _spawn;
    [SerializeField] private float _toSpawn;
    [SerializeField] private Vector3 _spawnPoint;

    private float _lifeTime;

    private void Awake()
    {
        _canvas.gameObject.SetActive(false);

        _navMesh = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _makeDamage = GetComponentInChildren<NPCMakeDamage>();
        _rb = GetComponent<Rigidbody>();

        _coinsSpawned = false;
        _trySpawnLootBag = false;
        _giveXp = false;

        _npcCreator = new NPCCreator(_npcTier, _npcLvl, _hp, _mp, _hpValue, _mpValue, _attackSpeed, _xp,
            _patrolTime, _patrolZone, _aggroZone, _returnSpeed, _currentSpeed, _patrolSpeed, _aggroSpeed, _minCoins, _maxCoins, _npcData);

        _npcCreator.SetStats(_npcData);

        _hpSlider.maxValue = NPCCreator.NpcHP;
        _hpSlider.value = NPCCreator.NpcHPValue;

        StartCoroutine(AfterSpawn());
    }

    private void Start()
    {        
        _spawnPoint = _spawn.transform.position;        
    }

    private void Update()
    {       
        _animator.SetFloat("Speed", _navMesh.velocity.magnitude);
        _toSpawn = Vector3.Distance(transform.parent.position, transform.position);
        _toTarget = Vector3.Distance(transform.position, _targetPoint);
        
        _lifeTime += Time.deltaTime;
        _timeAfterSpawn -= Time.deltaTime;

        _hpSlider.value = NPCCreator.NpcHPValue;

        if (NPCCreator.NpcHPValue <= 0)
            _dead = true;
        else _dead = false;

        if (_animator.GetFloat("Speed") == 0) _move = false;
        else _move = true;        

        if (!_dead)
        {
            if (!_isAggro && !_returnToSpawn)
                AggroCheck();
        }
        else if (_dead)
        {
            StartCoroutine(Die(3f));
            return;
        }

        if (_target != null && !_dead)
        {
            Vector3 direction = _target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _rb.MoveRotation(targetRotation);
        }


    }
    private IEnumerator AfterSpawn()
    { 
        yield return new WaitForSeconds(2f);
        if (_isAggro || _dead) yield break;
        _patrol = true;
        _navMesh.enabled = true;

        NPCCreator.SetCurrentSpeed(_npcData.PatrolSpeed);

        StartCoroutine(Patrol());

        yield break;
    }
    private IEnumerator Patrol()
    {
        while (_patrol)
        {
            if (_isAggro || _dead) yield break;
            
            if (!_move)
            {
                SetRandomDestination();
                StartCoroutine(Move());
                yield return new WaitForSeconds(NPCCreator.PatrolTime);
            }
            yield return null;
        }
    }
    private bool SetRandomDestination()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * NPCCreator.PatrolZone;
        randomDirection += _spawnPoint; // Прибавляем смещение относительно _spawnPoint

        // Находим ближайшую точку на поверхности с помощью Raycast
        if (Physics.Raycast(randomDirection + Vector3.up * 100f, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            // Проверяем, что точка пересечения не ближе чем 1.5 к самому объекту
            if (Vector3.Distance(hit.point, transform.position) >= 1.5f)
            {
                _targetPoint = hit.point;
                _navMesh.SetDestination(_targetPoint);

                // Отобразить луч в сцене и точку пересечения
                Debug.DrawLine(randomDirection + Vector3.up * 100f, hit.point, Color.green);
                Debug.DrawRay(hit.point, Vector3.up * 2f, Color.cyan);               

                return true;
            }
        }

        //// Если не удалось найти точку на поверхности, устанавливаем точку на уровне _spawnPoint
        _targetPoint = new Vector3(randomDirection.x, _spawnPoint.y, randomDirection.z);
        _navMesh.SetDestination(_targetPoint);

        // Отобразить луч в сцене
        Debug.DrawLine(randomDirection + Vector3.up * 100f, _targetPoint, Color.red);

        return false;
    }
    private IEnumerator Move()
    {
        while (_toTarget > 0.3)
        {
           
            if (_isAggro || _dead) yield break;

            _navMesh.SetDestination(_targetPoint);
            _navMesh.speed = NPCCreator.CurrentSpeed;
            _navMesh.isStopped = false;
            yield return null;            
        }        
    }
    private void AggroCheck()
    {
        if (!_returnToSpawn)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, NPCCreator.AggroZone);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    
                    _patrol = false;
                    _isAggro = true;
                    SetTarget(collider.gameObject);                    
                    _targetPoint = _target.transform.position;
                    _navMesh.stoppingDistance = 1.2f;
                    _toTarget = Vector3.Distance(_target.transform.position, transform.position);
                    NPCCreator.SetCurrentSpeed(NPCCreator.AggroSpeed);
                    StartCoroutine(Attacking());
                    break;
                }
            }
        }
    }
    private IEnumerator Attacking()
    {
        while (_isAggro)
        {

            if (_dead) yield break;
            if (_toSpawn > 15)
            {
                StartCoroutine(ReturnToPatrol());
                yield break;
            }
            _canvas.gameObject.SetActive(true);
            _navMesh.stoppingDistance = 1.2f;
            _targetPoint = _target.transform.position;
            StartCoroutine(FollowTarget());
            float currentTime = Time.time;

            if (_isAggro && _navMesh.remainingDistance < 1.1f && !_navMesh.pathPending)
            {

                if (currentTime - _lastAttack >= _npcData.AttackSpeed)
                {
                    _makeDamage._canHit = true;
                    _animator.SetTrigger("Hit");
                    _lastAttack = currentTime;

                }
            }
            yield return null;
        }
    }
    private IEnumerator FollowTarget()
    {
        while (_toTarget > 1)
        {
            if (_dead) yield break;

            if (_returnToSpawn) yield break;
            _navMesh.SetDestination(_targetPoint);
            _navMesh.speed = NPCCreator.CurrentSpeed;
            _navMesh.isStopped = false;
            yield return null;            
        }

    }
    private IEnumerator Die(float delay)
    {
        _animator.SetTrigger("Die");

        if (!_giveXp)
        {
            NPCCreator.GiveXP(_target);
            _giveXp = true;
        }

        _navMesh.enabled = false;

        if(!_coinsSpawned) SpawnCoins();
        if(!_trySpawnLootBag) SpawnLoot();

        yield return new WaitForSeconds(delay);

        var needSpawn = GetComponentInParent<NPCSpawn>();
        needSpawn.StartSpawnDelay();

        Destroy(this.gameObject);
    }
    private bool SpawnCoins()
    {
        _coinsSpawned = true;
        var loot = transform.Find("Loot");
        if (_npcData.HaveCoins)
        {
            Coins newCoin = Instantiate(_coinPrefab, transform.position, transform.rotation);
            newCoin.SetValue(UnityEngine.Random.Range(NPCCreator.MinCoins, NPCCreator.MaxCoins));
        }
        else return false;

        return true;       
    }
    private bool SpawnLoot()
    {
        _trySpawnLootBag = true;

        var chanseLoot = UnityEngine.Random.Range(1, 101);

        if (chanseLoot < 35)
        {
            LootBage bag = Instantiate(lootBage, transform.position, transform.rotation);            
        }
        else Debug.Log("SRY no loot");
        
        return true;
    }
    private void LateUpdate()
    {
        _canvas.transform.LookAt(transform.position + Camera.main.transform.forward);
    }
    private IEnumerator ReturnToPatrol()
    {
        _isAggro = false;
        _returnToSpawn = true;
        _targetPoint = _spawnPoint;
        NPCCreator.SetCurrentSpeed(_npcData.ReturnSpeed);
        NPCCreator.SetValueStats();
        _navMesh.stoppingDistance = 0f;

        ClearTarget();

        while (_toTarget > 1)
        {
            _navMesh.SetDestination(_targetPoint);
            _navMesh.speed = NPCCreator.CurrentSpeed;
            _navMesh.isStopped = false;
            _canvas.gameObject.SetActive(false);
            yield return new WaitForSeconds(3f);
            StopAllCoroutines();
            NPCCreator.SetCurrentSpeed(_npcData.PatrolSpeed);
            StartCoroutine(AfterSpawn());
            _returnToSpawn = false;
            yield break;
        }        
    }
    public void SetTarget(GameObject target)
    {
        _target = target.GetComponent<InventoryHolder>();

        _isAggro = true;

        NPCCreator.SetCurrentSpeed(NPCCreator.AggroSpeed);

        StartCoroutine(Attacking());
    }
    private void ClearTarget()
    {
        _target = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_targetPoint, 0.1f);
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, NPCCreator.AggroZone);
    }



}

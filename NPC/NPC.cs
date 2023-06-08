using System;
using System.Collections;

using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class NPC : MonoBehaviour
{
    [SerializeField] private int HP;
    [SerializeField] private int HPValue;
    public Coins _coinPrefab;
    public LootBage lootBage;
    public NPCData _npcData;
    
    public Slider _hp;
    public Canvas _canvas;
    private NavMeshAgent _navMesh;
    private NPCMakeDamage _makeDamage;

    private bool _move = false;
    private bool _patrol = false;
    private bool _isAggro = false;
    private bool _returnToSpawn = false;
    private bool _dead = false;
    private bool _coinSpawn = false;
    private bool _bagSpawn = false;
   

    private float _toTarget;
    private float _toSpawn;
    private float _moveSpeed;
    [SerializeField] private float _timeAfterSpawn = 5f;
    private float _patrolZone = 2f;

    private float _stopTime = 5f;
    private float _lastAttack = 0f;

    [SerializeField] private float _lifeTime;

    private float _zoneAggro;
    private Animator _animator;
    public GameObject _point;
    [SerializeField] private Rigidbody _rb;
    private Vector3 _spawnPoint;
    private Vector3 _targetPoint;
    private GameObject _target;
    [SerializeField] Collider _ground;
   

    private void Awake()
    {       
        HP = _npcData.HP;
        HPValue = HP;
        _hp.maxValue = HP;
        _hp.value = HPValue;
        _zoneAggro = _npcData.ZoneAggro;
        
        _navMesh = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        StartCoroutine(AfterSpawn());
        _makeDamage = GetComponentInChildren<NPCMakeDamage>();
        _rb = GetComponent<Rigidbody>();
        _ground = GetComponentInParent<Collider>();
    }

    private void Start()
    {

        _point = transform.parent.gameObject;
        _spawnPoint = _point.transform.position;
        _targetPoint = _spawnPoint;
    }

    private void Update()
    {       
        _animator.SetFloat("Speed", _navMesh.velocity.magnitude);
        _toSpawn = Vector3.Distance(transform.parent.position, transform.position);
        _toTarget = Vector3.Distance(transform.position, _targetPoint);
        _hp.value = HPValue;
        _lifeTime += Time.deltaTime;
        _timeAfterSpawn -= Time.deltaTime;

        if (HPValue <= 0)
            _dead = true;
        else _dead = false;

        if (_timeAfterSpawn < 0)
        {
            if (!_dead)
            {
                MoveSpeed();
                AggroCheck();
                FollowTarget();
                ReturnToPatrol();
                Attacking();
            }
            else if (_dead)
            {
                StartCoroutine(Die(3f));
                return;
            }
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
        yield return new WaitForSeconds(5f);
        
        _patrol = true;
        _navMesh.enabled = true;
        StopCoroutine(AfterSpawn());
        StartCoroutine(Patrol());
        yield break;
    }

    private IEnumerator Die(float delay)
    {
        _animator.SetTrigger("Die");
        
        _navMesh.enabled = false;
        SpawnLoot();

        yield return new WaitForSeconds(delay);
        
        Destroy(this.gameObject);
    }

    private void SpawnLoot()
    {
        var loot = transform.Find("Loot");

        if (!_coinSpawn)
        {
            Coins newCoin = Instantiate(_coinPrefab, transform.position, transform.rotation);
            newCoin.SetValue(UnityEngine.Random.Range(newCoin.minValue, newCoin.maxValue));

            _coinSpawn = true;
        }

        if (!_bagSpawn)
        {
            LootBage bag = Instantiate(lootBage, transform.position, transform.rotation);
            bag.transform.SetParent(loot);
            _bagSpawn = true;
        }
    }
    private void LateUpdate()
    {
        //_canvas.transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    private void Attacking()
    {
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
    }


    private void ReturnToPatrol()
    {
        if (_toSpawn > 15f)
        {
            _isAggro = false;
            _returnToSpawn = true;
            _targetPoint = _spawnPoint;
            _navMesh.stoppingDistance = 0f;
            HPValue = HP;
        }
        else if (!_patrol && _returnToSpawn && _toSpawn < 1f)
        {
            _returnToSpawn = false;
            _move = false;
            _patrol = true;
            _target = null;
            _zoneAggro = _npcData.ZoneAggro;
            StartCoroutine(Patrol());
            return;
        }
    }

    private void FollowTarget()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Idle"))
        {
            if (_isAggro && _toTarget > 1f)
            {
                Move(_targetPoint);
            }
            else if (_patrol && _toTarget > 0.1f)
            {
                Move(_targetPoint);
            }
            else if (_returnToSpawn && _toTarget > 0.5f)
            {
                Move(_targetPoint);
            }
            else
            {
                _move = false;
                _navMesh.isStopped = true;
            }
        }
        
    }

    private void MoveSpeed()
    {

        if (!_move)
            _moveSpeed = 0f;
        else if (_patrol && _move)
            _moveSpeed = _npcData.PatrolSpeed;
        else if (_isAggro && _move)
            _moveSpeed = _npcData.RunSpeed;
        else if (_returnToSpawn && _move)
            _moveSpeed = 6f;
        else
            _moveSpeed = 0f;
    }
    private IEnumerator Patrol()
    {
        while (_patrol)
        {
            if (_isAggro)
                yield break;

            SetRandomDestination();

            if (!_move)
            {
                yield return new WaitForSeconds(_stopTime);
            }
            yield return null;
        }
    }

    private bool SetRandomDestination()
    {
        // Получаем случайную точку внутри круга на горизонтальной поверхности
        
        Vector3 randomPosition = _spawnPoint + UnityEngine.Random.insideUnitSphere * _patrolZone;      
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(randomPosition, out navMeshHit, _patrolZone, NavMesh.AllAreas))
        {
            // Устанавливаем полученную точку патрулирования в приватную переменную
            _targetPoint = navMeshHit.position;
            Debug.Log("Fail");
            // Устанавливаем случайную точку патрулирования
            _navMesh.SetDestination(_targetPoint);
            return true;
        }
        _targetPoint = _spawnPoint;
        return false;
    }

    private void Move(Vector3 target)
    {
        _move = true;
        _navMesh.SetDestination(target);
        _navMesh.speed = _moveSpeed;
        _navMesh.isStopped = false;              
    }
    private void AggroCheck()
    {
        if (!_returnToSpawn)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _zoneAggro);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    _patrol = false;
                    _isAggro = true;                    
                    _target = collider.gameObject;
                    _targetPoint = _target.transform.position;
                    _navMesh.stoppingDistance = 1.2f;
                    _toTarget = Vector3.Distance(_target.transform.position, transform.position);
                    _zoneAggro = 10f;
                    
                    break;
                }
            }
        }       
    }
    
   

    public void NPCTakeDamage(int damage)
    {
        HPValue -= damage;        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_targetPoint, 0.1f);
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(transform.position, _zoneAggro);
    }



}

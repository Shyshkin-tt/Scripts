using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpell : MonoBehaviour
{
    [SerializeField] private GameObject _owner;
    [SerializeField] private Collider _hit;
    [SerializeField] private SpellData _spell;
    [SerializeField] private InventoryHolder _holder;
    [SerializeField] private List<NPC> _enemyList = new List<NPC>();

    [SerializeField] private Quaternion _direction;
    [SerializeField] private Vector3 _spawnPosition;

    [SerializeField] private float _currentLifeTime = 0f;
    [SerializeField] private float _currentDistanceRange = 0f;

    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField] private Vector3 _point;
    [SerializeField] private float _toPoint;

    [SerializeField] private bool _destroyOnTriggerEnter;

    private void Awake()
    {
        _hit = GetComponent<Collider>();
        _hit.isTrigger = true;
        //_spawnPosition = transform.position;
    }

    public void DealDamageBeforeDestroy()
    {
        StartCoroutine(DamageBeforeDestroy());
    }
    public void SetDestroyOnTriggerEnter()
    {
        _destroyOnTriggerEnter = true;
    }
    public void PlaySpellParticle()
    {
        _particleSystem.Play();
    }
    public void StartSpellMoveForward()
    {
        StartCoroutine(SpellMoveForward());
    }
    public void StartSpellMoveToPoint()
    {
        StartCoroutine(SpellMoveToPoint());
    }
    public void SetDestroyTime()
    {
        StartCoroutine(DestroyTime());
    }
    public void SetSpawnPosition()
    {
        _spawnPosition = transform.position;
    }
    public void SetDirectionForward(Quaternion direction)
    {
        _direction = direction;
    }
    public void SetDirectionPoint(Vector3 direction)
    {
        _point = direction;
    }
    public void SetOwner(GameObject owner)
    {
        _owner = owner;
        _holder = _owner.GetComponent<InventoryHolder>();
    }
    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag("Enemy"))
        {
            NPC enemy = target.GetComponent<NPC>();

            if (enemy != null && !_enemyList.Contains(enemy))
            {
                _enemyList.Add(enemy);
            }

            if (_destroyOnTriggerEnter)
            {
                DealDamage();
                Debug.Log("Destro OnTriggerEnter");
                Destroy(this.gameObject);
            }
        }
    }
    private void DealDamage()
    {
        foreach (var enemyTarget in _enemyList)
        {
            var spellDamage = _spell.Damage;
            var charBonusDamage = _holder.Characteristics.MDmg;
            var totalDamage = spellDamage + charBonusDamage;
            enemyTarget.NPCCreator.NPCTakeDamage(totalDamage);
            enemyTarget.SetTarget(_owner);
        }
    }
    private IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(_spell.LifeTime);
        
        Destroy(this.gameObject);
    }
    private IEnumerator SpellMoveForward()
    {
        while (_currentDistanceRange <= _spell.DistanceRange)
        {
            _currentLifeTime += Time.deltaTime;
            _currentDistanceRange = Vector3.Distance(transform.position, _spawnPosition);
            transform.position += _direction * Vector3.forward * _spell.Speed * Time.deltaTime;
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
    private IEnumerator SpellMoveToPoint()
    {
        Vector3 moveDirection = (_point - transform.position).normalized;
        _toPoint = Vector3.Distance(_point, _hit.transform.position);
        
        while (_toPoint > _spell.DestroyDistance)
        {
            Debug.Log("must move to point");
            transform.position += moveDirection * _spell.Speed * Time.deltaTime;
            _toPoint = Vector3.Distance(_point, _hit.transform.position);
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
    private IEnumerator DamageBeforeDestroy()
    {
        float damageBefore = 0.2f;
        yield return new WaitForSeconds(damageBefore);

        DealDamage();
        float lifeTime = _spell.LifeTime;
        float timeToDestroy = lifeTime -= damageBefore;

        yield return new WaitForSeconds(timeToDestroy); // 0.5 sek

        Debug.Log("Destro DamageBeforeDestroy");
        Destroy(this.gameObject);
    }
}

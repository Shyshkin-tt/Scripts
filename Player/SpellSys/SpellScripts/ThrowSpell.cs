using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpell : MonoBehaviour
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

    private void Awake()
    {
        _hit = GetComponent<Collider>();
        _hit.isTrigger = true;
        _spawnPosition = transform.position;
    }

    private void Update()
    {
        _currentLifeTime += Time.deltaTime;
        _currentDistanceRange = Vector3.Distance(transform.position, _spawnPosition);

        SpellMoving();
    }
    public void SetOwner(GameObject owner)
    {
        _owner = owner;
        _holder = _owner.GetComponent<InventoryHolder>();
    }
    public void SetDirection(Quaternion direction)
    {
        _direction = direction;
    }
    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Enemy")
        {
            NPC enemy = target.GetComponent<NPC>();
            enemy.NPCCreator.NPCTakeDamage(_spell.Damage);

            if (enemy != null && !_enemyList.Contains(enemy))
            {
                _enemyList.Add(enemy);
            }

            foreach (var enemyTarget in _enemyList)
            {
                var spellDamage = _spell.Damage;
                var charBonusDamage = _holder.Characteristics.MDmg;
                var totalDamage = spellDamage + charBonusDamage;
                enemyTarget.NPCCreator.NPCTakeDamage(totalDamage);
                enemyTarget.SetTarget(_owner);
            }

            Destroy(this.gameObject);
        }
    }
    public void SpellMoving()
    {
        if (_currentDistanceRange <= _spell.DistanceRange)
        {
            transform.position += _direction * Vector3.forward * _spell.Speed * Time.deltaTime;
        }
        else Destroy(this.gameObject);
    }
}

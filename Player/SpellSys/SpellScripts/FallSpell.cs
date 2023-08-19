using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpell : MonoBehaviour
{
    [SerializeField] private GameObject _owner;
    [SerializeField] private Collider _hit;
    [SerializeField] private SpellData _spell;
    [SerializeField] private InventoryHolder _holder;
    [SerializeField] private List<NPC> _enemyList = new List<NPC>();
        
    [SerializeField] private Vector3 _point;
    [SerializeField] float _toPoint;

    private void Awake()
    {
        _hit = GetComponent<Collider>();
        _hit.isTrigger = true;
    }
   
    private void Update()
    {
        _toPoint = Vector3.Distance(_point, _hit.transform.position);
        MoveToPoint();
    }
    public void SetOwner(GameObject owner)
    {
        _owner = owner;
        _holder = _owner.GetComponent<InventoryHolder>();
    }
    private void MoveToPoint()
    {
        Vector3 moveDirection = (_point - transform.position).normalized;

        transform.position += moveDirection * _spell.Speed * Time.deltaTime;

        if (_toPoint <= 0.1)
        {
            foreach (var target in _enemyList)
            {
                NPC enemy = target.GetComponent<NPC>();

                var spellDamage = _spell.Damage;
                var charBonusDamage = _holder.Characteristics.MDmg;
                var totalDamage = spellDamage + charBonusDamage;

                if (enemy != null) enemy.NPCCreator.NPCTakeDamage(totalDamage);

                enemy.SetTarget(_owner);
                
            }

            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        _point = direction;
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
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaySpell : MonoBehaviour
{
    [SerializeField] private GameObject _owner;
    [SerializeField] private Collider _hit;
    [SerializeField] private SpellData _spell;
    [SerializeField] private InventoryHolder _holder;
    [SerializeField] private List<NPC> _enemyList = new List<NPC>();

    private void Awake()
    {
        _hit = GetComponent<Collider>();
        _hit.isTrigger = true;
    }
    public void SetOwner(GameObject owner)
    {
        _owner = owner;
        _holder = _owner.GetComponent<InventoryHolder>();
    }
    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Enemy")
        {
            NPC enemy = target.GetComponent<NPC>();

            if (enemy != null && !_enemyList.Contains(enemy))
            {
                _enemyList.Add(enemy);

            }

        }
    }
    public void ForDestroy()
    {
        StartCoroutine(DestroyTime());
    }

    private IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(_spell.LifeTime / 2);
        foreach (var enemyTarget in _enemyList)
        {
            var spellDamage = _spell.Damage;
            var charBonusDamage = _holder.Characteristics.MDmg;
            var totalDamage = spellDamage + charBonusDamage;
            enemyTarget.NPCCreator.NPCTakeDamage(totalDamage);
            enemyTarget.SetTarget(_owner);            
        }
        yield return new WaitForSeconds(_spell.LifeTime / 2); // 0.5 sek        

        Destroy(this.gameObject);
    }
}

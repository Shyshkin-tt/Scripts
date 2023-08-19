using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSpell : MonoBehaviour
{
    [SerializeField] private GameObject _owner;
    [SerializeField] private Collider _hit;
    [SerializeField] private SpellData _spell;
    [SerializeField] private InventoryHolder _holder;
    [SerializeField] private List<NPC> _enemyList = new List<NPC>();

    [SerializeField] private ParticleSystem _particleSystem;
    private void Awake()
    {
        _hit = GetComponentInChildren<Collider>();
        _hit.isTrigger = true;
        _particleSystem.Play();
        StartCoroutine(DestroyTime());
    }

    private IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(_spell.LifeTime);
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
 
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
            foreach (var enemyTarget in _enemyList)
            {
                var spellDamage = _spell.Damage;
                var charBonusDamage = _holder.Characteristics.PDmg;
                var totalDamage = spellDamage + charBonusDamage;
                enemyTarget.NPCCreator.NPCTakeDamage(totalDamage);
                enemyTarget.SetTarget(_owner);
            }           
        }
    }
}

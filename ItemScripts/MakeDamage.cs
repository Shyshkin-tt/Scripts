using System.Collections.Generic;
using UnityEngine;

public class MakeDamage : MonoBehaviour
{
    private CapsuleCollider _hit;    
    public bool _canHit = false;
    [SerializeField] private Animator _anim;
    [SerializeField] private List<NPC> _npcList = new List<NPC>();
    [SerializeField] private bool _animHit;

    private void Awake()
    {
        _hit = GetComponent<CapsuleCollider>();
        _hit.isTrigger = true;
    }

    private void Start()
    {
        if (_anim == null)
            _anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && _canHit)
        {
            NPC npc = other.GetComponent<NPC>();
            if (npc != null && !_npcList.Contains(npc))
            {
                _npcList.Add(npc);
                var damage = transform.parent.GetComponentInParent<InventoryHolder>().Inventory.PDmg;
                npc.NPCCreator.NPCTakeDamage(damage);              
            }            
        }
    }
    private void Update()
    {
        _canHit = _anim.GetBool("Hit");
        if (!_canHit && _npcList.Count > 0) _npcList.Clear();
    }
}

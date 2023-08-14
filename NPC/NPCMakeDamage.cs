using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMakeDamage : MonoBehaviour
{
    private Collider _hit;
    public bool _canHit = false;

    private void Awake()
    {
        _hit = GetComponent<Collider>();
        _hit.isTrigger = true;
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {        
        if (collision.tag == "Player")
        {
            if (_canHit)
            {
                var player = collision.transform.GetComponent<InventoryHolder>();
                var npc = transform.parent.GetComponentInParent<NPC>();
                if (npc != null && player != null)
                {
                    var damage = npc._npcData.PhysicDamage;
                    player.Inventory.PlayerTakeDamage(damage);
                    _canHit = false;
                }
            }                    
        }
        else if (collision.tag != "Player")
        {            
            return;
        }
    }  
}

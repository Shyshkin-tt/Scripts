using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMakeDamage : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _hit;
    [SerializeField] private GameObject _ownerBullet;
    //[SerializeField] private Animator _anim;    

    
    [SerializeField] private bool _animHit;


    private void Awake()
    {
        //_anim = _ownerBullet.GetComponentInParent<Animator>();
        _hit = GetComponent<CapsuleCollider>();
        _hit.isTrigger = true;
    }

    private void Start()
    {
        //if (_anim == null)
        //    _anim = GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            NPC npc = other.GetComponent<NPC>();
            var owner = _ownerBullet.GetComponent<InventoryHolder>();
            var damage = owner.Characteristics.PDmg;

            npc.NPCCreator.NPCTakeDamage(damage);

            npc.SetTarget(_ownerBullet);

            Destroy(this.gameObject);
        }
    }

    public void SetOwner(GameObject owner)
    {
        _ownerBullet = owner;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Animation : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _char;
    private ActionController _moving;
    private InventoryItemData _itemData;
    private InventoryHolder _holder;
    public string _itemType;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _char = GetComponent<CharacterController>();
        _moving = GetComponent<ActionController>();
        _holder = GetComponent<InventoryHolder>();
    }

    private void Update()
    {
        float speed = _char.velocity.magnitude;
        _animator.SetFloat("Speed", speed);
        AnimationPlayer();


        if (_holder.Inventory.EquipSlots.Count > 3 && _holder.Inventory.EquipSlots[3].ItemData != null)
        {
            _itemType = _holder.Inventory.EquipSlots[3].ItemData.ItemType;
        }
        else
        {
            _itemType = null;
        }

        AnimationType();
    }

    private void AnimationType()
    {
        _animator.SetBool("No Weapon", _itemType == null);
        _animator.SetBool("1H", _itemType == "1H_Weapon");
        _animator.SetBool("2H", _itemType == "2H_Weapon");
        _animator.SetBool("Polearm", _itemType == "Polearm");
    }


    private void AnimationPlayer()
    {
        if (_moving._move == true)        
            _animator.SetFloat("Speed", 5);
        else
            _animator.SetFloat("Speed", 0f);
    }
}

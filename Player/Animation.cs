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
    public string _itemType;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _char = GetComponent<CharacterController>();
        _moving = GetComponent<ActionController>();        
    }

    private void Update()
    {
        float speed = _char.velocity.magnitude;
        _animator.SetFloat("Speed", speed);
        AnimationPlayer();

        _itemType = GetComponent<InventoryHolder>().Inventory.EquipSlots[3].ItemData.ItemType;
        AnimationType();
    }

    private void AnimationType()
    {
        _animator.SetBool("No Weapon", string.IsNullOrWhiteSpace(_itemType));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Coins : MonoBehaviour
{
    [SerializeField] private int _value;

    private BoxCollider myCollider;

    private InventoryHolder _playerInventoryHolder;

    private void Awake()
    {
        myCollider = GetComponent<BoxCollider>();
        myCollider.isTrigger = true;
    }

    public void SetValue(int value)
    {
        _value = value;
    }

    private void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerInventoryHolder = other.GetComponent<InventoryHolder>();
        if (!_playerInventoryHolder)
        {
            return;
        }

        var coins = _value;

        _playerInventoryHolder.Inventory.GainGold(coins);

        Destroy(this.gameObject);
    }
}

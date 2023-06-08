using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int _maxHP;    
    [SerializeField] private int _maxMP;

    [SerializeField] private int _hpValue;
    [SerializeField] private int _mpValue;


    [Header("UI")]
    public Slider _hp;
    public TextMeshProUGUI _hpText;
    public Slider _mp;
    public TextMeshProUGUI _mpText;

    [SerializeField] private InventoryHolder _inventroyHolder;

    private void Start()
    {
       
    }

    private void Update()
    {
        Stats();
        RefreshBar();

        _inventroyHolder.Inventory.RecoveryHP();
        _inventroyHolder.Inventory.RecoveryMP();
    }

    private void RefreshBar()
    {
        _hpText.text = $"{_hpValue} / {_maxHP}";
        _mpText.text = $"{_mpValue} / {_maxMP}";
    }

    private void Stats()
    {
        _maxHP = _inventroyHolder.Inventory.Health;
        _hp.maxValue = _maxHP;
        _hpValue = _inventroyHolder.Inventory.HPValue;
        _maxMP = _inventroyHolder.Inventory.Mana;
        _mp.maxValue = _maxMP;
        _mpValue = _inventroyHolder.Inventory.MPValue;

        _hp.value = _hpValue;
        _mp.value = _mpValue;
    }
}

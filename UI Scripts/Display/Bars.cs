using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    [SerializeField] private Slider _castbar;    

    public Button _qButton;
    public TextMeshProUGUI _cdTextQ;
    public Image _spriteQ;
    public Button _wButton;
    public TextMeshProUGUI _cdTextW;
    public Image _spriteW;
    public Button _eButton;
    public TextMeshProUGUI _cdTextE;
    public Image _spriteE;
    //public Button _rButton;
    //public TextMeshProUGUI _cdTextR;

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

    [SerializeField] private UIController _uiController;

    private void Start()
    {
        ClearItemIcons();
    }

    private void Update()
    {
        Stats();
        RefreshBar();

        _uiController._holder.Characteristics.RecoveryHP();
        _uiController._holder.Characteristics.RecoveryMP();
    }

    private void RefreshBar()
    {
        _hpText.text = $"{_hpValue} / {_maxHP}";
        _mpText.text = $"{_mpValue} / {_maxMP}";
    }

    private void Stats()
    {
        _maxHP = _uiController._holder.Characteristics.Health;
        _hp.maxValue = _maxHP;
        _hpValue = _uiController._holder.Characteristics.HPValue;
        _maxMP = _uiController._holder.Characteristics.Mana;
        _mp.maxValue = _maxMP;
        _mpValue = _uiController._holder.Characteristics.MPValue;

        _hp.value = _hpValue;
        _mp.value = _mpValue;
    }
    public void SetCastBar(float castTime)
    {
        _castbar.value = 0f;
        _castbar.maxValue = castTime;
    }
    public void SetCastBarValue(float value)
    {
        _castbar.value = value;        
    }
    public void SetCastBarAddValue(float value)
    {
        _castbar.value += value;
    }
    public void CastbarOff()
    {
        _castbar.gameObject.SetActive(false);
    }
    public void CastbarOn()
    {
        _castbar.gameObject.SetActive(true);
    }
    public void SetWeaponIcons(InventoryItemData data)
    {
        if (data.SpellOne != null)
        {
            _spriteQ.sprite = data.SpellOne.Icon;
            _spriteQ.color = Color.white;
        }

        if (data.SpellTwo != null)
        {
            _spriteW.sprite = data.SpellTwo.Icon;
            _spriteW.color = Color.white;
        }

        if (data.SpellThree != null)
        {
            _spriteE.sprite = data.SpellThree.Icon;
            _spriteE.color = Color.white;
        }
    }
    public void ClearItemIcons()
    {
        _spriteQ.sprite = null;
        _spriteQ.color = Color.clear;

        _spriteW.sprite = null;
        _spriteW.color = Color.clear;

        _spriteE.sprite = null;
        _spriteE.color = Color.clear;
    }
}

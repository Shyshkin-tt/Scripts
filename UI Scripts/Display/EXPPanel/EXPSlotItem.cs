using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class EXPSlotItem : MonoBehaviour
{
    [SerializeField] ItemsXP _itemStats;
    [SerializeField] ExpirianceDisplay _expDisplay;
    public TextMeshProUGUI _name;
    public Image itemSprite;
    public TextMeshProUGUI _lvl;
    public TextMeshProUGUI _currebtXP;
    public TextMeshProUGUI _xpNextLVL;
    public Slider _slider;

    private void Awake()
    {
        _expDisplay = GetComponentInParent<ExpirianceDisplay>();
    }

    public void SetXPLVL(ItemsXP item)
    {
        _itemStats = item;

        _lvl.text = item.Level.ToString();
        _currebtXP.text = item.CurrentXP.ToString();
        _xpNextLVL.text = item.XPToNextLVL.ToString();
        _slider.maxValue = item.XPToNextLVL;
        _slider.value = item.CurrentXP;
    }

    public void OnClick()
    {
        _expDisplay._statsBonus.SetOnDisplay(_itemStats);
    }
}

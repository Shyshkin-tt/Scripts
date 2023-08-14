using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsBonus : MonoBehaviour
{
    public TextMeshProUGUI _item;

    public TextMeshProUGUI _itemPower;
    public TextMeshProUGUI _health;
    public TextMeshProUGUI _mana;
    public TextMeshProUGUI _physicDamage;
    public TextMeshProUGUI _magicDamage;
    public TextMeshProUGUI _attackSpeed;
    public TextMeshProUGUI _physicDefence;
    public TextMeshProUGUI _magicDefence;
    public TextMeshProUGUI _healthRecovery;
    public TextMeshProUGUI _manaRecovery;


    public void SetOnDisplay(ItemsXP item)
    {
        _item.text = item.NameClass.ToString();
        _itemPower.text = item.ItemPower.ToString();
        _health.text = item.Health.ToString();
        _mana.text = item.Mana.ToString();
        _physicDamage.text = item.PDmg.ToString();
        _magicDamage.text = item.MDmg.ToString();
        _attackSpeed.text = item.AttackSpeed.ToString();
        _physicDefence.text = item.PDef.ToString();
        _magicDefence.text = item.MDef.ToString();
        _healthRecovery.text = item.HPRec.ToString();
        _manaRecovery.text = item.MPRec.ToString();
    }
}

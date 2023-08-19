using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI _ip;
    [SerializeField] private TextMeshProUGUI _hp;
    [SerializeField] private TextMeshProUGUI _mp;
    [SerializeField] private TextMeshProUGUI _pdmg;
    [SerializeField] private TextMeshProUGUI _mdmg;
    //[SerializeField] private TextMeshProUGUI _as;
    [SerializeField] private TextMeshProUGUI _pdef;
    [SerializeField] private TextMeshProUGUI _mdef;
    [SerializeField] private TextMeshProUGUI _hpRec;
    [SerializeField] private TextMeshProUGUI _mpRec;
   
    public void SetItemInfo(InventoryItemData data)
    {        
        _itemName.text = $"{data.DisplayName}";
        _ip.text = $"{data.ItemPower}";
        _hp.text = $"{data.Health}";
        _mp.text = $"{data.Mana}";
        _pdmg.text = $"{data.PhysicDamage}";
        _mdmg.text = $"{data.MagicDamage}";
        //_as.text = $"{data.AttackSpeed}";
        _pdef.text = $"{data.PhysicDefence}";
        _mdef.text = $"{data.MagicDefence}";
        _hpRec.text = $"{data.HealthRecovery}";
        _mpRec.text = $"{data.ManaRecovery}";
    }
}

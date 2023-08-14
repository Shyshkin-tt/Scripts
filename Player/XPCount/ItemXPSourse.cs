using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemXPSourse
{
    [NonSerialized] protected InventoryItemData _itemData;
    [SerializeField] protected string _nameClass;
    [SerializeField] protected string _nameItem;
    [SerializeField] protected int _itemID = -1;
    [SerializeField] protected int _lvl = 0;
    [SerializeField] protected int _currentXp = 0;
    [SerializeField] protected int _xpToNextLvL = 0;

    [Header("Bonus stats")]
    [SerializeField] protected int _itemPower = 0;
    [SerializeField] protected int _health = 0;
    [SerializeField] protected int _mana = 0;
    [SerializeField] protected int _physicDamage = 0;
    [SerializeField] protected int _magicDamage = 0;
    [SerializeField] protected float _attackSpeed = 0;
    [SerializeField] protected int _physicDefence = 0;
    [SerializeField] protected int _magicDefence = 0;
    [SerializeField] protected int _healthRecovery = 0;
    [SerializeField] protected int _manaRecovery = 0;   

    public InventoryItemData ItemData => _itemData;
    public int ID => _itemID;
    public string NameClass => _nameClass;
    public string NameItem => _nameItem;
    public int Level => _lvl;
    public int CurrentXP => _currentXp;
    public int XPToNextLVL => _xpToNextLvL;
    public int ItemPower => _itemPower;
    public int Health => _health;
    public int Mana => _mana;
    public int PDmg => _physicDamage;
    public int MDmg => _magicDamage;
    public float AttackSpeed => _attackSpeed;
    public int PDef => _physicDefence;
    public int MDef => _magicDefence;
    public int HPRec => _healthRecovery;
    public int MPRec => _manaRecovery;
    public void ClearBonusStats()
    {
        
        _itemPower = 0;
        _health = 0;
        _mana = 0;
        _physicDamage = 0;
        _magicDamage = 0;
        _attackSpeed = 0;
        _physicDefence = 0;
        _magicDefence = 0;
        _healthRecovery = 0;
        _manaRecovery = 0;
    }
    public void UpdateItemBonusStat()
    {
        ClearBonusStats();
        
        var bonusForUp = 0.01;

        _itemPower += (int)(_itemData.ItemPower * (bonusForUp * _lvl));
        _health += (int)(_itemData.Health * (bonusForUp * _lvl));
        _mana += (int)(_itemData.Mana * (bonusForUp * _lvl));
        _physicDamage += (int)(_itemData.PhysicDamage * (bonusForUp * _lvl));
        _magicDamage += (int)(_itemData.MagicDamage * (bonusForUp * _lvl));
        _attackSpeed += (float)(_itemData.AttackSpeed * (bonusForUp * _lvl));
        _physicDefence += (int)(_itemData.PhysicDefence * (bonusForUp * _lvl));
        _magicDefence += (int)(_itemData.MagicDefence * (bonusForUp * _lvl));
        _healthRecovery += (int)(_itemData.HealthRecovery * (bonusForUp * _lvl));
        _manaRecovery += (int)(_itemData.ManaRecovery * (bonusForUp * _lvl));
    }

    //public void OnBeforeSerialize()
    //{

    //}
    //public void OnAfterDeserialize()
    //{
    //    if (_itemID == -1) return;// если предмет отсутствует, то ничего не делаем
    //    var db = Resources.Load<Database>("ItemDatabase");// загружаем базу данных
    //    _itemData = db.GetItem(_itemID);// получаем данные о предмете по его ID

    //}
    // : ISerializationCallbackReceiver
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemXPSourse 
{
    [NonSerialized] protected InventoryItemData _itemData;
    [SerializeField] protected string _nameClass;
    [SerializeField] protected string _nameItem;
    [SerializeField] protected int _lvl;
    [SerializeField] protected int _xp;
    [SerializeField] protected int _xpMax;

    public InventoryItemData ItemData => _itemData;
    public string NameClass => _nameClass;
    public string NameItem => _nameItem;
    public int Level => _lvl;
    public int XP => _xp;
    public int XPMax => _xpMax;

    public void AssignItemXP(ItemsXP item)
    {
        _nameClass = item._nameClass;
        _nameItem = item.NameItem;
        _lvl = item.Level; 
        _xp = item.XP;        
        _xpMax = item.XPMax;
    }
}

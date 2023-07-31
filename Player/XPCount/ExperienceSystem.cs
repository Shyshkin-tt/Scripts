using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ExperienceSystem
{
    private List<InventoryItemData> _items;

    [SerializeField] protected int _totalXp;   
    [SerializeField] protected List<ItemsXP> _itemClass;
    [SerializeField] protected List<ItemsXP> _itemName;    
    public int TotalXp => _totalXp;
    public List<ItemsXP> ClassItem => _itemClass;    
    public List<ItemsXP> ItemList => _itemName;  

    public ExperienceSystem(int totalXp, List<ItemsXP> itemClass, List<ItemsXP> itemName, Database data)
    {
        _totalXp = totalXp;
        _itemClass = itemClass;
        _itemName = itemName;
       
        _items = new List<InventoryItemData>(data.GetItemDatabase());
        
        CreatePlayerSkillTree(_items);
    }

    private void CreatePlayerSkillTree(List<InventoryItemData> data)
    {
        CreateItemClass( data);
        CreateItemList( data);
    }
    private void CreateItemClass(List<InventoryItemData> data)
    {
        _itemClass = new List<ItemsXP>();

        foreach (InventoryItemData item in data)
        {
            if (!_itemClass.Any(i => i.NameClass == item.ItemClass))
            {
                _itemClass.Add(new ItemsXP(item.ItemClass, "", 0, 0, 2300));
            }
        }
    }
    public void AddNewClassItem(string itemClass)
    {
        _itemClass.Add(new ItemsXP(itemClass, "", 0, 0, 2300));
    }    
    private void CreateItemList(List<InventoryItemData> data)
    {
        _itemName = new List<ItemsXP>();

        foreach (InventoryItemData item in data)
        {
            _itemName.Add(new ItemsXP(item.ItemClass, item.DisplayName, 0, 0, 2600));
        }
    }
    public void AddNewItem(string itemClass, string itemName)
    {
        _itemName.Add(new ItemsXP(itemClass, itemName, 0, 0, 2300));
    }
    public void SetTotalXP(int xp)
    {
        _totalXp = xp;
    }
    public void SetItemXP(ItemsXP item, ItemsXP itemSaved)
    {
        item.AssignItemXP(itemSaved);
    }
    public void AddTotalXp(int xp)
    {
        _totalXp += xp;
    }
    public void GetXpForClass(string name, int xp)
    {
        foreach (var itemClass in _itemClass)
        {
            if (name == itemClass.NameClass)
            {
                itemClass.AddXp(xp);

                if (itemClass.XP >= itemClass.XPMax)
                {
                    int freeXp = itemClass.XP - itemClass.XPMax;

                    if (itemClass.Level != 100 || itemClass.Level <= 100 )
                    {                        
                        itemClass.AddLvl();
                        itemClass.UpClassXp();
                        itemClass.AddXp(freeXp);

                    }
                    else if (itemClass.Level == 100)
                    {
                        itemClass.MaxLvl();
                    }
                    break;
                }
            }
           
        }
    }

    public void GetXpForItem(string name, int xp)
    {
        foreach (var itemName in _itemName)
        {
            if (name == itemName.NameItem)
            {
                itemName.AddXp(xp);

                if (itemName.XP >= itemName.XPMax)
                {
                    int freeXp = itemName.XP - itemName.XPMax;

                    if (itemName.Level != 100 || itemName.Level <= 100)
                    {
                        itemName.AddLvl();
                        itemName.UpItemXp();
                        itemName.AddXp(freeXp);
                    }
                    else if (itemName.Level == 100)
                    {
                        itemName.MaxLvl();
                    }
                    break;
                }
            }
           
        }
    }

}




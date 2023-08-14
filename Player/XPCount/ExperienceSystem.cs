using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System;

[System.Serializable]
public class ExperienceSystem
{
    private List<InventoryItemData> _items;

    [SerializeField] protected int _totalXp;   
    [SerializeField] protected List<ItemsXP> _itemListClass;
    //[SerializeField] protected List<ItemsXP> _itemListName;    
    public int TotalXp => _totalXp;
    public List<ItemsXP> ItemListClass => _itemListClass;    
    //public List<ItemsXP> ItemListName => _itemListName;

    public static UnityAction<ItemsXP> LvlUp;
    public static UnityAction<ItemsXP> LvlMax;
    public static UnityAction<ItemsXP> TakedXP;
    public ExperienceSystem(int totalXp, List<ItemsXP> itemClass, Database data)
    {
        _totalXp = totalXp;
        _itemListClass = itemClass;
        //_itemListName = itemName;
       
        _items = new List<InventoryItemData>(data.GetItemDatabase());
        
        CreatePlayerSkillTree(_items);
    }

    private void CreatePlayerSkillTree(List<InventoryItemData> data)
    {
        CreateItemClass( data);
        //CreateItemList( data);
    }
    private void CreateItemClass(List<InventoryItemData> data)
    {
        _itemListClass = new List<ItemsXP>();

        foreach (InventoryItemData item in data)
        {
            if (!_itemListClass.Any(i => i.NameClass == item.ItemClass))
            {
                _itemListClass.Add(new ItemsXP(item, 0, 0, 5250, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
            }
        }
    }
    public void AddNewClassItem(InventoryItemData itemData)
    {
        _itemListClass.Add(new ItemsXP(itemData, 0, 0, 5250, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
    }    
    //private void CreateItemList(List<InventoryItemData> data)
    //{
    //    _itemListName = new List<ItemsXP>();

    //    foreach (InventoryItemData item in data)
    //    {
    //        _itemListName.Add(new ItemsXP(item, 0, 0, 2600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
    //    }
    //}
    //public void AddNewItem(InventoryItemData itemID)
    //{
    //    _itemListName.Add(new ItemsXP(itemID, 0, 0, 2600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
    //}
    public void SetTotalXP(int xp)
    {
        _totalXp = xp;
    }  
    public void AddTotalXp(int xp)
    {
        _totalXp += xp;
    }
    public void GetXpForClass(string name, int xp)
    {
        foreach (var itemClass in _itemListClass)
        {
            if (name == itemClass.NameClass)
            {
                itemClass.AddXp(xp);

                while (itemClass.CurrentXP >= itemClass.XPToNextLVL)
                {

                    if (itemClass.Level < 100)
                    {
                        itemClass.AddLvl();

                        LvlUp?.Invoke(itemClass);
                    }
                    else if (itemClass.Level == 100)
                    {
                        itemClass.MaxLvl();
                        break;
                    }
                }
            }          
        }
    }  

    //public void GetXpForItem(string name, int xp)
    //{
    //    foreach (var itemName in _itemListName)
    //    {
    //        if (name == itemName.NameItem)
    //        {
    //            itemName.AddXp(xp);

    //            if (itemName.CurrentXP >= itemName.XPToNextLVL)
    //            {
    //                int freeXp = itemName.CurrentXP - itemName.XPToNextLVL;

    //                if (itemName.Level != 100 || itemName.Level <= 100)
    //                {
    //                    itemName.AddLvl();
    //                    itemName.UpItemXp();
    //                    itemName.AddXp(freeXp);
    //                }
    //                else if (itemName.Level == 100)
    //                {
    //                    itemName.MaxLvl();
    //                }
    //                break;
    //            }
    //        }

    //    }
    //}
    public ItemXPSourse GetBonusStatsClass(string nameClass)
    {
        return _itemListClass.Find(i => i.NameClass == nameClass);
    }
    //public ItemXPSourse GetBonusStatsName(int id)
    //{
    //    return _itemListName.Find(i => i.ID == id);
    //}

}




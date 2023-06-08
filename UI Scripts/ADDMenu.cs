using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ADDMenu : MonoBehaviour
{
    public Database _data;
    public InventoryHolder _inventory;

    private TMPro.TMP_Dropdown _menu;    
    private List<InventoryItemData> _items;

    private void Awake()
    {
        _items = new List<InventoryItemData>(_data.GetItemDatabase());
        _menu = GetComponent<TMP_Dropdown>();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (var item in _items)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(item.DisplayName);
            options.Add(option);
        }
        _menu.AddOptions(options);
        _menu.onValueChanged.AddListener(OnMenuValueChanged);
    }
    private void OnMenuValueChanged(int index)
    {
        AddItem(index);
        
    }

    private void AddItem(int index)
    {
        var inventory = _inventory;
        if (!inventory) return;
       
        if (index >= 0 && index < _items.Count)
        {
            var item = _items[index];
            inventory.Inventory.AddToInventory(item, 1);
        }
    }
}

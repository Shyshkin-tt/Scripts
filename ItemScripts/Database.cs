using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "Inventory System/Item Database")]
public class Database : ScriptableObject
{
    // ������ ���� ��������� � ���� ������
    [SerializeField] private List<InventoryItemData> _itemDatabase;
    [SerializeField] private List<InventoryItemData> _rerourcesDatabase;
    [SerializeField] private List<InventoryItemData> _materialsDatabase;


    [ContextMenu("Set all IDs")]
    public void SetAllIDs()
    {
        _itemDatabase = new List<InventoryItemData>();

        string[] folders = { "ItemData", "Resources", "Materials" };
        foreach (string folder in folders)
        {
            LoadAndSetIDs(folder);
        }
    }

    // ����� ��� ��������� ID ��������� � ���� ������
    [ContextMenu("Set items IDs")]
    public void SetItemIDs()
    {
        _itemDatabase = new List<InventoryItemData>();
        LoadAndSetIDs("ItemData");
    }

    [ContextMenu("Set resources IDs")]
    public void SetResourcesIDs()
    {
        _rerourcesDatabase = new List<InventoryItemData>();
        LoadAndSetIDs("Resi");
    }

    [ContextMenu("Set materials IDs")]
    public void SetMaterialsIDs()
    {
        _materialsDatabase = new List<InventoryItemData>();
        LoadAndSetIDs("Materials");
    }

    private void LoadAndSetIDs(string folderName)
    {
        // �������� ���� ��������� �� ����� "ItemData" � �������� � ���������� �� ID
        var foundItems = Resources.LoadAll<InventoryItemData>(folderName).OrderBy(i => i.ID).ToList();
        // ���������� ���������, � ������� ���� ID � ���������� ��������� � ���������� �� �� ID
        var hasIDInRange = foundItems.Where(i => i.ID != -1 && i.ID < foundItems.Count).OrderBy(i => i.ID).ToList();
        // ���������� ���������, � ������� ���� ID, �� �� ��������� �� ��������� ����������� ��������� � ���������� �� �� ID
        var hasIDNotInRange = foundItems.Where(i => i.ID != -1 && i.ID >= foundItems.Count).OrderBy(i => i.ID).ToList();
        // ���������� ���������, � ������� ��� ID
        var noID = foundItems.Where(i => i.ID <= -1).ToList();
        // ������ ��� ������� �� ������ ��������� ��� ID
        var index = 0;
        // ������ �� ������ ���� ��������� � ���� ������
        for (int i = 0; i < foundItems.Count; i++)
        {
            InventoryItemData itemToAdd;
            // ���� ������� ��� ����� ID, �� ��������� ��� � ������ ��������� ���� ������
            itemToAdd = hasIDInRange.Find(d => d.ID == i);

            if (itemToAdd != null)
            {
                _itemDatabase.Add(itemToAdd); ;
            }
            // ���� ������� �� ����� ID, �� ������������� ��� ����� ID � ��������� � ������ ��������� ���� ������
            else if (index < noID.Count)
            {
                noID[index].ID = i;
                itemToAdd = noID[index];
                index++;
                _itemDatabase.Add(itemToAdd);
            }
        }
        // ��������� ���������� �������� � ID, ������� ��������� �� ��������� ����������� ���������, � ������ ��������� ���� ������
        foreach (var item in hasIDNotInRange)
        {
            _itemDatabase.Add(item);
        }
    }
    // ����� ��� ��������� �������� �� ���� ������ �� ��� ID
    public InventoryItemData GetItem(int id)
    {
        return _itemDatabase.Find(i => i.ID == id);
    }
    public InventoryItemData GetItemClass(string name)
    {
        return _itemDatabase.Find(i => i.DisplayName == name);
    }
    public InventoryItemData GetItemName(string name)
    {
        return _itemDatabase.Find(i => i.DisplayName == name);
    }
    public InventoryItemData GetItemChanse(int chanse)
    {
        return _itemDatabase.Find(i => i.Chance == chanse);
    }

    public List<InventoryItemData> GetItemDatabase()
    {
        return _itemDatabase;
    }

    public InventoryItemData GetItemPrefab(GameObject item)
    {
        return _itemDatabase.Find(i => i.ItemPrefab == item);
    }
}

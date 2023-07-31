using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "Inventory System/Item Database")]
public class Database : ScriptableObject
{
    // Список всех предметов в базе данных
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

    // Метод для установки ID предметов в базе данных
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
        // Загрузка всех предметов из папки "ItemData" в ресурсах и сортировка по ID
        var foundItems = Resources.LoadAll<InventoryItemData>(folderName).OrderBy(i => i.ID).ToList();
        // Фильтрация предметов, у которых есть ID в допустимом диапазоне и сортировка их по ID
        var hasIDInRange = foundItems.Where(i => i.ID != -1 && i.ID < foundItems.Count).OrderBy(i => i.ID).ToList();
        // Фильтрация предметов, у которых есть ID, но он находится за пределами допустимого диапазона и сортировка их по ID
        var hasIDNotInRange = foundItems.Where(i => i.ID != -1 && i.ID >= foundItems.Count).OrderBy(i => i.ID).ToList();
        // Фильтрация предметов, у которых нет ID
        var noID = foundItems.Where(i => i.ID <= -1).ToList();
        // Индекс для прохода по списку предметов без ID
        var index = 0;
        // Проход по списку всех предметов в базе данных
        for (int i = 0; i < foundItems.Count; i++)
        {
            InventoryItemData itemToAdd;
            // Если предмет уже имеет ID, то добавляем его в список предметов базы данных
            itemToAdd = hasIDInRange.Find(d => d.ID == i);

            if (itemToAdd != null)
            {
                _itemDatabase.Add(itemToAdd); ;
            }
            // Если предмет не имеет ID, то устанавливаем ему новый ID и добавляем в список предметов базы данных
            else if (index < noID.Count)
            {
                noID[index].ID = i;
                itemToAdd = noID[index];
                index++;
                _itemDatabase.Add(itemToAdd);
            }
        }
        // Добавляем оставшиеся предметы с ID, которые находятся за пределами допустимого диапазона, в список предметов базы данных
        foreach (var item in hasIDNotInRange)
        {
            _itemDatabase.Add(item);
        }
    }
    // Метод для получения предмета из базы данных по его ID
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

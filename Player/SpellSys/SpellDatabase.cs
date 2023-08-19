using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Database", menuName = "Spell System/Create Spell Database", order = 1)]
public class SpellDatabase : ScriptableObject
{
    [SerializeField] private List<SpellData> _spellDatabase;

    [ContextMenu("Set spell IDs")]
    public void SetItemIDs()
    {
        _spellDatabase = new List<SpellData>();
        LoadAndSetIDs("SpellData");
    }

    private void LoadAndSetIDs(string folderName)
    {
        // Загрузка всех предметов из папки "Data" в ресурсах и сортировка по ID
        var foundItems = Resources.LoadAll<SpellData>(folderName).OrderBy(i => i.ID).ToList();
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
            SpellData itemToAdd;
            // Если предмет уже имеет ID, то добавляем его в список предметов базы данных
            itemToAdd = hasIDInRange.Find(d => d.ID == i);

            if (itemToAdd != null)
            {
                _spellDatabase.Add(itemToAdd); ;
            }
            // Если предмет не имеет ID, то устанавливаем ему новый ID и добавляем в список предметов базы данных
            else if (index < noID.Count)
            {
                noID[index].ID = i;
                itemToAdd = noID[index];
                index++;
                _spellDatabase.Add(itemToAdd);
            }
        }
        // Добавляем оставшиеся предметы с ID, которые находятся за пределами допустимого диапазона, в список предметов базы данных
        foreach (var item in hasIDNotInRange)
        {
            _spellDatabase.Add(item);
        }
    }
    public SpellData GetItem(int id)
    {
        return _spellDatabase.Find(i => i.ID == id);
    }
    public List<SpellData> GetSpellDatabase()
    {
        return _spellDatabase;
    }
}

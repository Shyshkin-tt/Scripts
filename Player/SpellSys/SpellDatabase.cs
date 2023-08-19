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
        // �������� ���� ��������� �� ����� "Data" � �������� � ���������� �� ID
        var foundItems = Resources.LoadAll<SpellData>(folderName).OrderBy(i => i.ID).ToList();
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
            SpellData itemToAdd;
            // ���� ������� ��� ����� ID, �� ��������� ��� � ������ ��������� ���� ������
            itemToAdd = hasIDInRange.Find(d => d.ID == i);

            if (itemToAdd != null)
            {
                _spellDatabase.Add(itemToAdd); ;
            }
            // ���� ������� �� ����� ID, �� ������������� ��� ����� ID � ��������� � ������ ��������� ���� ������
            else if (index < noID.Count)
            {
                noID[index].ID = i;
                itemToAdd = noID[index];
                index++;
                _spellDatabase.Add(itemToAdd);
            }
        }
        // ��������� ���������� �������� � ID, ������� ��������� �� ��������� ����������� ���������, � ������ ��������� ���� ������
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

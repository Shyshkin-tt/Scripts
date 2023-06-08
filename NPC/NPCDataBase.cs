using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "NPC System/NPC Database")]
public class NPCDataBase : ScriptableObject
{
    [SerializeField] private List<NPCData> _npcDatabase;

    [ContextMenu("Set IDs")]
    public void SetNPCID()
    {
        _npcDatabase = new List<NPCData>();

        var foundNPC = Resources.LoadAll<NPCData>("NPC Data").OrderBy(i => i.ID).ToList();

        var hasIDInRange = foundNPC.Where(i => i.ID != -1 && i.ID < foundNPC.Count).OrderBy(i => i.ID).ToList();

        var hasIDNotInRange = foundNPC.Where(i => i.ID != -1 && i.ID >= foundNPC.Count).OrderBy(i => i.ID).ToList();

        var noID = foundNPC.Where(i => i.ID <= -1).ToList();

        var index = 0;

        for (int i = 0; i < foundNPC.Count; i++)
        {
            NPCData NPCToAdd;
            // ���� ������� ��� ����� ID, �� ��������� ��� � ������ ��������� ���� ������
            NPCToAdd = hasIDInRange.Find(d => d.ID == i);

            if (NPCToAdd != null)
            {
                _npcDatabase.Add(NPCToAdd); ;
            }
            // ���� ������� �� ����� ID, �� ������������� ��� ����� ID � ��������� � ������ ��������� ���� ������
            else if (index < noID.Count)
            {
                noID[index].ID = i;
                NPCToAdd = noID[index];
                index++;
                _npcDatabase.Add(NPCToAdd);
            }
        }
        // ��������� ���������� �������� � ID, ������� ��������� �� ��������� ����������� ���������, � ������ ��������� ���� ������
        foreach (var item in hasIDNotInRange)
        {
            _npcDatabase.Add(item);
        }

    }
    public NPCData GetItem(int id)
    {
        return _npcDatabase.Find(i => i.ID == id);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ChestDisplay : InventoryController
{
    [SerializeField] protected InventorySlot_UI _slotPrefab; // ������ UI ����� ���������
    [SerializeField] private TextMeshProUGUI _chestName;
    [SerializeField] private GameObject _itemList;

    protected override void Start()
    {
        base.Start();     // ����� ������ Start �� ������������� ������
    }

    public void RefreshDynamicInventory(InventorySystem invToDisplay)
    {
        ClearSlots(); // ������� ������ ����� ����������� ���������
        inventorySystem = invToDisplay;    // ������������� ������� ��������� ��� �����������   
        // ������������� �� ������� ��������� ����� ���������
        if (inventorySystem != null) inventorySystem.OnInventorySlotChanged += UpdateSlot;
        AssignSlot(invToDisplay); // �������� ����� ���������� �����
    }

    public override void AssignSlot(InventorySystem invToDisplay) // ��������� ����� ��� ����������� � UI
    {
        ClearSlots();

        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (invToDisplay == null) return;

        for (int i = 0; i < invToDisplay.InventorySize; i++)
        {            
            var uiSlot = Instantiate(_slotPrefab, _itemList.transform);// ������� ��������� �����
            slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]); // ��������� ���� � �������
            uiSlot.Init(invToDisplay.InventorySlots[i]); // �������������� ����
            uiSlot.UpdateUISlot(); // ��������� UI ����
        }
    }

    private void ClearSlots()
    {
        foreach (var item in _itemList.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        if (slotDictionary != null) slotDictionary.Clear();
    }

    private void OnDisable() // ������� �� ������� ��������� ����� ��������� ��� ���������� �������
    {
        if (inventorySystem != null) inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }
}
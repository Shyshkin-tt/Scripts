using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ChestDisplay : InventoryController
{
    [SerializeField] protected InventorySlot_UI _slotPrefab; // префаб UI слота инвентаря
    [SerializeField] private TextMeshProUGUI _chestName;
    [SerializeField] private GameObject _itemList;

    protected override void Start()
    {
        base.Start();     // вызов метода Start из родительского класса
    }

    public void RefreshDynamicInventory(InventorySystem invToDisplay)
    {
        ClearSlots(); // Очистка слотов перед обновлением инвентаря
        _inventorySystem = invToDisplay;    // устанавливает систему инвентаря для отображения   
        // подписывается на событие изменения слота инвентаря
        if (_inventorySystem != null) _inventorySystem.OnInventorySlotChanged += UpdateSlot;
        AssignSlot(invToDisplay); // вызывает метод назначения слота
    }

    public override void AssignSlot(InventorySystem invToDisplay) // Назначает слоты для отображения в UI
    {
        ClearSlots();

        inventorySlotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (invToDisplay == null) return;

        for (int i = 0; i < invToDisplay.InventorySize; i++)
        {            
            var uiSlot = Instantiate(_slotPrefab, _itemList.transform);// Создает экземпляр слота
            inventorySlotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]); // Добавляет слот в словарь
            uiSlot.Init(invToDisplay.InventorySlots[i]); // Инициализирует слот
            uiSlot.UpdateUISlot(); // Обновляет UI слот
        }
    }

    private void ClearSlots()
    {
        foreach (var item in _itemList.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        if (inventorySlotDictionary != null) inventorySlotDictionary.Clear();
    }

    private void OnDisable() // Отписка от события изменения слота инвентаря при выключении объекта
    {
        if (_inventorySystem != null) _inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }
}

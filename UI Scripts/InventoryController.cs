using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public abstract class InventoryController : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;

    protected InventorySystem _inventorySystem;
   

    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary; // Соеденение слотов UI инвентаря со слотами инвентаря
    protected Dictionary<InventorySlot_UI, InventorySlot> equipSlotDictionary;
    protected Dictionary<InventorySlot_UI, InventorySlot> beltSlotDictionary;
    
    public InventorySystem InventorySystem => _inventorySystem;
    
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;
    public Dictionary<InventorySlot_UI, InventorySlot> EquiupSlotDictionary => equipSlotDictionary;
    public Dictionary<InventorySlot_UI, InventorySlot> BeltSlotDictionary => beltSlotDictionary;

    protected virtual void Start()
    {
       

    }

    public abstract void AssignSlot(InventorySystem invToDisplay);    
    protected virtual void UpdateSlot(InventorySlot updateSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value == updateSlot)
            {
                slot.Key.UpdateUISlot(updateSlot);             

            }
        }
        foreach (var slot in equipSlotDictionary)
        {
            if (slot.Value == updateSlot)
            {
                slot.Key.UpdateUISlot(updateSlot);

            }
        }
        //foreach (var slot in beltSlotDictionary)
        //{
        //    if (slot.Value == updateSlot)
        //    {
        //        slot.Key.UpdateUISlot(updateSlot);

        //    }
        //}
    }
   
    public void InventorySlotClicked(InventorySlot_UI clickedUISlot)// Метод, который вызывается при клике на слот инвентаря
    {
        
        // Если на курсоре есть предмет и мы кликнули на пустой слот
        if (clickedUISlot.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            // Назначаем предмет на слот инвентаря
            
            if (clickedUISlot.SlotType == SlotType.EquipSlot)
            {
                if (CheckItemTypesMatch(clickedUISlot.EquipSlotType, mouseInventoryItem.AssignedInventorySlot.ItemData.EquipSlotype))
                {
                    clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                    
                    clickedUISlot.UpdateUISlot();
                    _inventorySystem.Holder.EquipOnPlayer(clickedUISlot.AssignedInventorySlot);

                    var bonusStatsClass = _inventorySystem.Holder.Experience.GetBonusStatsClass(clickedUISlot.AssignedInventorySlot.ItemData.ItemClass);

                    _inventorySystem.StatAdd(clickedUISlot.AssignedInventorySlot.ItemData, bonusStatsClass);                    

                    mouseInventoryItem.ClearSlot();                   
                    return;
                }
                else Debug.Log("Cant do this");
            }
            else if(clickedUISlot.SlotType != SlotType.LootSlot)
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();
                
                mouseInventoryItem.ClearSlot();                
                return;
            }
            
            return;
        }

        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else // Если клавиша Shift не нажата или мы не можем разделить стак предметов
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);// Обновляем данные о предмете на курсоре

                if (clickedUISlot.SlotType == SlotType.EquipSlot)
                {
                    var bonusStatsClass = _inventorySystem.Holder.Experience.GetBonusStatsClass(clickedUISlot.AssignedInventorySlot.ItemData.ItemClass);
                    
                    _inventorySystem.StatMinus(clickedUISlot.AssignedInventorySlot.ItemData, bonusStatsClass);

                    _inventorySystem.Holder.RemoveFromPlayer(clickedUISlot.AssignedInventorySlot);
                    clickedUISlot.ClearSlot();
                    InventorySystem.OnEquipSlotChanged?.Invoke();
                    return;
                }
                else
                {                   
                    clickedUISlot.ClearSlot();// Очищаем слот инвентаря                
                    return;
                }              
            }
        }

        // Если выбранный слот UI и слот мыши уже заняты предметами
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            // Проверяем, являются ли предметы одного типа
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;
            // Если предметы одного типа и в слоте UI есть место в стаке, то объединяем стаки предметов

            if (clickedUISlot.SlotType == SlotType.EquipSlot)
            {
                if (CheckItemTypesMatch(clickedUISlot.EquipSlotType, mouseInventoryItem.AssignedInventorySlot.ItemData.EquipSlotype))
                {
                    var oldBonusStatsClass = _inventorySystem.Holder.Experience.GetBonusStatsClass(clickedUISlot.AssignedInventorySlot.ItemData.ItemClass);
                    _inventorySystem.StatMinus(clickedUISlot.AssignedInventorySlot.ItemData, oldBonusStatsClass);
                    

                    _inventorySystem.Holder.RemoveFromPlayer(clickedUISlot.AssignedInventorySlot);
                    SwapSlots(clickedUISlot);

                    var newBonusStatsClass = _inventorySystem.Holder.Experience.GetBonusStatsClass(clickedUISlot.AssignedInventorySlot.ItemData.ItemClass);

                    _inventorySystem.StatAdd(clickedUISlot.AssignedInventorySlot.ItemData, newBonusStatsClass);
                    _inventorySystem.Holder.EquipOnPlayer(clickedUISlot.AssignedInventorySlot);

                    

                    return;
                }
                else Debug.Log("Cant do this");
            }
            else if(clickedUISlot.SlotType == SlotType.AllSlot)
            {
                if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
                {
                    // объединение стаков предметов одного типа
                    clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                    clickedUISlot.UpdateUISlot();
                    mouseInventoryItem.ClearSlot();
                    return;
                }
                // Если предметы одного типа, но в слоте UI места в стаке не хватает, то создаем новый стак и объединяем
                else if (isSameItem && !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize,
                    out int leftInStack))
                {
                    // Если на мыши осталось меньше 1 предмета, то свапаем слоты, иначе создаем новый стак и объединяем
                    if (leftInStack < 1) SwapSlots(clickedUISlot);
                    else
                    {
                        int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;

                        clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);// Добавляем выбранный предмет в слот
                        clickedUISlot.UpdateUISlot();// Обновляем UI слота
                                                     // Создаем новый слот с оставшимся количеством предметов на мыши
                        var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                        mouseInventoryItem.ClearSlot();// Очищаем слот мыши
                        mouseInventoryItem.UpdateMouseSlot(newItem);// Обновляем UI слота мыши с новым слотом
                        return;
                    }
                }
                else if (!isSameItem) // Если предметы разных типов, то меняем их местами
                {
                    SwapSlots(clickedUISlot);
                    return;
                }
            }            
        }
    }        

    private void SwapSlots(InventorySlot_UI inventorySlot_UI) // Функция, которая меняет слоты местами
    {
        // Создаем клон слота мыши
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot(); // Очищаем слот мыши

        mouseInventoryItem.UpdateMouseSlot(inventorySlot_UI.AssignedInventorySlot); // Обновляем UI слота мыши с выбранным слотом

        inventorySlot_UI.ClearSlot(); // Очищаем выбранный слот
        inventorySlot_UI.AssignedInventorySlot.AssignItem(clonedSlot); // Добавляем предмет из клонированного слота мыши в выбранный слот
        inventorySlot_UI.UpdateUISlot(); // Обновляем UI выбранного слота                
    }
  
    public bool CheckItemTypesMatch(EquipmentSlotType slotType, string itemSlotType)
    {
        return (slotType == EquipmentSlotType.Head && itemSlotType == "Head")
               || (slotType == EquipmentSlotType.Armor && itemSlotType == "Armor")
               || (slotType == EquipmentSlotType.Shoes && itemSlotType == "Shoes")
               || (slotType == EquipmentSlotType.RHand && itemSlotType == "RHand")
               || (slotType == EquipmentSlotType.LHand && itemSlotType == "LHand")
               || (slotType == EquipmentSlotType.Bag && itemSlotType == "Bag")
               || (slotType == EquipmentSlotType.BeltSlot && itemSlotType == "BeltSlot");
    }
  
}

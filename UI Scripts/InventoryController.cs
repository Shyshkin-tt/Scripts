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

    [SerializeField] protected UIController _uiController;
    protected CharacterCharacteristics _characterCharacteristics;
    protected InventorySystem _inventorySystem;
    protected ExperienceSystem _experienceSystem;
    protected SpellSystem _spellSystem;

    protected Dictionary<InventorySlot_UI, InventorySlot> inventorySlotDictionary; // ���������� ������ UI ��������� �� ������� ���������
    protected Dictionary<InventorySlot_UI, InventorySlot> equipSlotDictionary;
    protected Dictionary<InventorySlot_UI, InventorySlot> beltSlotDictionary;
    
    public CharacterCharacteristics CharacterCharacteristics => _characterCharacteristics;
    public InventorySystem InventorySystem => _inventorySystem;
    public ExperienceSystem ExperienceSystem => _experienceSystem;
    public SpellSystem SpellSystem => _spellSystem;

    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => inventorySlotDictionary;
    public Dictionary<InventorySlot_UI, InventorySlot> EquiupSlotDictionary => equipSlotDictionary;
    public Dictionary<InventorySlot_UI, InventorySlot> BeltSlotDictionary => beltSlotDictionary;

    protected virtual void Start()
    {
       

    }

    public abstract void AssignSlot(InventorySystem invToDisplay);    
    protected virtual void UpdateSlot(InventorySlot updateSlot)
    {
        foreach (var slot in inventorySlotDictionary)
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
        foreach (var slot in beltSlotDictionary)
        {
            if (slot.Value == updateSlot)
            {
                slot.Key.UpdateUISlot(updateSlot);
            }
        }
    }

    public void InventorySlotClicked(InventorySlot_UI clickedUISlot)// �����, ������� ���������� ��� ����� �� ���� ���������
    {
        
        // ���� �� ������� ���� ������� � �� �������� �� ������ ����
        if (clickedUISlot.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            // ��������� ������� �� ���� ���������
            
            if (clickedUISlot.SlotType == SlotType.EquipSlot)
            {
                if (CheckItemTypesMatch(clickedUISlot.EquipSlotType, mouseInventoryItem.AssignedInventorySlot.ItemData.EquipSlotype))
                {
                    clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                    
                    clickedUISlot.UpdateUISlot();

                    _characterCharacteristics.Holder.EquipOnPlayer(clickedUISlot.AssignedInventorySlot);

                    _spellSystem.SetSpellWhenEquip(clickedUISlot.AssignedInventorySlot.ItemData);
                    _uiController.PlayerActivUI.SetWeaponIcons(clickedUISlot.AssignedInventorySlot.ItemData);

                    var bonusStatsClass = _experienceSystem.GetBonusStatsClass(clickedUISlot.AssignedInventorySlot.ItemData.ItemClass);

                    _characterCharacteristics.StatAdd(clickedUISlot.AssignedInventorySlot.ItemData, bonusStatsClass);                    

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
            else // ���� ������� Shift �� ������ ��� �� �� ����� ��������� ���� ���������
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);// ��������� ������ � �������� �� �������

                if (clickedUISlot.SlotType == SlotType.EquipSlot)
                {
                    var bonusStatsClass = _experienceSystem.GetBonusStatsClass(clickedUISlot.AssignedInventorySlot.ItemData.ItemClass);
                    
                    _characterCharacteristics.StatMinus(clickedUISlot.AssignedInventorySlot.ItemData, bonusStatsClass);

                    _characterCharacteristics.Holder.RemoveFromPlayer(clickedUISlot.AssignedInventorySlot);
                    clickedUISlot.ClearSlot();
                    _spellSystem.ClearSpells();
                    _uiController.PlayerActivUI.ClearItemIcons();
                    InventorySystem.OnEquipSlotChanged?.Invoke();
                    return;
                }
                else
                {                   
                    clickedUISlot.ClearSlot();// ������� ���� ���������                
                    return;
                }              
            }
        }

        // ���� ��������� ���� UI � ���� ���� ��� ������ ����������
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            // ���������, �������� �� �������� ������ ����
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;
            // ���� �������� ������ ���� � � ����� UI ���� ����� � �����, �� ���������� ����� ���������

            if (clickedUISlot.SlotType == SlotType.EquipSlot)
            {
                if (CheckItemTypesMatch(clickedUISlot.EquipSlotType, mouseInventoryItem.AssignedInventorySlot.ItemData.EquipSlotype))
                {
                    var oldBonusStatsClass = _experienceSystem.GetBonusStatsClass(clickedUISlot.AssignedInventorySlot.ItemData.ItemClass);
                    _characterCharacteristics.StatMinus(clickedUISlot.AssignedInventorySlot.ItemData, oldBonusStatsClass);

                    _characterCharacteristics.Holder.RemoveFromPlayer(clickedUISlot.AssignedInventorySlot);
                    _spellSystem.ClearSpells();
                    _uiController.PlayerActivUI.ClearItemIcons();
                    SwapSlots(clickedUISlot);

                    var newBonusStatsClass = _experienceSystem.GetBonusStatsClass(clickedUISlot.AssignedInventorySlot.ItemData.ItemClass);

                    _characterCharacteristics.StatAdd(clickedUISlot.AssignedInventorySlot.ItemData, newBonusStatsClass);
                    _characterCharacteristics.Holder.EquipOnPlayer(clickedUISlot.AssignedInventorySlot);
                    _spellSystem.SetSpellWhenEquip(clickedUISlot.AssignedInventorySlot.ItemData);
                    _uiController.PlayerActivUI.SetWeaponIcons(clickedUISlot.AssignedInventorySlot.ItemData);

                    return;
                }
                else Debug.Log("Cant do this");
            }
            else if(clickedUISlot.SlotType == SlotType.AllSlot)
            {
                if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
                {
                    // ����������� ������ ��������� ������ ����
                    clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                    clickedUISlot.UpdateUISlot();
                    mouseInventoryItem.ClearSlot();
                    return;
                }
                // ���� �������� ������ ����, �� � ����� UI ����� � ����� �� �������, �� ������� ����� ���� � ����������
                else if (isSameItem && !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize,
                    out int leftInStack))
                {
                    // ���� �� ���� �������� ������ 1 ��������, �� ������� �����, ����� ������� ����� ���� � ����������
                    if (leftInStack < 1) SwapSlots(clickedUISlot);
                    else
                    {
                        int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;

                        clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);// ��������� ��������� ������� � ����
                        clickedUISlot.UpdateUISlot();// ��������� UI �����
                                                     // ������� ����� ���� � ���������� ����������� ��������� �� ����
                        var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                        mouseInventoryItem.ClearSlot();// ������� ���� ����
                        mouseInventoryItem.UpdateMouseSlot(newItem);// ��������� UI ����� ���� � ����� ������
                        return;
                    }
                }
                else if (!isSameItem) // ���� �������� ������ �����, �� ������ �� �������
                {
                    SwapSlots(clickedUISlot);
                    return;
                }
            }            
        }
    }        

    private void SwapSlots(InventorySlot_UI inventorySlot_UI) // �������, ������� ������ ����� �������
    {
        // ������� ���� ����� ����
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot(); // ������� ���� ����

        mouseInventoryItem.UpdateMouseSlot(inventorySlot_UI.AssignedInventorySlot); // ��������� UI ����� ���� � ��������� ������

        inventorySlot_UI.ClearSlot(); // ������� ��������� ����
        inventorySlot_UI.AssignedInventorySlot.AssignItem(clonedSlot); // ��������� ������� �� �������������� ����� ���� � ��������� ����
        inventorySlot_UI.UpdateUISlot(); // ��������� UI ���������� �����                
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

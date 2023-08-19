using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using TMPro;


public class InventorySlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    public SlotType _slotType;
    public EquipmentSlotType _equipSlotType;
    [SerializeField] private InventorySlot assignedInventorySlot;
    
    private Button button; // ������ �� ������ � UI
    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryController ParentDisplay { get; private set; }
    public ItemInfo ItemInfo { get; private set; }

    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);
        // ��������� ������ �� ������, ������������ ���������
        ParentDisplay = transform.parent.GetComponentInParent<InventoryController>();
        var ui = transform.GetComponentInParent<UIController>();
        ItemInfo = ui.InfoItem;
    }
    public void Init(InventorySlot slot) // ������������� ������ ���������
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }    
    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.Icon; // ��������� ������� ��������
            itemSprite.color = Color.white;
            // ���� ���������� ��������� � ������ ������ 1, �� ���������� ��� ����������
            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = "";
        }
        else
        {
            ClearSlot();
        }        
    }
    // ���������� UI-��������� ������ ���������
    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null) UpdateUISlot(assignedInventorySlot);
    }
    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";        
    }
    // ��������� ������� �� ������ ���������
    public void OnUISlotClick()
    {
        ParentDisplay?.InventorySlotClicked(this);        
    }
    public SlotType SlotType
    {
        get { return _slotType; }
    }
    public EquipmentSlotType EquipSlotType
    {
        get { return _equipSlotType; }
    }
  
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AssignedInventorySlot.ItemData != null)
        {
            //Debug.Log(this.AssignedInventorySlot.ItemData.DisplayName);
            UIController.OpenItemInfo?.Invoke();
            ItemInfo?.SetItemInfo(this.AssignedInventorySlot.ItemData);
        }
        //else Debug.Log("Slot empty");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse exit InventorySlot_UI");
        UIController.CloseItemInfo?.Invoke();
    }
}
public enum SlotType
{
    AllSlot, EquipSlot, LootSlot
}
public enum EquipmentSlotType
{
    None, Head, Armor, Shoes, RHand, LHand, Bag, Belt, BeltSlot, Neck, Spine
}
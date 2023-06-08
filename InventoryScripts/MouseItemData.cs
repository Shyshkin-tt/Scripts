using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;


[RequireComponent(typeof(CharacterController))]
public class MouseItemData : MonoBehaviour
{
    PlayerController _inputs;

    public Image ItemSprite;  // ����������� ��������, ������� ��������� � ���������
    public TextMeshProUGUI ItemCount;  // ���������� ���������, ������� ��������� � ����� ���������
    public InventorySlot AssignedInventorySlot; // ����������� ����, �� ������� ��������� �������

    private void Awake()
    {
        _inputs = new PlayerController();

        ItemSprite.color = Color.clear;// ��������� ������������ ����������� ��������
        ItemSprite.preserveAspect = true;
        ItemCount.text = "";// ��������� �������� �������� ���������� ��������� � �����
        
    }
    private void OnEnable()// ���������� ����������, ����� ������ �������
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot); // ���������� �������� �� ����������� ����
        ItemSprite.sprite = invSlot.ItemData.Icon; // ��������� ����������� ��������
        ItemCount.text = invSlot.StackSize.ToString(); // ��������� ���������� ���������
        ItemSprite.color = Color.white; // ��������� ����� ����������� �������� �� �����
    }
   
    private void Update()
    {        
        if (AssignedInventorySlot.ItemData != null) // ���� ���� ��������� ������� � ����� ���������
        {            
            transform.position = Mouse.current.position.ReadValue(); // ������� ������ �� ������
            // ���� ������� �� ����� ������ ���� � ������ �� ��������� ��� UI ��������, �� ���� ��������� ���������
            if (_inputs.Player.LMB.ReadValue<float>() > 0 && !IsPointerOverUIObject())
            {               
                ClearSlot();
                // �������� ���� ������������� �������� �������� ���� �� ������������
            }
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
        
    }
    public static bool IsPointerOverUIObject()// ��������, ��������� �� ��������� ���� ��� �������� UI
    {        
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);        
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();        
        List<RaycastResult> results = new List<RaycastResult>();        
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);        
        return results.Count > 0;
    }
}

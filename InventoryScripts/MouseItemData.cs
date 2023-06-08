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

    public Image ItemSprite;  // изображение предмета, который находится в инвентаре
    public TextMeshProUGUI ItemCount;  // количество предметов, которые находятся в слоте инвентаря
    public InventorySlot AssignedInventorySlot; // инвентарный слот, на котором находится предмет

    private void Awake()
    {
        _inputs = new PlayerController();

        ItemSprite.color = Color.clear;// установка прозрачности изображения предмета
        ItemSprite.preserveAspect = true;
        ItemCount.text = "";// установка нулевого значения количества предметов в слоте
        
    }
    private void OnEnable()// активирует управление, когда скрипт включен
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot); // назначение предмета на инвентарный слот
        ItemSprite.sprite = invSlot.ItemData.Icon; // установка изображения предмета
        ItemCount.text = invSlot.StackSize.ToString(); // установка количества предметов
        ItemSprite.color = Color.white; // установка цвета изображения предмета на белый
    }
   
    private void Update()
    {        
        if (AssignedInventorySlot.ItemData != null) // Если есть выбранный предмет в слоте инвентаря
        {            
            transform.position = Mouse.current.position.ReadValue(); // Двигаем объект за мышкой
            // Если нажатие на левую кнопку мыши и курсор не находится над UI объектом, то слот инвентаря очищается
            if (_inputs.Player.LMB.ReadValue<float>() > 0 && !IsPointerOverUIObject())
            {               
                ClearSlot();
                // Дописать окно подтверждения удаления предмета если он выкидывается
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
    public static bool IsPointerOverUIObject()// Проверка, находится ли указатель мыши над объектом UI
    {        
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);        
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();        
        List<RaycastResult> results = new List<RaycastResult>();        
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);        
        return results.Count > 0;
    }
}

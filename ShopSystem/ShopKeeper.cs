using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList _shopItemsHeld;// ������ ���������, ������� �������� � ��������
    //������� ��������, ������� ����� � ������������ ������� �����, ���������� ������� � ������� � ���������, ��������� ��� �������
    [SerializeField] private ShopSystem _shopSystem;
    
    // OnShopWindowRequested - �������, ������� ����������, ����� ����� �������� �� �������
    public static UnityAction<ShopSystem, InventoryHolder> OnShopWindowRequested;

    private string _id;// _id - ���������� ������������� ��������
    // OnInteractionComplete - ��������, ������� ������ ��������� ����� �������������� � ���������
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private void Awake()
    {// ������� ����� ������� �������� �� ������ _shopItemsHeld
        _shopSystem = 
            new ShopSystem(_shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedGold, _shopItemsHeld.BuyMarkUp, _shopItemsHeld.SellMarkUp);
        // ��������� ������ ������� �� _shopItemsHeld � _shopSystem
        foreach (var item in _shopItemsHeld.Items)
        {
            _shopSystem.AddToShop(item.ItemData, item.Amount);
        }

        _id = GetComponent<UniqueID>().ID;// �������� ���������� ������������� ��� ��������
       
    }

 
 
    public void Interact(ActionController interactor, out bool interactSuccessful)
    {
        var playerInv = interactor.GetComponent<InventoryHolder>();

        if (playerInv != null)
        {
            OnShopWindowRequested?.Invoke(_shopSystem, playerInv);           
            interactSuccessful = true;
        }
        else
        {
            interactSuccessful = false;
        }
    }

    public void EndInteraction()
    {

    }

}

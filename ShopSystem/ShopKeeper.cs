using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList _shopItemsHeld;// список предметов, которые хранятся в магазине
    //система магазина, которая знает о максимальной золотой кассе, затемнении покупки и продажи и предметах, доступных для продажи
    [SerializeField] private ShopSystem _shopSystem;
    
    // OnShopWindowRequested - событие, которое происходит, когда игрок нажимает на магазин
    public static UnityAction<ShopSystem, InventoryHolder> OnShopWindowRequested;

    private string _id;// _id - уникальный идентификатор магазина
    // OnInteractionComplete - действие, которое должно произойти после взаимодействия с магазином
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private void Awake()
    {// создаем новую систему магазина на основе _shopItemsHeld
        _shopSystem = 
            new ShopSystem(_shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedGold, _shopItemsHeld.BuyMarkUp, _shopItemsHeld.SellMarkUp);
        // добавляем каждый предмет из _shopItemsHeld в _shopSystem
        foreach (var item in _shopItemsHeld.Items)
        {
            _shopSystem.AddToShop(item.ItemData, item.Amount);
        }

        _id = GetComponent<UniqueID>().ID;// получаем уникальный идентификатор для магазина
       
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

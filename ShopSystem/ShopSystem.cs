using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> _shopInventory;// Ћист, хран€щий слоты магазина
    [SerializeField] private int _availableGold;// ƒоступное золото у магазина
    [SerializeField] private float _buyMarkUp;// Ќаценка на товары при покупке
    [SerializeField] private float _sellMarkUP;// Ќаценка на товары при продаже

    public List<ShopSlot> ShopInventory => _shopInventory;// ¬озвращает список слотов магазина
    public int AvailableGold => _availableGold;// ¬озвращает количество доступного золота у магазина
    public float BuyMarkUp => _buyMarkUp;// ¬озвращает наценку на товары при покупке
    public float SellMarkUp => _sellMarkUP;// ¬озвращает наценку на товары при продаже

    //  онструктор магазина, принимающий размер, количество золота, наценку при покупке и наценку при продаже
    public ShopSystem(int size, int gold, float buyMarkUp, float sellMarkUp)
    {
        _availableGold = gold;// ѕрисваивает доступное золото у магазина значение из параметра конструктора
        _buyMarkUp = buyMarkUp;
        _sellMarkUP = sellMarkUp;

        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {
        _shopInventory = new List<ShopSlot>(size);
        // —оздание новых магазинных слотов и добавление их в список
        for (int i = 0; i < size; i++)
        {
            _shopInventory.Add(new ShopSlot());
        }
    }

    public void AddToShop(InventoryItemData data, int amount)
    {
        // ≈сли товар уже присутствует в магазине, добавл€ем к его стаку новое количество
        if (ContainsItem(data, out ShopSlot shopSlot))
        {
            shopSlot.AddToStack(amount);
            return;
        }
        // ≈сли нет свободных магазинных слотов, создаем новый слот и добавл€ем в список
        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);
    }

    private ShopSlot GetFreeSlot()
    {// »спользуем метод FirstOrDefault дл€ нахождени€ первого пустого слота в списке
        var freeSlot = _shopInventory.FirstOrDefault(i => i.ItemData == null);
        // ≈сли пустой слот не найден, создаем новый и добавл€ем в список
        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            _shopInventory.Add(freeSlot);
        }
        return freeSlot;
    }
    // ћетод, провер€ющий наличие товара в магазине
    public bool ContainsItem(InventoryItemData itemToAdd, out ShopSlot shopSlot)
    {
        shopSlot = _shopInventory.Find(i => i.ItemData == itemToAdd);
        return shopSlot != null;
    }

    public void PurchaseItem(InventoryItemData data, int amount)// ѕокупка товара
    {
        if (!ContainsItem(data, out ShopSlot slot)) return;

        slot.RemoveFromStack(amount);
    }

    public void GainGold(int basketTotal)// ƒобавление золота после покупки товара
    {
        _availableGold += basketTotal;
    }

    public void SellItem(InventoryItemData kvpKey, int kvpValue, int price)// ѕродажа товара
    {
        AddToShop(kvpKey, kvpValue);
        ReduceGold(price);
    }

    private void ReduceGold(int price)// ”меньшение доступного золота после продажи товара
    {
        _availableGold -= price;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Shop System/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> _items;// список товаров в магазине
    [SerializeField] private int _maxAllowedGold;// максимально возможное золото, которое может иметь магазин
    [SerializeField] private float _sellMarkUp;// наценка на цену продажи товаров
    [SerializeField] private float _buyMarkUp;// наценка на цену покупки товаров

    public List<ShopInventoryItem> Items => _items;// получение списка товаров из класса
    public int MaxAllowedGold => _maxAllowedGold;// получение максимально возможного количества золота из класса
    public float SellMarkUp => _sellMarkUp;// получение наценки на цену продажи товаров из класса
    public float BuyMarkUp => _buyMarkUp;// получение наценки на цену покупки товаров из класса
}

[System.Serializable]
public struct ShopInventoryItem
{
    public InventoryItemData ItemData;// данные товара
    public int Amount;// количество товара
}
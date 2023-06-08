using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;// спрайт товара
    [SerializeField] private TextMeshProUGUI _itemName;// наименование товара
    [SerializeField] private TextMeshProUGUI _itemCount;// количество товара
    [SerializeField] private ShopSlot _assignedItemSlot;// назначенный слот товара

    public ShopSlot AssignedItemSlot => _assignedItemSlot;// Возвращает текущий слот, к которому назначен предмет

    [SerializeField] private Button _addItemToCartButton;// кнопка добавления в корзину
    [SerializeField] private Button _removeItemFromCartButton;// кнопка удаления из корзины

    private int _tempAmount;// временное количество товара

    public ShopKeeperDisplay ParentDisplay { get; private set; }// отображение магазина
    public float MarkUp { get; private set; }// наценка
    private void Awake()
    {
        // Сбрасывает изображение предмета, его количество, название, цвет на "прозрачный"
        _itemSprite.sprite = null;
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;
        _itemName.text = "";
        _itemCount.text = "";

        // подписываемся на событие добавления товара в корзину
        _addItemToCartButton?.onClick.AddListener(AddItemToCart);
        // подписываемся на событие удаления товара из корзины
        _removeItemFromCartButton?.onClick.AddListener(RemoveItemToCart);
        // получаем родительский объект - отображение магазина
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }
    public void Init(ShopSlot slot, float markUP)
    {
        _assignedItemSlot = slot;// Присваивание значения слоту, отображающему объект в магазине
        MarkUp = markUP;// Установка цены на объект в магазине
        _tempAmount = slot.StackSize;// Установка временной переменной количества объектов на основе значения слота
        UpdateUISlot();// Обновление отображения слота на экране
    }
    private void UpdateUISlot()
    {
        if (_assignedItemSlot.ItemData != null)// Если слот не пустой
        {
            _itemSprite.sprite = _assignedItemSlot.ItemData.Icon;// устанавливаем спрайт товара
            _itemSprite.color = Color.white;
            _itemCount.text = _assignedItemSlot.StackSize.ToString();// устанавливаем количество товара
            var modifiedPrice = ShopKeeperDisplay.GetModifiedPrice(_assignedItemSlot.ItemData, 1, MarkUp);// получаем цену товара с учетом наценки

            _itemName.text = $"{_assignedItemSlot.ItemData.DisplayName} - {modifiedPrice}G";// устанавливаем название товара и его цену
        }
        else// если слот пустой
        {
            _itemSprite.sprite = null;// Удаление изображения объекта со спрайта
            _itemSprite.color = Color.clear;// Установка цвета спрайта в прозрачный
            _itemName.text = "";// Удаление названия объекта с экрана
            _itemCount.text = "";// Удаление отображения количества объектов с экрана
        }
    }
    private void RemoveItemToCart()// удаляем товар из корзины
    {
        // Если количество объектов в корзине равно количеству объектов в слоте - выход
        if (_tempAmount == _assignedItemSlot.StackSize) return;

        _tempAmount++;// увеличиваем количество временного товара
        ParentDisplay.RemoveItemFromCart(this);// Удаление объекта из корзины в магазине
        _itemCount.text = _tempAmount.ToString();// Обновление отображения количества объектов на экране
    }
    private void AddItemToCart()// Добавление объекта в корзину в магазине
{
        if (_tempAmount <= 0) return;// Если количество объектов в корзине меньше или равно 0 - выход

        _tempAmount--;// Уменьшение временной переменной количества объектов
        ParentDisplay.AddItemToCart(this);// Добавление объекта в корзину в магазине
        _itemCount.text = _tempAmount.ToString(); // Обновление отображения количества объектов на экране
    }   
}

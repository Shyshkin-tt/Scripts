using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperDisplay : MonoBehaviour
{
    // Объект префаба ячейки магазина
    [SerializeField] private ShopSlotUI _shopSlotPrefab;
    [SerializeField] private ShoppingCartItemUI _shoppingCartItemPrefab;
    // Объекты отображения содержимого корзины покупок
    [SerializeField] private Button _buyTab;
    [SerializeField] private Button _sellTab;
    // Объекты отображения информации о предмете
    [Header("Shoppin Cart")]
    [SerializeField] private TextMeshProUGUI _basketTotalText;
    [SerializeField] private TextMeshProUGUI _playerGoldText;
    [SerializeField] private TextMeshProUGUI _shopGoldText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyButtonText;
    // Объекты отображения информации о предмете
    [Header("Item Preview Section")]
    [SerializeField] private Image _itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI _itemPriviewName;
    [SerializeField] private TextMeshProUGUI _itemPriviewDiscription;
    // Панели контента, на которые будут помещаться элементы магазина и товары в корзине
    [SerializeField] private GameObject _itemListContentPanel;
    [SerializeField] private GameObject _shoppingCartContentPanel;

    private int _basketTotal;// Общая стоимость товаров в корзине

    private bool _isSelling;// Флаг, указывающий на режим продажи или покупки предметов
    // Ссылки на систему магазина и инвентарь игрока
    private ShopSystem _shopSystem;
    private InventoryHolder _playerInventoryHolder;
    // Словарь содержимого корзины покупок (предмет-количество)
    private Dictionary<InventoryItemData, int> _shoppingCart = new Dictionary<InventoryItemData, int>();
    // Словарь отображения элементов UI корзины покупок (предмет-объект UI)
    private Dictionary<InventoryItemData, ShoppingCartItemUI> _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();

    // Отображает окно магазина и сохраняет ссылки на систему магазина и инвентарь игрока
    public void DisplayShopWindow(ShopSystem shopSystem, InventoryHolder playerInventoryHolder)
    {
        _shopSystem = shopSystem;// shopSystem - объект класса магазина, содержащий предметы магазина и доступное золото в магазине.
        // playerInventoryHolder - объект класса игрового инвентаря игрока, содержащий предметы игрока и доступное золото игрока.
        _playerInventoryHolder = playerInventoryHolder;

        RefreshDisplay();
    }
    public void RefreshDisplay()// Обновляет отображение окна магазина
    {
        if (_buyButton != null)// проверяем, существует ли объект _buyButton
        {
            // меняем текст кнопки на "Sell Items" если _isSelling = true, и на "Buy Items" если _isSelling = false
            _buyButtonText.text = _isSelling ? "Sell Items" : "Buy Items";
            _buyButton.onClick.RemoveAllListeners();// удаляем все обработчики нажатия на кнопку
            // если _isSelling = true, добавляем обработчик нажатия на кнопку SellItems
            if (_isSelling) _buyButton.onClick.AddListener(SellItems);
            // если _isSelling = false, добавляем обработчик нажатия на кнопку BuyItems
            else _buyButton.onClick.AddListener(BuyItems);
        }

        ClearSlots();
        ClearItemPreview();

        _basketTotalText.enabled = false;// скрываем текст "Total"
        _buyButton.gameObject.SetActive(false);// скрываем кнопку "Buy"/"Sell"
        _basketTotal = 0;// обнуляем общую стоимость товаров в корзине
        // обновляем текст "Player Gold" с текущим количеством золота у игрока
        //_playerGoldText.text = $"Coins: {_playerInventoryHolder.CharacterStats.Coines}";
        // обновляем текст "Shop Gold" с текущим количеством золота у магазина
        _shopGoldText.text = $"Shop Gold: {_shopSystem.AvailableGold}";

        if (_isSelling) DisplayPlayerInventory();// если _isSelling = true, отображаем инвентарь игрока
        else DisplayShopInventory();// если _isSelling = false, отображаем инвентарь магазина
    }

    private void BuyItems()
    {
        // если у игрока недостаточно золота, выходим из функции
        //if (_playerInventoryHolder.CharacterStats.Coines < _basketTotal) return;
        // если в инвентаре игрока не хватает места для всех товаров в корзине, выходим из функции
        if (!_playerInventoryHolder.Inventory.CheckInventoryRemaining(_shoppingCart)) return;

        foreach (var kvp in _shoppingCart)// проходимся по всем товарам в корзине
        {
            _shopSystem.PurchaseItem(kvp.Key, kvp.Value);// покупаем товар у магазина

            for (int i = 0; i < kvp.Value; i++)// добавляем товар в инвентарь игрока
            {
                _playerInventoryHolder.Inventory.AddToInventory(kvp.Key, 1);
            }
        }

        _shopSystem.GainGold(_basketTotal);// магазин получает золото от продажи
        //_playerInventoryHolder.CharacterStats.SpendGold(_basketTotal);// игрок тратит золото на покупку

        RefreshDisplay();
    }

    private void SellItems()
    {
        // Проверка, есть ли достаточно золота у магазина для покупки товаров из корзины
        if (_shopSystem.AvailableGold < _basketTotal) return;
        // Проходит по товарам в корзине, продает их и переносит деньги в инвентарь игрока
        foreach (var kvp in _shoppingCart)
        {
            var price = GetModifiedPrice(kvp.Key, kvp.Value, _shopSystem.SellMarkUp);// Получаем модифицированную цену товара

            _shopSystem.SellItem(kvp.Key, kvp.Value, price);// Продаем товар

            //_playerInventoryHolder.CharacterStats.GainGold(price);// Переносим деньги в инвентарь игрока
            // Удаляем товар из инвентаря игрока
            _playerInventoryHolder.Inventory.RemoveItemsFromInventory(kvp.Key, kvp.Value);           
        }
        RefreshDisplay();
    }

    private void ClearSlots()
    {
        // Очищает слоты с товарами
        _shoppingCart = new Dictionary<InventoryItemData, int>();
        _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();
        // Удаляет элементы в списке с товарами
        foreach (var item in _itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
        // Удаляет элементы в списке корзины
        foreach (var item in _shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    private void DisplayShopInventory()
    {
        foreach (var item in _shopSystem.ShopInventory)// Отображает товары в магазине
        {
            if (item.ItemData == null) continue;
            // Создает слот и инициализирует его
            var shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Init(item, _shopSystem.BuyMarkUp);
        }
    }

    private void DisplayPlayerInventory()
    {
        // Отображает товары в инвентаре игрока
        foreach (var item in _playerInventoryHolder.Inventory.GettAllItemsHeld())
        {
            // Создает временный слот и присваивает ему значения из инвентаря игрока
            var tempSlot = new ShopSlot();
            tempSlot.AssignItem(item.Key, item.Value);
            // Создает слот и инициализирует его
            var shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Init(tempSlot, _shopSystem.SellMarkUp);
        }
    }

    public void RemoveItemFromCart(ShopSlotUI shopSlotUI)
    {
        // Получаем данные о товаре из слота магазина, выбранного пользователем
        var data = shopSlotUI.AssignedItemSlot.ItemData;
        // Получаем цену товара с учетом наценки магазина
        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);
        
        if (_shoppingCart.ContainsKey(data))// Если выбранный товар уже был добавлен в корзину
        {
            _shoppingCart[data]--;// Уменьшаем количество выбранного товара в корзине            
            var newString = $"{data.DisplayName} ({price}G) x {_shoppingCart[data]}";// Обновляем текст элемента в корзине
            _shoppingCartUI[data].SetItemText(newString); // Обновляем строку описания товара в списке корзины

            // Если количество товара в корзине стало равно 0, удаляем его из корзины
            if (_shoppingCart[data] <= 0)
            {
                _shoppingCart.Remove(data);// Удаляем товар из словаря корзины
                // Получаем ссылку на объект, отображающий товар в списке корзины, который нужно удалить
                var tempObj = _shoppingCartUI[data].gameObject;
                _shoppingCartUI.Remove(data);// Удаляем объект из списка отображаемых товаров в корзине
                Destroy(tempObj);// Удаляем объект из сцены
            }
        }
        // Обновляем информацию об общей стоимости товаров в корзине
        _basketTotal -= price;
        _basketTotalText.text = $"Total: {_basketTotal}G";
        // Если общая стоимость корзины стала меньше или равна 0 и текст еще активен, скрываем его и кнопку покупки
        if (_basketTotal <= 0 && _basketTotalText.IsActive())
        {
            _basketTotalText.enabled = false;
            _buyButton.gameObject.SetActive(false);
            ClearItemPreview();
            return;
        }
        // Проверяем, не превышает ли общая стоимость корзины доступное количество золота игрока
        CheckCartVsAvailableGold();
    }

    private void ClearItemPreview()// Очищаем область предварительного просмотра товара
    {
        _itemPreviewSprite.sprite = null;
        _itemPreviewSprite.color = Color.clear;
        _itemPriviewName.text = "";
        _itemPriviewDiscription.text = "";
    }

    public void AddItemToCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.ItemData;// Получаем данные о товаре из слота магазина, выбранного пользователем

        UpdateItemPreview(shopSlotUI); // Обновляем область предварительного просмотра товара

        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);// Получаем цену товара с учетом наценки магазина 

        if (_shoppingCart.ContainsKey(data))// если предмет уже есть в корзине, увеличиваем количество
        {
            _shoppingCart[data]++;// Увеличиваем количество предметов в корзине
            // Обновление текста для отображения количества выбранного предмета в UI-элементе корзины
            var newString = $"{data.DisplayName} ({price}G) x {_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);            
        }
        else// если предмета еще нет в корзине, добавляем его
        {
            _shoppingCart.Add(data, 1);
            // Создание нового текстового объекта для отображения выбранного предмета в UI-элементе корзины
            var shoppingCartTextObj = Instantiate(_shoppingCartItemPrefab, _shoppingCartContentPanel.transform);
            var newString = $"{data.DisplayName} ({price}G) x1";// Обновляем текст на предмете в корзине
            shoppingCartTextObj.SetItemText(newString);
            _shoppingCartUI.Add(data, shoppingCartTextObj);// Добавляем предмет и его отображение в словарь
        }

        _basketTotal += price;// Обновление общей суммы покупок в UI
        _basketTotalText.text = $"Total: {_basketTotal}G";// Обновляем текст с общей стоимостью корзины

        // Если общая сумма покупок больше 0 и элементы для отображения не активны, то включить их
        if (_basketTotal > 0 && !_basketTotalText.IsActive())
        {
            _basketTotalText.enabled = true;
            _buyButton.gameObject.SetActive(true);
        }
        // Проверка, возможно ли купить предметы в корзине при имеющейся у игрока сумме золота
        CheckCartVsAvailableGold();
    }    

    private void CheckCartVsAvailableGold()// Метод для проверки возможности покупки предметов в корзине при имеющейся у игрока сумме золота
    {
        // Получение доступного у игрока золота
        //var goldToCheck = _isSelling ? _shopSystem.AvailableGold : _playerInventoryHolder.CharacterStats.Coines;
        // Изменение цвета текста общей суммы покупок в зависимости от того, достаточно ли у игрока золота для покупки предметов
        //_basketTotalText.color = _basketTotal > goldToCheck ? Color.red : Color.white;

        // Если переключились на вкладку "продать" или в инвентаре достаточно места, то выходим из метода
        if (_isSelling || _playerInventoryHolder.Inventory.CheckInventoryRemaining(_shoppingCart)) return;

        // Если нет места в инвентаре, то изменяем текст корзины на сообщение об ошибке и меняем его цвет на красный
        _basketTotalText.text = "Not enoght room in inventory";
        _basketTotalText.color = Color.red;
    }

    public static int GetModifiedPrice(InventoryItemData data, int amount, float markUp)// Вычисляет цену, учитывая количество и наценку.
    {
        var baseValue = data.GoldValue * amount;// Рассчитываем базовую стоимость, учитывая количество товара.
        return Mathf.FloorToInt(baseValue + baseValue * markUp);// Рассчитываем итоговую цену с учетом наценки.
    }

    private void UpdateItemPreview(ShopSlotUI shopSlotUI)// Обновляет предварительный просмотр товара в магазине.
    {
        var data = shopSlotUI.AssignedItemSlot.ItemData;// Получаем данные о товаре.

        _itemPreviewSprite.sprite = data.Icon;// Обновляем изображение предварительного просмотра товара.
        _itemPreviewSprite.color = Color.white;
        _itemPriviewName.text = data.DisplayName;// Обновляем название предварительного просмотра товара.
        _itemPriviewDiscription.text = data.Description;// Обновляем описание предварительного просмотра товара.
    }
    public void OnBuyTabPressed()// Обработчик события нажатия на вкладку "купить".
    {
        _isSelling = false;// Устанавливаем флаг "продажа" в значение false.
        RefreshDisplay();// Обновляем отображение магазина.
    }
    public void OnSellTabPressed()// Обработчик события нажатия на вкладку "продать".
    {
        _isSelling = true;// Устанавливаем флаг "продажа" в значение true.
        RefreshDisplay();// Обновляем отображение магазина.
    }   
}

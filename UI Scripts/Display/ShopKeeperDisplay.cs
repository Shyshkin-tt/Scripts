using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperDisplay : MonoBehaviour
{
    // ������ ������� ������ ��������
    [SerializeField] private ShopSlotUI _shopSlotPrefab;
    [SerializeField] private ShoppingCartItemUI _shoppingCartItemPrefab;
    // ������� ����������� ����������� ������� �������
    [SerializeField] private Button _buyTab;
    [SerializeField] private Button _sellTab;
    // ������� ����������� ���������� � ��������
    [Header("Shoppin Cart")]
    [SerializeField] private TextMeshProUGUI _basketTotalText;
    [SerializeField] private TextMeshProUGUI _playerGoldText;
    [SerializeField] private TextMeshProUGUI _shopGoldText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyButtonText;
    // ������� ����������� ���������� � ��������
    [Header("Item Preview Section")]
    [SerializeField] private Image _itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI _itemPriviewName;
    [SerializeField] private TextMeshProUGUI _itemPriviewDiscription;
    // ������ ��������, �� ������� ����� ���������� �������� �������� � ������ � �������
    [SerializeField] private GameObject _itemListContentPanel;
    [SerializeField] private GameObject _shoppingCartContentPanel;

    private int _basketTotal;// ����� ��������� ������� � �������

    private bool _isSelling;// ����, ����������� �� ����� ������� ��� ������� ���������
    // ������ �� ������� �������� � ��������� ������
    private ShopSystem _shopSystem;
    private InventoryHolder _playerInventoryHolder;
    // ������� ����������� ������� ������� (�������-����������)
    private Dictionary<InventoryItemData, int> _shoppingCart = new Dictionary<InventoryItemData, int>();
    // ������� ����������� ��������� UI ������� ������� (�������-������ UI)
    private Dictionary<InventoryItemData, ShoppingCartItemUI> _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();

    // ���������� ���� �������� � ��������� ������ �� ������� �������� � ��������� ������
    public void DisplayShopWindow(ShopSystem shopSystem, InventoryHolder playerInventoryHolder)
    {
        _shopSystem = shopSystem;// shopSystem - ������ ������ ��������, ���������� �������� �������� � ��������� ������ � ��������.
        // playerInventoryHolder - ������ ������ �������� ��������� ������, ���������� �������� ������ � ��������� ������ ������.
        _playerInventoryHolder = playerInventoryHolder;

        RefreshDisplay();
    }
    public void RefreshDisplay()// ��������� ����������� ���� ��������
    {
        if (_buyButton != null)// ���������, ���������� �� ������ _buyButton
        {
            // ������ ����� ������ �� "Sell Items" ���� _isSelling = true, � �� "Buy Items" ���� _isSelling = false
            _buyButtonText.text = _isSelling ? "Sell Items" : "Buy Items";
            _buyButton.onClick.RemoveAllListeners();// ������� ��� ����������� ������� �� ������
            // ���� _isSelling = true, ��������� ���������� ������� �� ������ SellItems
            if (_isSelling) _buyButton.onClick.AddListener(SellItems);
            // ���� _isSelling = false, ��������� ���������� ������� �� ������ BuyItems
            else _buyButton.onClick.AddListener(BuyItems);
        }

        ClearSlots();
        ClearItemPreview();

        _basketTotalText.enabled = false;// �������� ����� "Total"
        _buyButton.gameObject.SetActive(false);// �������� ������ "Buy"/"Sell"
        _basketTotal = 0;// �������� ����� ��������� ������� � �������
        // ��������� ����� "Player Gold" � ������� ����������� ������ � ������
        //_playerGoldText.text = $"Coins: {_playerInventoryHolder.CharacterStats.Coines}";
        // ��������� ����� "Shop Gold" � ������� ����������� ������ � ��������
        _shopGoldText.text = $"Shop Gold: {_shopSystem.AvailableGold}";

        if (_isSelling) DisplayPlayerInventory();// ���� _isSelling = true, ���������� ��������� ������
        else DisplayShopInventory();// ���� _isSelling = false, ���������� ��������� ��������
    }

    private void BuyItems()
    {
        // ���� � ������ ������������ ������, ������� �� �������
        //if (_playerInventoryHolder.CharacterStats.Coines < _basketTotal) return;
        // ���� � ��������� ������ �� ������� ����� ��� ���� ������� � �������, ������� �� �������
        if (!_playerInventoryHolder.Inventory.CheckInventoryRemaining(_shoppingCart)) return;

        foreach (var kvp in _shoppingCart)// ���������� �� ���� ������� � �������
        {
            _shopSystem.PurchaseItem(kvp.Key, kvp.Value);// �������� ����� � ��������

            for (int i = 0; i < kvp.Value; i++)// ��������� ����� � ��������� ������
            {
                _playerInventoryHolder.Inventory.AddToInventory(kvp.Key, 1);
            }
        }

        _shopSystem.GainGold(_basketTotal);// ������� �������� ������ �� �������
        //_playerInventoryHolder.CharacterStats.SpendGold(_basketTotal);// ����� ������ ������ �� �������

        RefreshDisplay();
    }

    private void SellItems()
    {
        // ��������, ���� �� ���������� ������ � �������� ��� ������� ������� �� �������
        if (_shopSystem.AvailableGold < _basketTotal) return;
        // �������� �� ������� � �������, ������� �� � ��������� ������ � ��������� ������
        foreach (var kvp in _shoppingCart)
        {
            var price = GetModifiedPrice(kvp.Key, kvp.Value, _shopSystem.SellMarkUp);// �������� ���������������� ���� ������

            _shopSystem.SellItem(kvp.Key, kvp.Value, price);// ������� �����

            //_playerInventoryHolder.CharacterStats.GainGold(price);// ��������� ������ � ��������� ������
            // ������� ����� �� ��������� ������
            _playerInventoryHolder.Inventory.RemoveItemsFromInventory(kvp.Key, kvp.Value);           
        }
        RefreshDisplay();
    }

    private void ClearSlots()
    {
        // ������� ����� � ��������
        _shoppingCart = new Dictionary<InventoryItemData, int>();
        _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();
        // ������� �������� � ������ � ��������
        foreach (var item in _itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
        // ������� �������� � ������ �������
        foreach (var item in _shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    private void DisplayShopInventory()
    {
        foreach (var item in _shopSystem.ShopInventory)// ���������� ������ � ��������
        {
            if (item.ItemData == null) continue;
            // ������� ���� � �������������� ���
            var shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Init(item, _shopSystem.BuyMarkUp);
        }
    }

    private void DisplayPlayerInventory()
    {
        // ���������� ������ � ��������� ������
        foreach (var item in _playerInventoryHolder.Inventory.GettAllItemsHeld())
        {
            // ������� ��������� ���� � ����������� ��� �������� �� ��������� ������
            var tempSlot = new ShopSlot();
            tempSlot.AssignItem(item.Key, item.Value);
            // ������� ���� � �������������� ���
            var shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Init(tempSlot, _shopSystem.SellMarkUp);
        }
    }

    public void RemoveItemFromCart(ShopSlotUI shopSlotUI)
    {
        // �������� ������ � ������ �� ����� ��������, ���������� �������������
        var data = shopSlotUI.AssignedItemSlot.ItemData;
        // �������� ���� ������ � ������ ������� ��������
        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);
        
        if (_shoppingCart.ContainsKey(data))// ���� ��������� ����� ��� ��� �������� � �������
        {
            _shoppingCart[data]--;// ��������� ���������� ���������� ������ � �������            
            var newString = $"{data.DisplayName} ({price}G) x {_shoppingCart[data]}";// ��������� ����� �������� � �������
            _shoppingCartUI[data].SetItemText(newString); // ��������� ������ �������� ������ � ������ �������

            // ���� ���������� ������ � ������� ����� ����� 0, ������� ��� �� �������
            if (_shoppingCart[data] <= 0)
            {
                _shoppingCart.Remove(data);// ������� ����� �� ������� �������
                // �������� ������ �� ������, ������������ ����� � ������ �������, ������� ����� �������
                var tempObj = _shoppingCartUI[data].gameObject;
                _shoppingCartUI.Remove(data);// ������� ������ �� ������ ������������ ������� � �������
                Destroy(tempObj);// ������� ������ �� �����
            }
        }
        // ��������� ���������� �� ����� ��������� ������� � �������
        _basketTotal -= price;
        _basketTotalText.text = $"Total: {_basketTotal}G";
        // ���� ����� ��������� ������� ����� ������ ��� ����� 0 � ����� ��� �������, �������� ��� � ������ �������
        if (_basketTotal <= 0 && _basketTotalText.IsActive())
        {
            _basketTotalText.enabled = false;
            _buyButton.gameObject.SetActive(false);
            ClearItemPreview();
            return;
        }
        // ���������, �� ��������� �� ����� ��������� ������� ��������� ���������� ������ ������
        CheckCartVsAvailableGold();
    }

    private void ClearItemPreview()// ������� ������� ���������������� ��������� ������
    {
        _itemPreviewSprite.sprite = null;
        _itemPreviewSprite.color = Color.clear;
        _itemPriviewName.text = "";
        _itemPriviewDiscription.text = "";
    }

    public void AddItemToCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.ItemData;// �������� ������ � ������ �� ����� ��������, ���������� �������������

        UpdateItemPreview(shopSlotUI); // ��������� ������� ���������������� ��������� ������

        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);// �������� ���� ������ � ������ ������� �������� 

        if (_shoppingCart.ContainsKey(data))// ���� ������� ��� ���� � �������, ����������� ����������
        {
            _shoppingCart[data]++;// ����������� ���������� ��������� � �������
            // ���������� ������ ��� ����������� ���������� ���������� �������� � UI-�������� �������
            var newString = $"{data.DisplayName} ({price}G) x {_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);            
        }
        else// ���� �������� ��� ��� � �������, ��������� ���
        {
            _shoppingCart.Add(data, 1);
            // �������� ������ ���������� ������� ��� ����������� ���������� �������� � UI-�������� �������
            var shoppingCartTextObj = Instantiate(_shoppingCartItemPrefab, _shoppingCartContentPanel.transform);
            var newString = $"{data.DisplayName} ({price}G) x1";// ��������� ����� �� �������� � �������
            shoppingCartTextObj.SetItemText(newString);
            _shoppingCartUI.Add(data, shoppingCartTextObj);// ��������� ������� � ��� ����������� � �������
        }

        _basketTotal += price;// ���������� ����� ����� ������� � UI
        _basketTotalText.text = $"Total: {_basketTotal}G";// ��������� ����� � ����� ���������� �������

        // ���� ����� ����� ������� ������ 0 � �������� ��� ����������� �� �������, �� �������� ��
        if (_basketTotal > 0 && !_basketTotalText.IsActive())
        {
            _basketTotalText.enabled = true;
            _buyButton.gameObject.SetActive(true);
        }
        // ��������, �������� �� ������ �������� � ������� ��� ��������� � ������ ����� ������
        CheckCartVsAvailableGold();
    }    

    private void CheckCartVsAvailableGold()// ����� ��� �������� ����������� ������� ��������� � ������� ��� ��������� � ������ ����� ������
    {
        // ��������� ���������� � ������ ������
        //var goldToCheck = _isSelling ? _shopSystem.AvailableGold : _playerInventoryHolder.CharacterStats.Coines;
        // ��������� ����� ������ ����� ����� ������� � ����������� �� ����, ���������� �� � ������ ������ ��� ������� ���������
        //_basketTotalText.color = _basketTotal > goldToCheck ? Color.red : Color.white;

        // ���� ������������� �� ������� "�������" ��� � ��������� ���������� �����, �� ������� �� ������
        if (_isSelling || _playerInventoryHolder.Inventory.CheckInventoryRemaining(_shoppingCart)) return;

        // ���� ��� ����� � ���������, �� �������� ����� ������� �� ��������� �� ������ � ������ ��� ���� �� �������
        _basketTotalText.text = "Not enoght room in inventory";
        _basketTotalText.color = Color.red;
    }

    public static int GetModifiedPrice(InventoryItemData data, int amount, float markUp)// ��������� ����, �������� ���������� � �������.
    {
        var baseValue = data.GoldValue * amount;// ������������ ������� ���������, �������� ���������� ������.
        return Mathf.FloorToInt(baseValue + baseValue * markUp);// ������������ �������� ���� � ������ �������.
    }

    private void UpdateItemPreview(ShopSlotUI shopSlotUI)// ��������� ��������������� �������� ������ � ��������.
    {
        var data = shopSlotUI.AssignedItemSlot.ItemData;// �������� ������ � ������.

        _itemPreviewSprite.sprite = data.Icon;// ��������� ����������� ���������������� ��������� ������.
        _itemPreviewSprite.color = Color.white;
        _itemPriviewName.text = data.DisplayName;// ��������� �������� ���������������� ��������� ������.
        _itemPriviewDiscription.text = data.Description;// ��������� �������� ���������������� ��������� ������.
    }
    public void OnBuyTabPressed()// ���������� ������� ������� �� ������� "������".
    {
        _isSelling = false;// ������������� ���� "�������" � �������� false.
        RefreshDisplay();// ��������� ����������� ��������.
    }
    public void OnSellTabPressed()// ���������� ������� ������� �� ������� "�������".
    {
        _isSelling = true;// ������������� ���� "�������" � �������� true.
        RefreshDisplay();// ��������� ����������� ��������.
    }   
}

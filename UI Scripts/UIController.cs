
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Класс управления пользовательским интерфейсом
public class UIController : MonoBehaviour
{

    PlayerController _input;
    [SerializeField] private InventoryDisplay _inventoryDisplay;
    [SerializeField] private Bars _playerActivUI;
    [SerializeField] private ExpirianceDisplay _expirianceDisplay;
    [SerializeField] private ChestDisplay _chestDisplay;
    [SerializeField] private LootDisplay _lootDisplay;
    [SerializeField] private ShopKeeperDisplay _shopKeeperDisplay;
    [SerializeField] private PortalDisplay _portalDisplay;
    [SerializeField] private ItemInfo _itemInfoPanel;

    public static UnityAction OpenItemInfo;
    public static UnityAction CloseItemInfo;

    public Bars PlayerActivUI => _playerActivUI;
    public ItemInfo InfoItem => _itemInfoPanel;

    public SceneLoader _sceneLoader;
    public InventoryHolder _holder;
    private bool _inventoryEnable = false;
    private bool _expirianceEnable = false;

    public static UnityAction LoadXpForItems;

    private void Awake()
    {
        OpenItemInfo += OpenTab;
        CloseItemInfo += CloseTab;

        _input = new PlayerController();

        LoadXpForItems += LoadXp;

        ExperienceSystem.LvlUp += LVLUP;
        ExperienceSystem.LvlMax += RefreshXp;
        ExperienceSystem.TakedXP += RefreshXp;

        _chestDisplay.gameObject.SetActive(false);
        _shopKeeperDisplay.gameObject.SetActive(false);
        _lootDisplay.gameObject.SetActive(false);
        _portalDisplay.gameObject.SetActive(false);
        _sceneLoader = FindAnyObjectByType<SceneLoader>();
        _itemInfoPanel.gameObject.SetActive(false);
    }

    private void LoadXp()
    {
        var classXpList = _expirianceDisplay._classItem;
        var classList = _holder.Experience.ItemListClass;

        foreach (var classXpInList in classXpList)
        {
            foreach (var classInList in classList)
            {
                if (classXpInList._name.text == classInList.NameClass)
                {
                    classXpInList.SetXPLVL(classInList);
                }
            }
        }
    }
    private void OnEnable()
    {
        _input.Enable();// Включение управления вводом игрока
        LootBage.LootBag += DisplayLootWindow;
        ChestInventory.InventoryChest += DisplayChestInventory;
        ShopKeeper.OnShopWindowRequested += DisplayShopWhindow;
        PortalPanel.CliclOnPortalPanel += DisplayPortalPanel;
    }
    private void OnDisable()
    {
        _input.Disable();// Отключение управления вводом игрока
        LootBage.LootBag -= DisplayLootWindow;
        ChestInventory.InventoryChest -= DisplayChestInventory;
        ShopKeeper.OnShopWindowRequested -= DisplayShopWhindow;
        PortalPanel.CliclOnPortalPanel -= DisplayPortalPanel;
    }
    private void Update()
    {
        InventoryDisplayWindow();
        ExpirianceDisplayWindow();

        if (_input.Player.Esc.triggered)
            CloseActiveWindows();      
    }

    private void InventoryDisplayWindow()
    {
        if (_input.Player.Inventory.triggered)
        {
            _inventoryEnable = !_inventoryEnable;
        }
        if (_inventoryEnable && _input.Player.Esc.triggered)
        {
            _inventoryEnable = false;
        }
        _inventoryDisplay.gameObject.SetActive(_inventoryEnable);
    }
    private void ExpirianceDisplayWindow()
    {
        if (_input.Player.ExpTree.triggered)
        {
            _expirianceEnable = !_expirianceEnable;
        }
        if (_expirianceEnable && _input.Player.Esc.triggered)
        {
            _expirianceEnable = false;
            _expirianceDisplay._statsBonus.gameObject.SetActive(false);
        }
        _expirianceDisplay.gameObject.SetActive(_expirianceEnable);
    }
    private void DisplayShopWhindow(ShopSystem shopSystem, InventoryHolder playerInventory)
    {
        _shopKeeperDisplay.gameObject.SetActive(true);// Включение компонента UI для отображения магазина
        _shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory);// Отображение окна магазина с переданными параметрами
    }
    void DisplayChestInventory(InventorySystem invToDisplay, int i)
    {
        _chestDisplay.gameObject.SetActive(true);
        _chestDisplay.RefreshDynamicInventory(invToDisplay);
    }
    private void DisplayLootWindow(InventorySystem loot, int i)
    {
        _lootDisplay.gameObject.SetActive(true);// Включение компонента UI для отображения магазина
        _lootDisplay.RefreshLootBag(loot);
    }
    public void CloseActiveWindows()
    {
        _chestDisplay.gameObject.SetActive(false);
        _shopKeeperDisplay.gameObject.SetActive(false);
        _lootDisplay.gameObject.SetActive(false);
        _portalDisplay.gameObject.SetActive(false);
    }
    public void ToMainMenu()
    {
        _sceneLoader.Position = _holder.CurrentCoordinats;
        _holder.Characteristics.SetLocation(_sceneLoader.Location);
        _holder.Characteristics.SetCoord(_sceneLoader.Position);

        SaveAndLoadManager.SaveCharacteristics(_sceneLoader.CharName);
        SaveAndLoadManager.SaveInventory(_sceneLoader.CharName);
        SaveAndLoadManager.SavePlayerXP(_sceneLoader.CharName);

        SceneManager.LoadSceneAsync(1);
    }
    public void InventoryCheck()
    {
        if (!_inventoryEnable) _inventoryEnable = true;
        else if (_inventoryEnable) _inventoryEnable = false;
        _inventoryDisplay.gameObject.SetActive(_inventoryEnable);
    }
    public void ExpirianceCheck()
    {
        if (!_expirianceEnable) _expirianceEnable = true;
        else if (_expirianceEnable) _expirianceEnable = false;
        _expirianceDisplay.gameObject.SetActive(_expirianceEnable);
    }
    private void LVLUP(ItemsXP item)
    {
        _expirianceDisplay.RefreshXpDisplayLVL(item);
    }
    private void RefreshXp(ItemsXP xp)
    {
        _expirianceDisplay.RefreshXpDisplay(xp);
    }
    private void DisplayPortalPanel()
    {
        _portalDisplay.gameObject.SetActive(true);
    }

    private void OpenTab()
    {
        _itemInfoPanel.gameObject.SetActive(true);
    }
    private void CloseTab()
    {
        _itemInfoPanel.gameObject.SetActive(false);
    }
}

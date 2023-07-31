
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// ����� ���������� ���������������� �����������
public class UIController : MonoBehaviour
{

    PlayerController _input;
    [SerializeField] private InventoryDisplay _inventoryDisplay;
    [SerializeField] private ChestDisplay _chestDisplay;
    [SerializeField] private LootDisplay _lootDisplay;
    [SerializeField] private ShopKeeperDisplay _shopKeeperDisplay;
    public SceneLoader _sceneLoader;
    public InventoryHolder _holder;
    private bool _inventoryEnable = false;

    private void Awake()
    {
        _input = new PlayerController();

        _chestDisplay.gameObject.SetActive(false);
        _shopKeeperDisplay.gameObject.SetActive(false);
        _lootDisplay.gameObject.SetActive(false);
        _sceneLoader = FindAnyObjectByType<SceneLoader>();

    }
    private void OnEnable()
    {
        _input.Enable();// ��������� ���������� ������ ������
        LootBage.LootBag += DisplayLootWindow;
        ChestInventory.InventoryChest += DisplayChestInventory;
        ShopKeeper.OnShopWindowRequested += DisplayShopWhindow;

    }



    private void OnDisable()
    {
        _input.Disable();// ���������� ���������� ������ ������
        LootBage.LootBag -= DisplayLootWindow;
        ChestInventory.InventoryChest -= DisplayChestInventory;
        ShopKeeper.OnShopWindowRequested -= DisplayShopWhindow;

    }

    private void Update()
    {
        InventoryDisplayWindow();
              
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

    private void DisplayShopWhindow(ShopSystem shopSystem, InventoryHolder playerInventory)
    {
        _shopKeeperDisplay.gameObject.SetActive(true);// ��������� ���������� UI ��� ����������� ��������
        _shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory);// ����������� ���� �������� � ����������� �����������
    }
    void DisplayChestInventory(InventorySystem invToDisplay, int i)
    {
        _chestDisplay.gameObject.SetActive(true);
        _chestDisplay.RefreshDynamicInventory(invToDisplay);
    }

    private void DisplayLootWindow(InventorySystem loot, int i)
    {
        _lootDisplay.gameObject.SetActive(true);// ��������� ���������� UI ��� ����������� ��������
        _lootDisplay.RefreshLootBag(loot);
    }

    public void CloseActiveWindows()
    {
        _chestDisplay.gameObject.SetActive(false);
        _shopKeeperDisplay.gameObject.SetActive(false);
        _lootDisplay.gameObject.SetActive(false);
    }
    public void ToMainMenu()
    {
        _sceneLoader.Position = _holder.CurrentCoordinats;
        _holder.Inventory.SetLocation(_sceneLoader.Location);
        _holder.Inventory.SetCoord(_sceneLoader.Position);

        SaveAndLoadManager.SaveInventory();
        SaveAndLoadManager.SavePlayerXP();

        SceneManager.LoadSceneAsync(1);
    }

    public void InventoryCheck()
    {
        if (!_inventoryEnable) _inventoryEnable = true;
        else if (_inventoryEnable) _inventoryEnable = false;
        _inventoryDisplay.gameObject.SetActive(_inventoryEnable);
    }

}

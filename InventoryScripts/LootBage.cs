using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class LootBage : MonoBehaviour, IInteractable
{
    public Database _whatLoot;
    private int[] _chance = { 60, 30, 10 };
    private int _total = 100;
    private int _chanceSpawn;
    [SerializeField] private int _itemsCount;
    [SerializeField] private int _slotsCount;
    private UIController _uiController;

    [SerializeField] protected InventorySystem _inventory;
    public InventorySystem Inventory => _inventory;

    private List<string> _selectedItems = new List<string>();
    public int ItemCount => _itemsCount;
    public static UnityAction<InventorySystem, int> LootBag;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    private void Awake()
    {
        _itemsCount = Random.Range(1, 5);
        _slotsCount = _itemsCount;
        _slotsCount += 3;
        _inventory = new InventorySystem(_slotsCount);
       // Debug.Log("Количество слотов и предметов " +  _itemsCount);
       
    }
    private void Start()
    {
        foreach (var chance in _chance)
        {
            _total += chance;
        }
        ItemCountAdd();

        StartCoroutine(DestroyAfter(120f));
    }
    private void Update()
    {
        CheckEmptySlotsAndDestroy();
    }
    public void ItemCountAdd()
    {
        
        //Debug.Log("Получено " + _itemsCount + " шмоток!");
        for (int i = 0; i < _itemsCount; i++)
            GetRandomItem();
    }

    public void GetRandomItem()
    {
        while (_selectedItems.Count < _itemsCount)
        {
            _chanceSpawn = Random.Range(1, _total);

            for (var i = 0; i < _chance.Length; i++)
            {
                if (_chanceSpawn <= _chance[i])
                {
                    var itemsWithChance = _whatLoot.GetItemDatabase()
                        .Where(item => item.Chance > _chanceSpawn)
                        .OrderBy(item => item.Chance)
                        .ToList();

                    if (itemsWithChance.Count > 0)
                    {
                        var randomItem = itemsWithChance[Random.Range(0, itemsWithChance.Count)];
                        _selectedItems.Add(randomItem.DisplayName); // Добавляем выбранный предмет в список
                        AddToLootBag(randomItem, 1); // Добавляем предмет в инвентарь
                    }

                    break;
                }
                else
                {
                    _chanceSpawn -= _chance[i];
                }
            }
        }
    }



    public void AddToLootBag(InventoryItemData item, int count)
    {
        var existingSlot = _inventory.InventorySlots.FirstOrDefault(slot => slot.ItemData == item);
        if (existingSlot != null)
        {
            if (existingSlot.StackSize == 1)
            {
                // Найден слот с предметом, у которого максимальный стак-сайз равен 1
                // Проверяем следующие слоты и добавляем предмет в первый свободный слот
                for (int i = _inventory.InventorySlots.IndexOf(existingSlot) + 1; i < _inventory.InventorySlots.Count; i++)
                {
                    var nextSlot = _inventory.InventorySlots[i];
                    if (nextSlot.ItemData == null)
                    {
                        nextSlot.AssignItem(item, count);
                        return;
                    }
                }
            }
            existingSlot.AddToStack(count);
        }
        else
        {
            _inventory.AddToInventory(item, count);
        }
    }

    public void Interact(ActionController interactor, out bool interactSuccessful)
    {
        // Вызываем делегат для отображения окна инвентаря
        LootBag?.Invoke(_inventory, 0);
        // Устанавливаем флаг успешного взаимодействия
        interactSuccessful = true;
    }

    public void EndInteraction()
    {

        Debug.Log("Должно закрыть окно");
    }

    private void CheckEmptySlotsAndDestroy()
    {
        bool allSlotsEmpty = _inventory.InventorySlots.All(slot => slot.ItemData == null);

        if (allSlotsEmpty)
        {
            //Debug.Log("Удалил без лута");
            _uiController = FindObjectOfType<UIController>();
            _uiController.CloseActiveWindows();
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}

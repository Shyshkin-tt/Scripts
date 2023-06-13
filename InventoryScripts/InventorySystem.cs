using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] protected string _acc;
    [SerializeField] protected string _name;
    [SerializeField] protected string _location;
    [SerializeField] protected int _coins;
    [SerializeField] protected int _ip;
    [SerializeField] protected int _hp;
    [SerializeField] protected int _mp;
    [SerializeField] protected int _pd;
    [SerializeField] protected int _md;
    [SerializeField] protected float _as;
    [SerializeField] protected float _ms;
    [SerializeField] protected int _pdef;
    [SerializeField] protected int _mdef;
    [SerializeField] protected int _hpValue;
    [SerializeField] protected int _mpValue;
    [SerializeField] protected int _hpRec;
    [SerializeField] protected int _mpRec;

    [SerializeField] protected Vector3 _spawnCoords;
    [SerializeField] protected Vector3 _curentCoordinats;

    [SerializeField] protected SkinnedMeshRenderer _playerSkin;
    [SerializeField] protected InventoryHolder _holder;

    private float _timeRec = 2f;
    private float _lastRecHP = 0f;
    private float _lastRecMP = 0f;
    public string Name => _name;
    public string Location => _location;
    public int Coines => _coins;
    public int ItemPower => _ip;
    public int Health => _hp;
    public int Mana => _mp;
    public int PDmg => _pd;
    public int MDmg => _md;
    public float AttackSpeed => _as;
    public float MoveSpeed => _ms;
    public int PDef => _pdef;
    public int MDef => _mdef;
    public int HPValue => _hpValue;
    public int MPValue => _mpValue;
    public int HPRec => _hpRec;
    public int MPRec => _mpRec;
    public Vector3 SpawnCoord => _spawnCoords;
    public Vector3 CurrentCoordinats => _curentCoordinats;
    
    public InventoryHolder Holder => _holder;

    [SerializeField] protected List<InventorySlot> _inventorySlots;
    [SerializeField] protected List<InventorySlot> _equipSlots;
    [SerializeField] protected List<InventorySlot> _beltSlots;
    public List<InventorySlot> InventorySlots => _inventorySlots;
    public List<InventorySlot> EquipSlots => _equipSlots;
    public List<InventorySlot> BeltSlots => _beltSlots;
    public int InventorySize => InventorySlots.Count;
    public int EquipSlotsCount => EquipSlots.Count;
    public int BeltSlotsCount => BeltSlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;
    public InventorySystem(int bagSlots, int equpslots, int beltsslots, string acc, string name, string location, int coins, int ip, int hp, int mp, int pd, int md, float @as, float ms,
        int pdef, int mdef, int hpValue, int mpValue, int hpRec, int mpRec, Vector3 lastCoordinats, Vector3 curentCoordinats, SkinnedMeshRenderer playerSkin, InventoryHolder holder)
    {
        _acc = acc;
        _name = name;
        _location = location;
        _coins = coins;
        _ip = ip;
        _hp = hp;
        _mp = mp;
        _pd = pd;
        _md = md;
        _as = @as;
        _ms = ms;
        _pdef = pdef;
        _mdef = mdef;
        _hpValue = hpValue;
        _mpValue = mpValue;
        _hpRec = hpRec;
        _mpRec = mpRec;
        _spawnCoords = lastCoordinats;
        _curentCoordinats = curentCoordinats;
        _playerSkin = playerSkin;
        _holder = holder;

        CreatePlayerInventory(bagSlots, equpslots, beltsslots);
        _holder = holder;
    }

    public InventorySystem(int slot)
    {
        CreateInventory(slot);
    }

    private void CreatePlayerInventory(int bagslots, int equpslots, int beltsslots)
    {
        _inventorySlots = new List<InventorySlot>(bagslots);

        for (int i = 0; i < bagslots; i++)
        {
            _inventorySlots.Add(new InventorySlot());
        }
        _equipSlots = new List<InventorySlot>(equpslots);

        for (int i = 0; i < equpslots; i++)
        {
            _equipSlots.Add(new InventorySlot());
        }

        _beltSlots = new List<InventorySlot>(beltsslots);

        for (int i = 0; i < beltsslots; i++)
        {
            _beltSlots.Add(new InventorySlot());
        }

    }

    private void CreateInventory(int bagslots)
    {
        _inventorySlots = new List<InventorySlot>(bagslots);

        for (int i = 0; i < bagslots; i++)
        {
            _inventorySlots.Add(new InventorySlot());
        }
    }
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) // Существует ли предмет в инвинтаре
        {
            foreach (var slot in _inventorySlots)
            {
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {

                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }

        }

        if (HasFreeSlot(out InventorySlot freeSlot)) // Берем первый свободный слот
        {
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
        }
        return false;
    }
    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot) // проверяем если в наших слотах предмет что б его добавить
    {
        invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList(); //Если да, получаю их список
        return invSlot.Count == 0 ? false : true;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot) //получаем первый свободный слот
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }

    public bool CheckInventoryRemaining(Dictionary<InventoryItemData, int> shoppingCart)
    {

        var clonedSystem = new InventorySystem(this.InventorySize);

        for (int i = 0; i < InventorySize; i++)
        {

            clonedSystem.InventorySlots[i].AssignItem(this.InventorySlots[i].ItemData, this.InventorySlots[i].StackSize);
        }
        foreach (var kvp in shoppingCart)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                if (!clonedSystem.AddToInventory(kvp.Key, 1)) return false;
            }
        }
        return true;
    }

    public Dictionary<InventoryItemData, int> GettAllItemsHeld()
    {
        var distinctItems = new Dictionary<InventoryItemData, int>();

        foreach (var item in _inventorySlots)
        {
            if (item.ItemData == null) continue;

            if (!distinctItems.ContainsKey(item.ItemData)) distinctItems.Add(item.ItemData, item.StackSize);
            else distinctItems[item.ItemData] += item.StackSize;
        }
        return distinctItems;
    }

    public void RemoveItemsFromInventory(InventoryItemData data, int amount)
    {
        //Удаляем определенное количество предметов из инвентаря
        if (ContainsItem(data, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot)
            {
                var stackSize = slot.StackSize;

                if (stackSize > amount) slot.RemoveFromStack(amount);
                else
                {
                    slot.RemoveFromStack(stackSize);
                    amount -= stackSize;
                }
                Debug.Log("remove item from slot");
                OnInventorySlotChanged?.Invoke(slot);
            }
        }
    }

    public void GetCurentCoords(InventoryHolder holder)
    {
        _curentCoordinats = holder.GetComponent<Transform>().transform.position;
    }
    public void SpendGold(int basketTotal)
    {
        _coins -= basketTotal;
    }
    public void GainGold(int price)
    {
        _coins += price;
    }
    public void SetValue()
    {
        _hpValue = _hp;
        _mpValue = _mp;
    }

    public void RecoveryHP()
    {
        float currentTime = Time.time;

        if (_hpValue < _hp)
        {
            if (currentTime - _lastRecHP >= _timeRec)
            {
                _hpValue += _hpRec;
                _lastRecHP = currentTime;
            }
        }
        else if (_hpValue >= _hp)
            _hpValue = _hp;
    }

    public void RecoveryMP()
    {
        float currentTime = Time.time;
        if (_mpValue < _mp)
        {
            if (currentTime - _lastRecMP >= _timeRec)
            {
                _mpValue += _mpRec;
                _lastRecMP = currentTime;
            }
        }
        else if (_mpValue >= _mp)
            _mpValue = _mp;
    }

    public void PlayerTakeDamage(int damage)
    {
        _hpValue -= damage;
    }

    public void ManaCoast(int mana)
    {
        _mpValue -= mana;
    }

    public void StatAdd(InventoryItemData stats)
    {
        _ip += stats.ItemPower;
        _hp += stats.Health;
        _mp += stats.Mana;
        _pd += stats.PhysicDamage;
        _md += stats.MagicDamage;
        _as -= stats.AttackSpeed;
        _pdef += stats.PhysicDamage;
        _mdef += stats.MagicDamage;
        _hpRec += stats.HealthRecovery;
        _mpRec += stats.ManaRecovery;
        _hpValue += stats.Health;
        _mpValue += stats.Mana;

    }
    public void StatMinus(InventoryItemData stats)
    {
        _ip -= stats.ItemPower;
        _hp -= stats.Health;
        _mp -= stats.Mana;
        _pd -= stats.PhysicDamage;
        _md -= stats.MagicDamage;
        _as += stats.AttackSpeed;
        _pdef -= stats.PhysicDamage;
        _mdef -= stats.MagicDamage;
        _hpRec -= stats.HealthRecovery;
        _mpRec -= stats.ManaRecovery;
        _hpValue -= stats.Health;
        _mpValue -= stats.Mana;
    }

    public void SetName(string name)
    {
        _name = name;
    }
    public void SetLocation(string location)
    {
        _location = location;
    }

    public void SetCoord(Vector3 coords)
    {
        _spawnCoords = coords;
    }
}

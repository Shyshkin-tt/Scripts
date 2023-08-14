using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _location;
    [SerializeField] protected bool _inCombat;
    [SerializeField] protected float _timeExitCombat;
    [Header("____________________________________")]
    [SerializeField] protected int _coins;
    [Header("____________________________________")]
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
    [Header("____________________________________")]
    [SerializeField] protected Vector3 _spawnCoords;
    [SerializeField] protected Vector3 _curentCoordinats;
    [Header("____________________________________")]
    [SerializeField] protected SkinnedMeshRenderer _playerSkin;
    [SerializeField] protected InventoryHolder _holder;

    private float _timeRec = 5f;
    private float _lastRecHP = 0f;
    private float _lastRecMP = 0f;
    private float _lastTimeHit = 30f;
    
    public string Name => _name;
    public string Location => _location;
    public bool InCombat => _inCombat;
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
    public float LastTimeHit => _timeExitCombat;
    public Vector3 SpawnCoord => _spawnCoords;
    public Vector3 CurrentCoordinats => _curentCoordinats;
    public SkinnedMeshRenderer Skin => _playerSkin;
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

    public static UnityAction OnEquipSlotChanged;
    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int bagSlots, int equpslots, int beltsslots, string name, string location, bool inCobat, float exitCombat, int coins, int ip, int hp, int mp, int pd, int md, float @as, float ms,
        int pdef, int mdef, int hpValue, int mpValue, int hpRec, int mpRec, Vector3 lastCoordinats, Vector3 curentCoordinats, SkinnedMeshRenderer playerSkin, InventoryHolder holder)
    {
        
        _name = name;
        _location = location;
        _inCombat = inCobat;
        _timeExitCombat = exitCombat;
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
        
    }

    public InventorySystem(int slot)
    {
        CreateInventory(slot);
    }

    public void SetSkinAndHolder(SkinnedMeshRenderer skin, InventoryHolder holder)
    {
        _playerSkin = skin;
        _holder = holder;
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
                int combatHpRec = _hpRec / 2;
                if (_inCombat)
                {
                    _hpValue += combatHpRec;
                    _lastRecHP = currentTime;
                }
                else
                {
                    _hpValue += _hpRec;
                    _lastRecHP = currentTime;
                }                
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

        _inCombat = true;

        _timeExitCombat = _lastTimeHit;
    }

    public void ExitCombat()
    {
        _inCombat = false;        
    }

    public void TimeExitCombatLeft()
    {
        var exitCombat = _timeExitCombat -= Time.deltaTime;
    }

    public void ManaCoast(int mana)
    {
        _mpValue -= mana;
    }

    public void StatAdd(InventoryItemData stats, ItemXPSourse bonusClass)
    {
        _ip += stats.ItemPower + bonusClass.ItemPower;
        _hp += stats.Health + bonusClass.Health;
        _mp += stats.Mana + bonusClass.Mana;
        _pd += stats.PhysicDamage + bonusClass.PDmg;
        _md += stats.MagicDamage + bonusClass.MDmg;
        _as -= stats.AttackSpeed + bonusClass.AttackSpeed;
        _pdef += stats.PhysicDefence + bonusClass.PDef;
        _mdef += stats.MagicDefence + bonusClass.MDef;
        _hpRec += stats.HealthRecovery + bonusClass.HPRec;
        _mpRec += stats.ManaRecovery + bonusClass.MPRec;
        _hpValue += stats.Health + bonusClass.Health;
        _mpValue += stats.Mana + bonusClass.Mana; ;

    }
    public void StatMinus(InventoryItemData stats, ItemXPSourse bonusClass)
    {
        _ip -= stats.ItemPower + bonusClass.ItemPower;
        _hp -= stats.Health + bonusClass.Health;
        _mp -= stats.Mana + bonusClass.Mana;
        _pd -= stats.PhysicDamage + bonusClass.PDmg;
        _md -= stats.MagicDamage + bonusClass.MDmg;
        _as += stats.AttackSpeed + bonusClass.AttackSpeed;
        _pdef -= stats.PhysicDefence + bonusClass.PDef;
        _mdef -= stats.MagicDefence + bonusClass.MDef;
        _hpRec -= stats.HealthRecovery + bonusClass.HPRec;
        _mpRec -= stats.ManaRecovery + bonusClass.MPRec;
        _hpValue -= stats.Health + bonusClass.Health;
        _mpValue -= stats.Mana + bonusClass.Mana;
    }
    public void BonusStatAdd(ItemXPSourse stats)
    {
        _ip += stats.ItemPower;
        _hp += stats.Health;
        _mp += stats.Mana;
        _pd += stats.PDmg;
        _md += stats.MDmg;
        _as -= stats.AttackSpeed;
        _pdef += stats.PDef;
        _mdef += stats.MDef;
        _hpRec += stats.HPRec;
        _mpRec += stats.MPRec;
        _hpValue += stats.Health;
        _mpValue += stats.Mana;

    }
    public void BonusStatMinus(ItemXPSourse stats)
    {
        _ip -= stats.ItemPower;
        _hp -= stats.Health;
        _mp -= stats.Mana;
        _pd -= stats.PDmg;
        _md -= stats.MDmg;
        _as += stats.AttackSpeed;
        _pdef -= stats.PDef;
        _mdef -= stats.MDef;
        _hpRec -= stats.HPRec;
        _mpRec -= stats.MPRec;
        _hpValue -= stats.Health;
        _mpValue -= stats.Mana;
    }
    public void OnCreate(string name, string location, Vector3 coords)
    {
        _name = name;
        _location = location;
        _spawnCoords = coords;
    }
    public Vector3 GetSpawnCoordInHolder()
    {
        return _spawnCoords;
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

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using static UnityEditor.FilePathAttribute;
using UnityEngine.WSA;

[System.Serializable]
public class CharacterCharacteristics
{
   [SerializeField] protected string _name;
    [SerializeField] protected string _location;
    [SerializeField] protected bool _inCombat;
    [SerializeField] protected float _timeExitCombat;

    [Header("______________STATS_________________")]
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

    public CharacterCharacteristics(string name, string location, bool inCobat, float exitCombat, int ip, int hp, int mp, int pd, int md, float @as, float ms,
        int pdef, int mdef, int hpValue, int mpValue, int hpRec, int mpRec, Vector3 lastCoordinats, Vector3 curentCoordinats, SkinnedMeshRenderer playerSkin, InventoryHolder holder)
    {
        _name = name;
        _location = location;
        _inCombat = inCobat;
        _timeExitCombat = exitCombat;

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
    }
    public void SetSkinAndHolder(SkinnedMeshRenderer skin, InventoryHolder holder)
    {
        _playerSkin = skin;
        _holder = holder;
    }
    public void GetCurentCoords(InventoryHolder holder)
    {
        _curentCoordinats = holder.GetComponent<Transform>().transform.position;
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

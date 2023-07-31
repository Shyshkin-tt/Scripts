using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCCreator
{
    [SerializeField] protected int _npcTier;
    [SerializeField] protected int _npcLvl;
    [Header("Stats")]
    [SerializeField] protected int _hp;
    [SerializeField] protected int _mp;
    [SerializeField] protected int _hpValue;
    [SerializeField] protected int _mpValue;
    [SerializeField] protected float _attackSpeed;

    [SerializeField] protected int _xp;
    [Header("Patrol & aggro")]
    [SerializeField] protected int _patrolZone;
    [SerializeField] protected float _patrolTime;
    [SerializeField] protected float _aggroZone;

    [SerializeField] protected float _currentSpeed;
    [SerializeField] protected float _patrolSpeed;
    [SerializeField] protected float _aggroSpeed;
    [SerializeField] protected float _returnSpeed;
    [Header("Loot")]
    [SerializeField] protected int _minCoins;
    [SerializeField] protected int _maxCoins;

    [SerializeField] protected List<InventoryItemData> _lootList;

    public int NpcTier => _npcTier;
    public int NpcLvl => _npcLvl;
    public int NpcHP => _hp;
    public int NpcMP => _mp;
    public int NpcHPValue => _hpValue;
    public int NpcMPValue => _mpValue;
    public float NpcAttackSpeed => _attackSpeed;
    public int NpcXp => _xp;
    public int PatrolZone => _patrolZone;
    public float PatrolTime => _patrolTime;
    public float AggroZone => _aggroZone;
    public float CurrentSpeed => _currentSpeed;
    public float PatrolSpeed => _patrolSpeed;
    public float AggroSpeed => _aggroSpeed;
    public int MinCoins => _minCoins;
    public int MaxCoins => _maxCoins;

    public List<InventoryItemData> LootList => _lootList;

    public NPCCreator(int npcTier, int npcLvl, int hp, int mp, int hpValue, int mpValue, float attackSpeed, int xp, 
         float patrolTime, int patrolZone, float aggroZone, float currentSpeed, float patrolSpeed, float aggroSpeed, float returnSpeed, 
         int minCoins, int maxCoins, NPCData data)
    {
        _npcTier = npcTier;
        _npcLvl = npcLvl;
        
        _hp = hp;
        _mp = mp;
        _hpValue = hpValue;
        _mpValue = mpValue;
        _attackSpeed = attackSpeed;

        _xp = xp;
        
        _patrolZone = patrolZone;
        _patrolTime = patrolTime;
        _aggroZone = aggroZone;

        _currentSpeed = currentSpeed;
        _patrolSpeed = patrolSpeed;
        _aggroSpeed = aggroSpeed;
        _returnSpeed = returnSpeed;
        
        _minCoins = minCoins;
        _maxCoins = maxCoins;
        _lootList = data.LootList;
    }

    public void SetStats(NPCData data)
    {
        _npcTier = data.Tier;
        _npcLvl = data.MobLVL;

        _hp = data.HP;
        _mp = data.MP;
        _hpValue = _hp;
        _mpValue = _mp;
        _attackSpeed = data.AttackSpeed;

        _xp = data.XP;

        _patrolZone = data.PatrolZone;
        _patrolTime = data.PatrolTime;
        _aggroZone = data.AgrroZone;
        
        _patrolSpeed = data.PatrolSpeed;
        _aggroSpeed = data.AggroSpeed;

        _minCoins = data.MinCoins;
        _maxCoins = data.MaxCoins;
    }

    public bool SetCurrentSpeed(float speed)
    {
        _currentSpeed = speed;
        return true;
    }

    public void SetValueStats()
    {
        _hpValue = _hp;
        _mpValue = _mp;
    }
    public bool NPCTakeDamage(int damage)
    {
        _hpValue -= damage;
        return true;
    }
    public bool GiveXP(InventoryHolder player)
    {
        player.GetXP(_xp);
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC System/NPC")]
public class NPCData : ScriptableObject
{
    public int ID = -1;
    public int Tier;
    public int MobLVL;
    public string NPCType;
    public float ZoneAggro;

    public GameObject NPCPrefab;

    [Header("Stats")]
    public int HP;
    public int MP;
    public int PhysicDamage;
    public int MagicDamage;
    public float AttackSpeed;
    public float HitDistance;
    public int SpawnChance;

    [Header("Give to player")]
    public int XP;

    [Header("Patrol & aggro")]
    public int PatrolZone;
    public float PatrolTime;
    public float AgrroZone;

    public float PatrolSpeed;
    public float AggroSpeed;
    public float ReturnSpeed = 6f;

    [Header("Loot")]
    public bool HaveCoins;
    public int MinCoins;
    public int MaxCoins;

    public List<InventoryItemData> LootList;

}

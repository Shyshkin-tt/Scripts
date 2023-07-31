using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1;
    public int Tier;
    public string DisplayName;
    public string ItemType;
    public string ItemClass;
    public string SlotType;
    public string EquipSlotype;

    [TextArea(4, 4)]
    public string Description;
    public Sprite Icon;
    
    public int MaxStackSize;
    public int GoldValue;
    public int Chance;
    public GameObject ItemPrefab;

    [Header("Stats")]
    public int ItemPower;
    public int Health;
    public int Mana;
    public int PhysicDamage;
    public int MagicDamage;
    public float AttackSpeed;
    public int PhysicDefence;
    public int MagicDefence;
    public int HealthRecovery;
    public int ManaRecovery;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Spell", menuName = "Spell System/Create Spell", order = 1)]
public class SpellData : ScriptableObject
{
    public int ID = -1;
    public string SpellName;
    public Sprite Icon;
    [TextArea] public string Description;
    public int RequiredLevel;
    public GameObject SpellPrefab;
    // Добавить эффект после попадания
    public GameObject VisualFX;
    public AudioClip SoundFX;

    public DamageType TypeDamage;
    public SpawnPoint SpellSpawn;
    public SpawnType TypeSpawn;
    public SpellType SpellType;
    public TargetType TargetType;
    public CastType TypeCast;
    public ResourceType TypeResource;
    [Header("==================")]
    public bool _destroyOnTriggerEnter;
    public bool _dealDamageBeforeDestroy;
    
    public bool _moveForward;
    public bool _moveToPoint;

    public bool _delayDestroy;
    public float DestroyDistance;

    [Header("==================")]
    public int Damage;
    public int ManaCost;
    //public int Durotation;
    public float CastingTime;
    public float Cooldown;
    public float LifeTime;
    public int DistanceRange;
    public int Speed;
    public int SpellRepeatCount = 1;
    //public int DelayTime;
    //public bool SelfCast;
    //public bool Interruptible;
    //public int CooldownReduction;
    //public int ManaCostReduction;

    public SpawnPoint GetSpawnPoint()
    {
        return SpellSpawn;
    }
    public SpellType GetSpellType()
    {
        return SpellType;
    }
    public SpawnType GetSpawnType()
    {
        return TypeSpawn;
    }
    public TargetType GetTargetType()
    {
        return TargetType;
    }
    public CastType GetCastType()
    {
        return TypeCast;
    }
   
}

public enum DamageType
{
    Physical, Magical, Pure
}
public enum SpawnPoint
{
    // ======= RAYCAST ========== // ==================== SPAWN POINT ==================== //
    AtPoint, OverPoint, OnTopPoint, UpperForward, UpperCenter, GroundForward, GroundCenter
}
public enum SpellType
{
    Throw, Fall, Point, AroundPoint
}
public enum SpawnType
{
    Point, Random
}

public enum TargetType
{
    Solo, Mass
}
public enum CastType
{
    Instant, // моментальный
    Channeled, // поддерживание
    TimeCast, // со временем каста    
}
public enum ResourceType
{
    Mana, Health
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC System/NPC")]
public class NPCData : ScriptableObject
{
    public int ID = -1;
    public int Tier = 1;
    public int MobLVL = 1;
    public string NPCType;
    public float ZoneAggro;

    public GameObject NPCPrefab;

    [Header("Stats")]
    public int HP;
    public int PhysicDamage;

    public float AttackSpeed;
    public float PatrolSpeed;
    public float RunSpeed;
}

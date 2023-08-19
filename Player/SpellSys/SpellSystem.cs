using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellSystem
{
    [SerializeField] protected SpellData _spellQ;
    [SerializeField] protected float _cooldownQ;
    [SerializeField] protected SpellData _spellW;
    [SerializeField] protected float _cooldownW;
    [SerializeField] protected SpellData _spellE;
    [SerializeField] protected float _cooldownE;   
    public SpellData QSpell => _spellQ;
    public float QSpellCD => _cooldownQ;
    public SpellData WSpell => _spellW;
    public float WSpellCD => _cooldownW;
    public SpellData ESpell => _spellE;
    public float ESpellCD => _cooldownE;

    public SpellSystem (SpellData spellQ, float cooldownQ, SpellData spellW, float cooldownW, SpellData spellE, float cooldownE)
    {
        _spellQ = spellQ;
        _cooldownQ = cooldownQ;
        _spellW = spellW;
        _cooldownW = cooldownW;
        _spellE = spellE;
        _cooldownE = cooldownE;
    }

    public void SetSpellWhenEquip(InventoryItemData data)
    {
        if(data.SpellOne != null) _spellQ = data.SpellOne;
        if (data.SpellTwo != null) _spellW = data.SpellTwo;
        if (data.SpellThree != null) _spellE = data.SpellThree;
    }

    public void ClearSpells()
    {
        _spellQ = null;
        _spellW = null;
        _spellE = null;
    }

    public void SetSpellQ(SpellData spellQ)
    {
        _spellQ = spellQ;
    }
    public void SetSpellQCD(float cd)
    {
        _cooldownQ = cd;
    }
    public void SetSpellW(SpellData spellW)
    {
        _spellW = spellW;
    }
    public void SetSpellWCD(float cd)
    {
        _cooldownW = cd;
    }
    public void SetSpellE(SpellData spellE)
    {
        _spellE = spellE;
    }
    public void SetSpellECD(float cd)
    {
        _cooldownE = cd;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.Types;
using static Unity.VisualScripting.Member;

[System.Serializable]
public class ItemsXP : ItemXPSourse
{
    public ItemsXP(InventoryItemData source, int lvl, int xp, int xpMax,
        int ip, int hp, int mana, int pd, int md, float aSpeed, int pDef, int mDef, int hpRec, int mpRec)
    {
        _itemData = source;
        _nameClass = source.ItemClass;
        _nameItem = source.DisplayName;
        _itemID = source.ID;
        _lvl = lvl;
        _currentXp = xp;
        _xpToNextLvL = xpMax;

        _itemPower = ip;
        _health = hp;
        _mana = mana;
        _physicDamage = pd;
        _magicDamage = md;
        _attackSpeed = aSpeed;
        _physicDefence = pDef;
        _magicDefence = mDef;
        _healthRecovery = hpRec;
        _manaRecovery = mpRec;


    }
    public void AddXp(int xp)
    {
        _currentXp += xp;
        ExperienceSystem.TakedXP?.Invoke(this);
    }

    public void AddLvl()
    {
        _lvl += 1;
        _currentXp -= _xpToNextLvL;
        UpClassXp();
    }

    public void MaxLvl()
    {
        _currentXp = _xpToNextLvL;
        ExperienceSystem.LvlMax?.Invoke(this);
    }

    public void UpClassXp()
    {
        _xpToNextLvL = (int)(_xpToNextLvL + Mathf.Pow(1.1281f, (_lvl - 1)) + 350);
    }   
}

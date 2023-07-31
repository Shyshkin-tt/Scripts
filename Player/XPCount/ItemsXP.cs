using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemsXP : ItemXPSourse
{    
    public ItemsXP (string nameClass, string nameItem, int lvl, int xp, int xpMax)
    {        
        _nameClass = nameClass;
        _nameItem = nameItem;
        _lvl = lvl;
        _xp = xp;
        _xpMax = xpMax;
    }
    public void AddXp(int xp)
    {
        _xp += xp;
    }

    public void AddLvl()
    {
        _lvl += 1;        
        _xp = 0;
    }

    public void MaxLvl()
    {
        _xp = _xpMax;
    }

    public void UpClassXp()
    {
        _xpMax = (int)(_xpMax * 1.131f);
    }
    public void UpItemXp()
    {
        _xpMax = (int)(_xpMax * 1.141f);
    }
}

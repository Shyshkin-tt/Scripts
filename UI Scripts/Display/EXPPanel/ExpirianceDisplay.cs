using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExpirianceDisplay : MonoBehaviour
{
    public EXPSlotItem[] _classItem;
    public StatsBonus _statsBonus;


    public void RefreshXpDisplayLVL(ItemsXP name)
    {
        foreach(var item in _classItem)
        {
            if (name.NameClass == item._name.text)
            {
                item.SetXPLVL(name);
            }
        }
    }
    public void RefreshXpDisplay(ItemsXP name)
    {        
        foreach (var item in _classItem)
        {
            if (name.NameClass == item._name.text)
            {
                item.SetXPLVL(name);
            }
        }
    }
}

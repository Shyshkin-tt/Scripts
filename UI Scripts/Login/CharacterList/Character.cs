using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class Character : CharInfo
{
    
    public Character(string name, string gender, string location)
    {
        _name = name;
        _gender = gender;
        _location = location;
    }

    public void UpdateCharName(string name)
    {
        _name = name;
               
    }
    public void UpdateCharGender(string gender)
    {
        _gender = gender;
    }
    public void UpdateCharLoc(string location)
    {
        _location = location;
    }
}

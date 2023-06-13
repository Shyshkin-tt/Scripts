using System.Collections;
using System.Collections.Generic;
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

    public void UpdateChar(string name, string gender, string location)
    {
        _name = name;
        _gender = gender;
        _location = location;
    }
}

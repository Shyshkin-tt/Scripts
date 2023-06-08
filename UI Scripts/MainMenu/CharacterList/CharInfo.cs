using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class CharInfo
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _gender;
    [SerializeField] protected string _location; 

    public string Name  => _name;
    public string Gender => _gender;
    public string Location => _location;


}

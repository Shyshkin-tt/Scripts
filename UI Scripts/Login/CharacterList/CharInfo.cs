using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public abstract class CharInfo
{
    [SerializeField] protected string _name;
    [SerializeField] protected string _gender;
    [SerializeField] protected string _location;
    [SerializeField] protected Vector3 _spawnCoords;

    public string Name  => _name;
    public string Gender => _gender;
    public string Location => _location;
    public Vector3 SpawnCoords => _spawnCoords;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[System.Serializable]
public class CharCreator 
{
    protected string _name;
    protected string _gender;
    protected string _location;
   
    [SerializeField] protected int _countCharacters;
    public string PlayerName => _name;
    public string PlayerGender => _gender;
    public string PlayerLocation => _location;   
    public int CountCharacters => _countCharacters;

    [SerializeField] private List<Character> _charList;
    public List<Character> CharLists => _charList;
    
    public CharCreator(int countChars)
    {           

        CreatePlayerCharsList(countChars);
    }

    private void CreatePlayerCharsList(int countChars)
    {
        _charList = new List<Character>(countChars);

        for (int i = 0; i < countChars; i++)
        {
            _charList.Add(new Character(_name, _gender, _location));
        }
    }
    public void SetGender(string gender)
    {
        _gender = gender;        
    }  

    public void CreateNewCharList(string name, string gender, string location)
    {
        _charList.Add(new Character(name, gender, location));
        _countCharacters += 1;        
    }
    public void DeleteCharFromList(int index)
    {
        _charList.RemoveAt(index);
        _countCharacters -= 1;
    }
}

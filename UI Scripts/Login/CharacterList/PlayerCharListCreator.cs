using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class PlayerCharListCreator
{
    [SerializeField] protected int _countChars;
    [SerializeField] protected List<Character> _characterPreviewList;
    public int CountChars => _countChars;
    public List<Character> CharacterPreviewList => _characterPreviewList;

    public PlayerCharListCreator(List<Character> charList, int countChars)
    {
        _characterPreviewList = charList;
        _countChars = countChars;        
    }

    public void AddCharToList(string name, string gender, string location, Vector3 spawnCoords)
    {        
        _characterPreviewList.Add(new Character(name, gender, location, spawnCoords));
        _countChars += 1;
    }
    public void RemoveCharFromList(int index)
    {       
        _characterPreviewList.RemoveAt(index);
        _countChars -= 1;
    }
}

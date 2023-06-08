using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Char_UI : MonoBehaviour
{
    public TextMeshProUGUI _name;
    public TextMeshProUGUI _gender;
    public TextMeshProUGUI _location;
    
    public Button _delButton;
    public Button Buttom => _delButton;
    private void Start()
    {
       // _delButton.onClick.AddListener(DeleteCharUI);
        
    }
    public void Sets(string name, string gender, string locaton)
    {
        _name.text = name;
        _gender.text = "Gender: " + $"{gender}";
        _location.text = "Location: " + $"{locaton}";     
    }

    public void DeleteCharUI()
    {
        int childIndex = transform.GetSiblingIndex();
        var list = transform.GetComponentInParent<PlayerCharList>().CharCreator;
        list.DeleteCharFromList(childIndex);
        Destroy(this.transform.gameObject);
    }
}

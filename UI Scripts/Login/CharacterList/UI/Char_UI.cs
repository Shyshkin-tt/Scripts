using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class Char_UI : MonoBehaviour
{
    public TextMeshProUGUI _name;
    public TextMeshProUGUI _gender;
    public TextMeshProUGUI _location;
    public Button _delButton;

    public PlayerCharListScene ParentDisplay { get; private set; }
    private void Start()
    {
        ParentDisplay = transform.parent.GetComponentInParent<PlayerCharListScene>();
        ActivChar();
    }
    public void Sets(string name, string gender, string location)
    {
        _name.text = name;
        _gender.text = gender;
        _location.text = location;     
    }

    public void DeleteCharUI()
    {
        var index = transform.GetSiblingIndex();        
        ParentDisplay.DeleteChar(index, _name.text);
        Destroy(this.transform.gameObject);

        if (ParentDisplay._activCharacterPreview == _name.text)
        {
            ParentDisplay.ActivCharScreenSet("");
            ParentDisplay._sceneLoader.CharName = "";
        }
        
    }


    public void ActivChar()
    {
        ParentDisplay.ActivCharScreenSet(_name.text);       
    }
}

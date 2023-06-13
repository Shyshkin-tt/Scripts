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
    public void Sets(string name, string gender, string locaton)
    {
        _name.text = name;
        _gender.text = gender;
        _location.text = locaton;     
    }

    public void DeleteCharUI()
    {
        var index = transform.GetSiblingIndex();
        ParentDisplay._playerCharList.CharCreator.DeleteCharFromList(index);
        ParentDisplay.DeleteChar();
        Destroy(this.transform.gameObject);
        ParentDisplay.ActivCharScreenSet("");
        SaveAndLoadManager.SaveCharList();
    }


    public void ActivChar()
    {
        ParentDisplay.ActivCharScreenSet(_name.text);

        if (ParentDisplay._previewCharacter.transform.childCount == 0)
        {
            ParentDisplay.CreateChars(_name.text, _gender.text, _location.text);
        }
        else if (ParentDisplay._previewCharacter.transform.childCount == 1)
        {
            ParentDisplay.DeleteChar();
            ParentDisplay.CreateChars(_name.text, _gender.text, _location.text);
        }
    }
}

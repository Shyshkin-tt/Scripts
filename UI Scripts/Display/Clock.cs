using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI _clockDisplay;    
       
    void Update()
    {
        System.DateTime currentTime = System.DateTime.Now;
       
        int hour = currentTime.Hour;
        int minute = currentTime.Minute;

        string hourString = hour.ToString("00");
        string minuteString = minute.ToString("00");

        _clockDisplay.text = hourString + ":" + minuteString;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class SettingToggle : MonoBehaviour
{
    private Text Lable;
    private void Awake()
    {
         Lable = transform.Find("Label").GetComponent<Text>();
    }

    public void IsOn(bool isOn)
    {
        if (isOn)
        {
            AudioManager.PlaySound("Button_Audio");
        }
        
        Color tempColor;
        ColorUtility.TryParseHtmlString(isOn?"#4DACFF":"#8ECAFF", out tempColor);
        Lable.color = tempColor;
        Lable.fontSize = isOn ? 40 : 32;
        Lable.fontStyle = isOn ? FontStyle.Bold : FontStyle.Normal;
    }
}

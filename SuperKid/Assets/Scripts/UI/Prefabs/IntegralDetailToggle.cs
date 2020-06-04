using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class IntegralDetailToggle : MonoBehaviour
{
   
    
    private Text mTitle;
    private Image mBackgroundPanel;


    private void Awake()
    {
    mTitle = transform.Find("Background/Text").GetComponent<Text>();
    mBackgroundPanel = transform.Find("Background").GetComponent<Image>();

    }
    
    public void IsOn(bool isOn)
    {
    Color titleColor;
    Color toggleBGColor;
    ColorUtility.TryParseHtmlString(isOn?"#FFD400":"#FFFFFF", out titleColor);
    ColorUtility.TryParseHtmlString(isOn?"#5A5FCF":"#6D72DF", out toggleBGColor);

    mTitle.color = titleColor; 
    mBackgroundPanel.color = toggleBGColor;
    }

}
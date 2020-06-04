using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class DailyMedalToggle : MonoBehaviour
{

    private Text mTitle;
    private Image mImage;
    private Image mBackground;
    private ResLoader mResLoader = ResLoader.Allocate();
    
    private void Awake()
    {
        mTitle = transform.Find("Background/Text").GetComponent<Text>();
        mBackground = transform.Find("Background").GetComponent<Image>();
        mImage = mBackground.transform.Find("GameObject/Image").GetComponent<Image>();
    }
    
    public void IsOn(bool isOn)
    {
        Color titleColor;
        Color toggleBGColor;
        ColorUtility.TryParseHtmlString(isOn?"#FFD400":"#FFFFFF", out titleColor);
        ColorUtility.TryParseHtmlString(isOn?"#5A5FCF":"#6D72DF", out toggleBGColor);
        var texture2D = mResLoader.LoadSync<Texture2D>( !isOn?"ic_dailyShare":"ic_dailyShareSel");
        mImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);

        mTitle.color = titleColor;
        // mImage.sprite = 
        mBackground.color = toggleBGColor;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class FamilyItem : MonoBehaviour
{
    [SerializeField]
    public Button mButton;
    [SerializeField]
    public Text mText;

    public Toggle mToggle;
    
    public int index;
    
    private void Awake()
    {
        mText.text = "-"; 
        
    }

    public void SetContent(string text, Color bgColor, int type)
    {
        mText.text = text + " : " + index;
        mButton.image.color = bgColor;
    }

}

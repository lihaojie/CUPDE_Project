using System;
using System.Net.Mime;
using QFramework;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class TextLine : MonoBehaviour
{
    private Text linkText;
    public LineType lineType;

    public string textContent;
    private void Awake()
    {
        linkText = GetComponent<Text>();
    }

    private void Update()
    {
        if (!linkText.text.Equals(textContent))
        {
            textContent = linkText.text;
            CreateLink(linkText);
        }
    }

    public void CreateLink(Text text)
    {
        if (text == null)
            return;
        //克隆Text，获得相同的属性  
        text.DestroyAllChild();
        text.GetComponent<TextLine>().enabled = false;
        Text underline = Instantiate(text) as Text;
        underline.name = "Underline";
        underline.transform.SetParent(linkText.transform);
        underline.rectTransform.localScale = Vector3.one;
        RectTransform rt = underline.rectTransform;

        //设置下划线坐标和位置  
        rt.anchoredPosition3D = Vector3.zero;
        rt.offsetMax = Vector2.zero;
        rt.offsetMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.anchorMin = Vector2.zero;
        if (lineType == LineType.UnderLine)
        {
            underline.text = "_";
        }
        else
        {
            underline.text = "-";
        }

        float perlineWidth = underline.preferredWidth; //单个下划线宽度  
        Debug.Log(perlineWidth);
        float width = text.preferredWidth;
        Debug.Log(width);
        int lineCount = (int) Mathf.Round(width / perlineWidth);
        Debug.Log(lineCount);
        for (int i = 1; i < lineCount; i++)
        {
            if (lineType == LineType.UnderLine)
            {
                underline.text += "_";
            }
            else
            {
                underline.text += "-";
            }
        }
        text.GetComponent<TextLine>().enabled = true;

    }

    public enum LineType
    {
        UnderLine,
        DeleteLine
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class InputDelEmoji: MonoBehaviour
{
    List<string> patterns = new List<string>();

    public InputField input;
    public Text tex;
    public Text tex1;

    public string lastStr;

    void Awake()
    {
        patterns.Add(@"\p{Cs}");
        patterns.Add(@"[\u2702-\u27B0]");

        input.onValidateInput = MyOnValidateInput;

        input.onEndEdit.AddListener((arg0 =>
        {
            tex.text = arg0;
        }));

        input.onValueChanged.AddListener((arg0 =>
        {
            tex1.text = arg0;
        }));
    }


    private char MyOnValidateInput(string text, int charIndex, char addedChar)
    {
        if (patterns.Count > 0)
        {
            string s = string.Format("{0}", addedChar);
            if (BEmoji(s))
            {
                return '\0';
            }
        }
        return addedChar;
    }

    private bool BEmoji(string s)
    {
        bool bEmoji = false;
        for (int i = 0; i < patterns.Count; ++i)
        {
            bEmoji = Regex.IsMatch(s, patterns[i]);
            if (bEmoji)
            {
                break;
            }
        }
        return bEmoji;
    }

    public void AddPatterns(string s)
    {
        patterns.Add(s);
    }

    public void ClearPatterns(string s)
    {
        patterns.Clear();
    }
}
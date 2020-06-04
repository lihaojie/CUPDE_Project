using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using SuperKid;
using UnityEngine;
using Object = System.Object;

public class SignUtil 
{
    public static String SIGN = "sign";
    public static String APP_ID = "appid";

    public static Dictionary<String, object> ConvertParameterDict(Dictionary<String, Object> paramDictionary)
    {
        paramDictionary.Add(APP_ID, AppConst.APPID);
        
        return paramDictionary;
    }

    /**
     * 验签生成
     */
    public static string getSign(Dictionary<string, object> paramDict)
    {
        // 拼接固定字符串
        Dictionary<string, object> param = ConvertParameterDict(paramDict);
        // 排序动作
        List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>(param);
        list.Sort(delegate(KeyValuePair<string, object> pair1, KeyValuePair<string, object> pair2)
        {
            return pair1.Key.CompareTo(pair2.Key);
        });
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            KeyValuePair<string, object> pair = list[i];
            sb.Append(pair.Key.ToLower());
            sb.Append("=");
            sb.Append(pair.Value);
            if (i != list.Count - 1)
            {
                sb.Append("&");
            }
        }
        sb.Append("&appkey=" + AppConst.APPKEY);
        return CreateMD5Str(sb.ToString().Trim()).ToLower();
    }

    /**
     * 获取方法参数字符串
     */
    public static string getMethodParamStr(Dictionary<string, object> paramDict)
    {
        List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>(paramDict);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            KeyValuePair<string, object> pair = list[i];
            sb.Append(pair.Key);
            sb.Append("=");
            sb.Append(pair.Value);
            if (i != list.Count - 1)
            {
                sb.Append("&");
            }
        }
        return sb.ToString();
    }

    
    public static string CreateMD5Str(string str)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(str);
        return CalcMD5(buffer);
    }


    public static string CalcMD5(byte[] buffer)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] md5Bytes = md5.ComputeHash(buffer);
            return BytesToString(md5Bytes);
        }
    }

    private static string BytesToString(byte[] md5Bytes)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < md5Bytes.Length; i++)
        {
            sb.Append(md5Bytes[i].ToString("X2"));
        }
        return sb.ToString();
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using QFramework;
using SuperKid;
using SuperKid.Utils;
using UniRx;
using UnityEngine;

public class HttpUtil
{
    public static string token;


    public static IObservable<T> PostWithSign<T>(string url, Dictionary<string, object> param, 
        string fileName = null, string filePath = null, bool showToast = true)
    {
        StringBuilder strParam = new StringBuilder();
        var formParam = new WWWForm();
        if (token.IsNullOrEmpty())
        {
            token = PlayerPrefsUtil.GetToken();
        }

        if (!token.IsNullOrEmpty())
        {
            param.Add("token", token);
        }

        if (param.Count > 0)
        {
            param.ForEach(pair =>
            {
                strParam.Append(pair.Key);
                strParam.Append(":");
                strParam.Append(pair.Value.ToString());
                strParam.Append("&");
                formParam.AddField(pair.Key, pair.Value.ToString());
            });
            string sign = SignUtil.getSign(param);
            formParam.AddField("sign", sign);
            formParam.AddField("appid", AppConst.APPID);
            strParam.Append("sign");
            strParam.Append(":");
            strParam.Append(sign);
            strParam.Append("&");
            strParam.Append("appid");
            strParam.Append(":");
            strParam.Append(AppConst.APPID);
        }

        if (filePath.IsNotNullAndEmpty())
        {
            formParam.AddBinaryData("file", File.ReadAllBytes(filePath), fileName);
            strParam.Append("&");
            strParam.Append("file");
            strParam.Append(":");
            strParam.Append(File.ReadAllBytes(filePath));
        }

        return ObservableWWW.Post(url, formParam).Select<string, T>(s =>
        {
            Log.I("url==" + url);
            Log.I("formParam==" + strParam);
            Log.I("返回信息是" + s);
            if (s.IsNullOrEmpty())
            {
                throw new HttpException("104", "网络连接失败", 104);
            }

            BaseEntity<T> deserializeObject = JsonConvert.DeserializeObject<BaseEntity<T>>(s);
            if (deserializeObject.status != 1)
            {
                CommonUtil.error(deserializeObject.errCode, deserializeObject.message, deserializeObject.status);
                throw new HttpException(deserializeObject.errCode, deserializeObject.message, deserializeObject.status);
            }

            return deserializeObject.data;
        }).DoOnError(e =>
        {
            LoadingManager.GetInstance().DismissLoading();
            if (showToast)
            {
                if (e is HttpException)
                {
                    CommonUtil.toast(((HttpException)e).ErrMessage);
                }
                else if (e is WWWErrorException)
                {
                    ToastManager.GetInstance().CreatToast("网络连接失败，请检查网络连接");
                }else 
                {
                    ToastManager.GetInstance().CreatToast(e.Message);
                }
            }
        });
    }

    public static IObservable<string> PostWithSign(string url, Dictionary<string, object> param, bool showToast = true)
    {
        StringBuilder strParam = new StringBuilder();
        var formParam = new WWWForm();
        if (token.IsNullOrEmpty())
        {
            token = PlayerPrefsUtil.GetToken();
        }

        if (!token.IsNullOrEmpty())
        {
            param.Add("token", token);
        }

        if (param.Count > 0)
        {
            param.ForEach(pair =>
            {
                strParam.Append(pair.Key);
                strParam.Append(":");
                strParam.Append(pair.Value.ToString());
                strParam.Append("&");
                formParam.AddField(pair.Key, pair.Value.ToString());
            });
            string sign = SignUtil.getSign(param);
            formParam.AddField("sign", sign);
            formParam.AddField("appid", AppConst.APPID);
            strParam.Append("sign");
            strParam.Append(":");
            strParam.Append(sign);
            strParam.Append("&");
            strParam.Append("appid");
            strParam.Append(":");
            strParam.Append(AppConst.APPID);
        }
        Log.I("url==" + url);
        Log.I("formParam==" + strParam);
        return ObservableWWW.Post(url, formParam).DoOnError(e =>
        {
            LoadingManager.GetInstance().DismissLoading();
            if (showToast)
            {
                if (e is HttpException)
                {
                    Log.E("e.Message: " + e.Message);
                }
                else if (e is WWWErrorException)
                {
                    ToastManager.GetInstance().CreatToast("网络连接失败，请检查网络连接");
                }else 
                {
                    ToastManager.GetInstance().CreatToast(e.Message);
                } 
            }
        });
    }

    public static IObservable<string> PostWithSign(string url, Dictionary<string, object> param, 
        string fileName = null, string filePath = null, bool showToast = true)
    {
        var formParam = new WWWForm();
        if (token.IsNullOrEmpty())
        {
            token = PlayerPrefsUtil.GetToken();
        }

        if (!token.IsNullOrEmpty())
        {
            param.Add("token", token);
        }

        if (param.Count > 0)
        {
            param.ForEach(pair => { formParam.AddField(pair.Key, pair.Value.ToString()); });
            string sign = SignUtil.getSign(param);
            formParam.AddField("sign", sign);
            formParam.AddField("appid", AppConst.APPID);
        }

        return ObservableWWW.Post(url, formParam).DoOnError(e =>
        {
            LoadingManager.GetInstance().DismissLoading();
            if (showToast)
            {
                if (e is HttpException)
                {
                 
                }
                else if (e is WWWErrorException)
                {
                    ToastManager.GetInstance().CreatToast("网络连接失败，请检查网络连接");
                }else 
                {
                    ToastManager.GetInstance().CreatToast(e.Message);
                }
            }
        });
    }

    public static IObservable<T> GetWithSign<T>(string url, Dictionary<string, object> param, bool showToast = true)
    {
        if (token.IsNullOrEmpty())
        {
            token = PlayerPrefsUtil.GetToken();
        }

        if (!token.IsNullOrEmpty())
        {
            param.Add("token", token);
        }

        var methodParams = SignUtil.getMethodParamStr(param);
        var sign = SignUtil.getSign(param);
        url = url + "?" + methodParams + "&sign=" + sign + "&appid=" + AppConst.APPID;
        return ObservableWWW.Get(url).Select<string, T>(s =>
        {
            Log.I("url==" + url + "\n网络请求返回内容\n" + s);
            BaseEntity<T> deserializeObject = JsonConvert.DeserializeObject<BaseEntity<T>>(s);
            if (deserializeObject.status != 1)
            {
                CommonUtil.error(deserializeObject.errCode, deserializeObject.message, deserializeObject.status);
                throw new HttpException(deserializeObject.errCode, deserializeObject.message, deserializeObject.status);
            }

            return deserializeObject.data;
        }).DoOnError(e =>
        {
            LoadingManager.GetInstance().DismissLoading();
            if (showToast)
            {
                if (e is HttpException)
                {
                    CommonUtil.toast(((HttpException)e).ErrMessage);
                }
                else if (e is WWWErrorException)
                {
                    ToastManager.GetInstance().CreatToast("网络连接失败，请检查网络连接");
                }else 
                {
                    ToastManager.GetInstance().CreatToast(e.Message);
                }    
            }
        });
    }

    public static IObservable<string> GetWithSign(string url, Dictionary<string, object> param, bool showToast = true)
    {
        if (token.IsNullOrEmpty())
        {
            token = PlayerPrefsUtil.GetToken();
        }

        if (!token.IsNullOrEmpty())
        {
            param.Add("token", token);
        }

        var methodParams = SignUtil.getMethodParamStr(param);
        var sign = SignUtil.getSign(param);
        url = url + "?" + methodParams + "&sign=" + sign + "&appid=" + AppConst.APPID;
        return ObservableWWW.Get(url).DoOnError(e =>
        {
            LoadingManager.GetInstance().DismissLoading();
            if (showToast)
            {
                if (e is HttpException)
                {
                 
                }
                else if (e is WWWErrorException)
                {
                    ToastManager.GetInstance().CreatToast("网络连接失败，请检查网络连接");
                }else 
                {
                    ToastManager.GetInstance().CreatToast(e.Message);
                }
            }
        });
    }

    public static IObservable<string> Get(string url, IProgress<float> progress = null)
    {
        return ObservableWWW.Get(url, null, progress);
    }

    public static IObservable<string> Post(string url, WWWForm content, IProgress<float> progress = null)
    {
        return ObservableWWW.Post(url, content, progress);
    }
}
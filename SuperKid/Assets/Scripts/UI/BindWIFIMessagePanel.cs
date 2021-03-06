//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SuperKid.Utils;
using UniRx;

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using QFramework;
    
    public class BindWIFIMessagePanelData : QFramework.UIPanelData
    {
    }
    
    public partial class BindWIFIMessagePanel : QFramework.UIPanel
    {
        [System.Runtime.InteropServices.DllImport( "__Internal" )]
        private static extern void _NativeGallery_GetSSID();
        
        private bool showWiFipwd = true;
        private bool saveWiFipwd = true;
        private ResLoader mResLoader = ResLoader.Allocate();
        private bool isSendMessage;

        private void GetWifiInfo()
        {
            if (isSendMessage)
            {
                NativeGallery.GetSomethingFromNative( ( json , action1) =>
                {
                    isSendMessage = false;
                    if (json.IsNotNullAndEmpty())
                    {
                        InputFieldSSID.text = json;
                        string pwd = PlayerPrefsUtil.GetWiFiPWD(json);
                        if (pwd.IsNotNullAndEmpty())
                        {
                            InputFieldPWD.text = pwd;
                        }
                        else
                        {
                            InputFieldPWD.text = string.Empty;
                        }
                    }
                    Log.I( "GetSomethingFromNative: " + json );
                }, (int) NativeAction.Location);
            }
            else
            {
                CancelInvoke("GetWifiInfo");
            }
        }
        
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
            throw new System.NotImplementedException ();
        }
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as BindWIFIMessagePanelData ?? new BindWIFIMessagePanelData();
            
            if (Application.platform == RuntimePlatform.Android)
            {
                NativeGallery.RequestPermission(( result, action ) =>
                {
                    isSendMessage = true;
                    Log.I( "RequestPermission: " + result );
                    if (result == (int) NativeGallery.Permission.Granted)
                    {
                        // InvokeRepeating("GetWifiInfo",1,1); 可以实时更新wifi，但无法修改wifi密码，暂时舍弃
                        NativeGallery.GetSomethingFromNative( ( json , action1) =>
                        {
                            if (json.IsNotNullAndEmpty())
                            {
                                isSSIDContains5G(json);
                                InputFieldSSID.text = json;
                                string pwd = PlayerPrefsUtil.GetWiFiPWD(json);
                                if (pwd.IsNotNullAndEmpty())
                                {
                                    InputFieldPWD.text = pwd;
                                }
                                else
                                {
                                    InputFieldPWD.text = string.Empty;
                                }
                            }
                            Log.I( "GetSomethingFromNative: " + json );
                        }, (int) NativeAction.Location);
                    }
                }, (int) NativeAction.Location);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (NativeGallery.RequestIPhonePermission(2) == NativeGallery.Permission.Granted)
                {
                    NativeGallery.GetSomethingFromIPhone(result =>
                    {
                        if (result.IsNotNullAndEmpty())
                        {
                            InputFieldSSID.text = result;
                            isSSIDContains5G(result);
                            string pwd = PlayerPrefsUtil.GetWiFiPWD(result);
                            if (pwd.IsNotNullAndEmpty())
                            {
                                InputFieldPWD.text = pwd;
                            }
                            else
                            {
                                InputFieldPWD.text = String.Empty;
                            }
                        }
                        
                    },1);
                }
            }
            
            var submit = Observable.Merge(
                InputFieldSSID.OnEndEditAsObservable().Where(_ =>  Input.GetKeyDown(KeyCode.Return)),
                BtnCommit.OnClickAsObservable().Select(_ => InputFieldSSID.text)
                );
            submit.Where(s => s != "")
                .Subscribe(s =>
                {
                    AudioManager.PlaySound("Button_Audio");
                    isSendMessage = false;
                    if (saveWiFipwd)
                    {
                        // 保存 WiFi ssid 和 pwd 到本地
                        PlayerPrefsUtil.SetWiFiSSIDPWD(InputFieldSSID.text,InputFieldPWD.text);
                    }
                    else
                    {
                        PlayerPrefsUtil.SetWiFiSSIDPWD(InputFieldSSID.text,String.Empty);
                    }
                    
                    Debug.Log("submit-确定 "+"ssid=" + s +"  pwd="+InputFieldPWD.text);
                    UIMgr.OpenPanel<BindDevicePanel>(new BindDevicePanelData()
                    {
                        pwdStr = InputFieldPWD.text,
                        ssidStr = InputFieldSSID.text
                    },UITransitionType.CIRCLE, this);
                }).AddTo(this);
            BtnClearPWD.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                InputFieldPWD.text = String.Empty;
            }).AddTo(this);
            BtnSSIDList.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
#if UNITY_ANDROID
                NativeGallery.RequestPermission(( result, action ) =>
                {
                    Log.I( "RequestPermission: " + result );
                    if (result == (int) NativeGallery.Permission.Granted)
                    {
                        NativeGallery.OpenWifiSettings();
                    }
                }, (int) NativeAction.Location);
#elif UNITY_IOS
                NativeGallery.OpenSettings();
#else
                
#endif

            });
            InputFieldSSID.OnValueChangedAsObservable().Subscribe((s =>
            {
                var texture2D = mResLoader.LoadSync<Texture2D>("ic_bindDetermine");
                if (s.Length > 0)
                {
                    texture2D = mResLoader.LoadSync<Texture2D>("ic_bindDetermine");
                }
                else
                {
                    texture2D = mResLoader.LoadSync<Texture2D>("btn_code_sel");
                }
                ImgDetermine.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);

            })).AddTo(this);

            BtnShow.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                var texture2D = mResLoader.LoadSync<Texture2D>("ic_savepwd");
                showWiFipwd = !showWiFipwd;
                if (showWiFipwd)
                {
                    texture2D = mResLoader.LoadSync<Texture2D>("btn_show_pwd");
                    InputFieldPWD.contentType = InputField.ContentType.EmailAddress;
                }
                else
                {                 
                    texture2D = mResLoader.LoadSync<Texture2D>("btn_hide_pwd");
                    InputFieldPWD.contentType = InputField.ContentType.Password;
                }

                var image = BtnShow.transform.Find("Image").GetComponent<Image>();
                image.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
                InputFieldPWD.MyUpdateLabel();
            }).AddTo(this);

            BtnConnectTips.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<BindCheckWIFIPanel>(new BindCheckWIFIPanelData(),UITransitionType.NULL);
            }).AddTo(this);

            BtnBack.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                Debug.Log("WiFi 返回");
                Back();
            }).AddTo(this);

            BtnSavepwd.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                Debug.Log("记住密码");
                saveWiFipwd = !saveWiFipwd;
                var texture2D = mResLoader.LoadSync<Texture2D>("ic_savepwd");
                if (saveWiFipwd)
                {
                    texture2D = mResLoader.LoadSync<Texture2D>("ic_savepwd");
                }
                else
                {
                    texture2D = mResLoader.LoadSync<Texture2D>("ic_unsavepwd");
                }
                ImageSavePWD.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
            }).AddTo(this);


            SimpleEventSystem.GetEvent<TipConfirmClick>()
                .Subscribe(_ =>
                {
                    if (_.GetAction == TipAction.Contains5GAlter)
                    {
                        if (Application.platform == RuntimePlatform.IPhonePlayer)
                        {
                            NativeGallery.RequestPermission(( result, action ) =>
                            {
                                Log.I( "RequestPermission: " + result );
                                if (result == (int) NativeGallery.Permission.Granted)
                                {
                                    NativeGallery.OpenWifiSettings();
                                }
                            }, (int) NativeAction.Location);
                        }
                        else if (Application.platform == RuntimePlatform.IPhonePlayer)
                        {
                            NativeGallery.OpenSettings();
                        }           
                    }
                }).AddTo(this);

            // please add init code here
        }

        private void isSSIDContains5G(string str)
        {
            if (str.Contains("5G") || str.Contains("5g") || str.Contains("5 g") || str.Contains("5 G"))
            {
                UIMgr.OpenPanel<TipPanel>(new TipPanelData()
                {
                    action = TipAction.Contains5GAlter,
                    message = "选择非5G的 WiFi 网络进行配网!",
                    strConfirm = "去设置",
                    strCancel = "取消",
                    strTitle = "配网账号",
                });
            }
        }
        
        protected override void OnOpen(QFramework.IUIData uiData)
        {
        }
        
        protected override void OnShow()
        {
        }
        
        protected override void OnHide()
        {
        }
        
        protected override void OnClose()
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
    }
}

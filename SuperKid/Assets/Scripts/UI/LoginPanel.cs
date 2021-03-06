//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using QFramework;
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
    using DG.Tweening;
    
    public class LoginPanelData : QFramework.UIPanelData
    {
        public int action;
    }
    
    public partial class LoginPanel : QFramework.UIPanel
    {

        private bool mIsHidePwd = true;
        private ResLoader mResLoader = ResLoader.Allocate();
        private float mTotalTime = 60;
        private Texture2D texture2DNor;
        private Texture2D texture2DSel;
        private string mBusType; // 1为登录忘记，2修改密码忘记
        private int action; // 1注册，2登录，3登录忘记密码，4设置忘记密码
        

        protected override void OnInit(QFramework.IUIData uiData)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidForUnity.CallAndroidHideSplash();
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                Dictionary<string, object> subParam = new Dictionary<string, object>();
                param.Add("method", IOSClientUtil.MainPanelOnInit);
                param.Add("params", subParam);
                IOSClientUtil.CommonMethodCallIOSClient(param.ToJson());
            }
            BtnClearPwd.gameObject.SetActive(false);
            BtnClearPhone.gameObject.SetActive(false);
            StartAnimation();
            mData = uiData as LoginPanelData ?? new LoginPanelData();
            action = mData.action;
            if (action == 4)
            {
                // 隐藏手机号输入 获取验证码
                PhonePanel.gameObject.SetActive(false);
                CodePanel.gameObject.SetActive(true);
                PwdPanel.gameObject.SetActive(false);
                BtnConfirm.gameObject.SetActive(false);
                BtnBack.gameObject.SetActive(true);
                mBusType = "2";
                StartResquestForSendSMS(mBusType);
            }
            else
            {
                PhonePanel.gameObject.SetActive(true);
                CodePanel.gameObject.SetActive(false);
                PwdPanel.gameObject.SetActive(false);
                BtnConfirm.gameObject.SetActive(true);
                BtnBack.gameObject.SetActive(false);
            }
            // please add init code here
            BtnBack.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                BackClick();
            });
            Observable.EveryUpdate().Where(_=>Input.GetKeyDown(KeyCode.Escape))
               .Subscribe(_ =>
               {
                   if (PhonePanel.gameObject.active)
                   {
                       Application.Quit();
                   }
                   else
                   {
                       BackClick();
                   }
               }).AddTo(this);
            BtnConfirm.onClick.AddListener(()=>
            {
                AudioManager.PlaySound("Button_Audio");
                if (PhonePanel.gameObject.active)
               {
                   if (InputPhone.text.IsNullOrEmpty())
                   {
                       Log.I("请输入手机号");
                       CommonUtil.toast("请输入手机号");
                   }
                   else
                   {
                       StartRequestForCheckIsRegister();
                   }
               } 
               else
               {
                   if (InputPwd.text.IsNullOrEmpty())
                   {
                       Log.I("请输入密码");
                       CommonUtil.toast("请输入密码");
                   } 
                   else if (!StringUtil.checkPassword(InputPwd.text))
                   {
                       Log.I("密码必须由至少8位的字母+数字组成");
                       CommonUtil.toast("密码必须由至少8位的字母+数字组成");
                   }
                   else
                   {
                       if(action == 1) {
                           StartResquestForDoRegister();
                       } 
                       else if(action == 2) 
                       {
                           StartResquestForDoLogin(InputPhone.text);
                       }
                       else if(action == 3) 
                       {
                           StartResquestForModifyPasswd();
                       } else if (action == 4)
                       {
                           StartResquestForDoUpdatePasswd();
                       }
                   }
               }
            });
            BtnCode.onClick.AddListener(()=>
            {
               Log.I("获取验证码 click");
               StartResquestForSendSMS(mBusType);
            });
            InputCode.onValueChange.AddListener(ChangeCode);
            InputPhone.onValueChange.AddListener(ChangePhone);
            InputPwd.onValueChange.AddListener(ChangePwd);
            BtnClearPhone.onClick.AddListener(() => { InputPhone.text = ""; });
            BtnClearPwd.onClick.AddListener(() => { InputPwd.text = ""; });
            BtnHideOrShow.onClick.AddListener(() =>
            {
               if (mIsHidePwd)
               {
                   var texture2D = mResLoader.LoadSync<Texture2D>("btn_show_pwd");
                   ImgHideOrShow.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
                   InputPwd.inputType = InputField.InputType.Standard;
                   mIsHidePwd = false;
                   InputPwd.MyUpdateLabel();
                   Log.I("显示" + InputPwd.text);
               }
               else
               {
                   var texture2D = mResLoader.LoadSync<Texture2D>("btn_hide_pwd");
                   ImgHideOrShow.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
                   InputPwd.inputType = InputField.InputType.Password;
                   mIsHidePwd = true;
                   InputPwd.MyUpdateLabel();
                   Log.I("隐藏" + InputPwd.text);
               }
               
            });
            BtnForget.onClick.AddListener(() =>
            {
                mBusType = "2";
                StartResquestForSendSMS(mBusType);
            });
            texture2DSel = mResLoader.LoadSync<Texture2D>("btn_code_sel");
            texture2DNor = mResLoader.LoadSync<Texture2D>("btn_code_nor");
        }

        private void StartAnimation()
        {
            ImgHyrogen.transform.DOMoveY(2f, 5f).SetLoops(-1,LoopType.Yoyo);
            ImgCloud1.transform.DOLocalMoveX(-372, 40f).SetLoops(-1,LoopType.Restart);
            Observable.Timer(TimeSpan.FromSeconds(10f))
                .Subscribe(_ =>             ImgCloud2.transform.DOLocalMoveX(-372, 40f).SetLoops(-1,LoopType.Restart)).AddTo(this);
            Observable.Timer(TimeSpan.FromSeconds(18f))
                .Subscribe(_ =>             ImgCloud3.transform.DOLocalMoveX(-372, 40f).SetLoops(-1,LoopType.Restart)).AddTo(this);
        }
        
        private void BackClick()
        {
            if (action == 4)
            {
                Back();
            }
            else
            {
                if (CodePanel.gameObject.active)
                {
                    if (action != 4)
                    {
                        PhonePanel.gameObject.SetActive(true);
                        CodePanel.gameObject.SetActive(false);
                        PwdPanel.gameObject.SetActive(false);
                        BtnConfirm.gameObject.SetActive(true);
                        BtnBack.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (UIManager.Instance.mAllUI.Count > 1)
                        {
                            Back();
                        }
                    }
                }
                else
                {
                    PhonePanel.gameObject.SetActive(true);
                    CodePanel.gameObject.SetActive(false);
                    PwdPanel.gameObject.SetActive(false);
                    BtnConfirm.gameObject.SetActive(true);
                    BtnBack.gameObject.SetActive(false);
                }
            }
        }


        void ChangeCode(string str)
        {
            if (!str.IsNullOrEmpty() && str.Length == 4)
            {
                StartResquestForCheckVerfyNo(str);
            }
        }
        
        void ChangePhone(string str)
        {
            if (!str.IsNullOrEmpty())
            {
                // BtnConfirm.enabled = true;
                BtnClearPhone.gameObject.SetActive(true);
            }
            else
            {
                // BtnConfirm.enabled = false;
                BtnClearPhone.gameObject.SetActive(false);
            }
        }
        
        void ChangePwd(string str)
        {
            if (!str.IsNullOrEmpty())
            {
                // BtnConfirm.enabled = true;
                BtnClearPwd.gameObject.SetActive(true);
            }
            else
            {
                // BtnConfirm.enabled = false;
                BtnClearPwd.gameObject.SetActive(false);
            }
        }
        
        private IEnumerator CountDown()
        {
            mTotalTime = 60;
            while (mTotalTime > 0)
            {
                yield return new WaitForSeconds(1);
                mTotalTime--;
                TextBtnCode.text = string.Format("{1:D2}s", (int)mTotalTime / 60, (int)mTotalTime % 60) + " 重新获取";
                BtnCode.image.sprite = Sprite.Create(texture2DSel, new Rect(0, 0, texture2DSel.width, texture2DSel.height), Vector2.one * 0.5f);
                BtnCode.enabled = false;
            }
            TextBtnCode.text = "获取验证码";
            BtnCode.image.sprite = Sprite.Create(texture2DNor, new Rect(0, 0, texture2DNor.width, texture2DNor.height), Vector2.one * 0.5f);
            BtnCode.enabled = true;
            StopCoroutine(CountDown());
        }

        private void StartRequestForCheckIsRegister()
        {
            BtnConfirm.enabled = false;
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("mobile", InputPhone.text);
            HttpUtil.GetWithSign<int>(UrlConst.CheckIsRegist, paramDict)
                .Subscribe(response =>
                    {
                        BtnConfirm.enabled = true;
                        if (response == 1)
                        {
                            action = 2;
                            TextPwdTitle.text = "输入登录密码";
                            BtnForget.gameObject.SetActive(true);
                            // 1-已经注册过 跳转输入登录密码页面 0-未注册过 发送验证码
                            PhonePanel.gameObject.SetActive(false);
                            CodePanel.gameObject.SetActive(false);
                            PwdPanel.gameObject.SetActive(true);
                            BtnConfirm.gameObject.SetActive(true);
                            BtnBack.gameObject.SetActive(true);
                        }
                        else
                        {
                            action = 1;
                            TextPwdTitle.text = "设置登录密码";
                            BtnForget.gameObject.SetActive(false);
                            mBusType = "1";
                            StartResquestForSendSMS(mBusType);
                        }
                    }
                    , e =>
                    {
                        BtnConfirm.enabled = true;
                        if (e is HttpException)
                        {
                            HttpException http = e as HttpException;
                            Log.E("弹吐司" + http.Message);
                        }
                    }).AddTo(this);
        }
        
        void StartResquestForSendSMS(string busType)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            if (action == 4)
            {
                if (PlayerPrefsUtil.UserInfo == null || PlayerPrefsUtil.UserInfo.mobile.IsNullOrEmpty())
                {
                    CommonUtil.toast("数据异常，请重新登录");
                    return;
                }
                paramDict.Add("mobile", PlayerPrefsUtil.UserInfo.mobile);
            }
            else
            {
                paramDict.Add("mobile", InputPhone.text);
            }
            paramDict.Add("busType", busType);
            HttpUtil.PostWithSign<object>(UrlConst.SendSMS, paramDict)
                .Subscribe(response =>
                    {
                        if (action == 2)
                        {
                            action = 3;
                        } 
                        PhonePanel.gameObject.SetActive(false);
                        CodePanel.gameObject.SetActive(true);
                        PwdPanel.gameObject.SetActive(false);
                        BtnConfirm.gameObject.SetActive(false);
                        BtnBack.gameObject.SetActive(true);
                        if (action == 4)
                        {
                            TextTip.text = "验证码已发送至手机号 " + PlayerPrefsUtil.UserInfo.mobile;
                        }
                        else
                        {
                            TextTip.text = "验证码已发送至手机号 " + InputPhone.text;
                        }
                        StartCoroutine(CountDown());
                    }
                    , e =>
                    {
                        if (e is HttpException)
                        {
                            HttpException http = e as HttpException;
                            Log.E("弹吐司" + http.Message);
                        }
                    }).AddTo(this);
        }
        
        void StartResquestForCheckVerfyNo(string verifyNo)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            if (action == 4)
            {
                if (PlayerPrefsUtil.UserInfo == null || PlayerPrefsUtil.UserInfo.mobile.IsNullOrEmpty())
                {
                    CommonUtil.toast("数据异常，请重新登录");
                    return;
                }
                paramDict.Add("mobile", PlayerPrefsUtil.UserInfo.mobile);
            }
            else
            {
                paramDict.Add("mobile", InputPhone.text);
            }
            paramDict.Add("verifyNo", verifyNo);
            HttpUtil.PostWithSign<object>(UrlConst.CheckVerfyNoValid, paramDict)
                .Subscribe(response =>
                    {
                        StopCoroutine(CountDown());
                        if (action == 2)
                        {
                            TextPwdTitle.text = "输入登录密码";
                            BtnForget.gameObject.SetActive(true);
                        }
                        else
                        {
                            TextPwdTitle.text = "设置登录密码";
                            BtnForget.gameObject.SetActive(false);
                        }
                        PhonePanel.gameObject.SetActive(false);
                        CodePanel.gameObject.SetActive(false);
                        PwdPanel.gameObject.SetActive(true);
                        BtnConfirm.gameObject.SetActive(true);
                        BtnBack.gameObject.SetActive(true);
                    }
                    , e =>
                    {
                        if (e is HttpException)
                        {
                            HttpException http = e as HttpException;
                            Log.E("弹吐司" + http.Message);
                        }
                    }).AddTo(this);
        }
        
        void StartResquestForDoLogin(string phone)
        {
            BtnConfirm.enabled = false;
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("mobile", phone);
            paramDict.Add("passwd", InputPwd.text);
            HttpUtil.PostWithSign<UserInfoModel>(UrlConst.Login, paramDict)
                .Subscribe(response =>
                    {
                        BtnConfirm.enabled = true;
                        PlayerPrefsUtil.SetPhone(phone);
                        PlayerPrefsUtil.SetPwd(InputPwd.text);
                        PlayerPrefsUtil.UserInfo = response;
                        if (response.babyInfoVo.IsNull())
                        {
                            // BabyInfoActivity
                            BtnConfirm.transform.DOLocalMoveX(806, 2f);
                            Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ =>
                            {
                                UIMgr.OpenPanel<BaByInfoPanel>(new BaByInfoPanelData(),UITransitionType.CIRCLE, this);
                            }).AddTo(this);
                        } 
                        else if (response.babyRelation.IsNullOrEmpty())
                        {
                            // BabyRelationShipActivity
                            BtnConfirm.transform.DOLocalMoveX(806, 2f);
                            Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ =>
                            {
                                UIMgr.OpenPanel<UserInfoPanel>(new UserInfoPanelData(),UITransitionType.CIRCLE, this);
                            }).AddTo(this);
                        }
                        else
                        {
                            BtnConfirm.transform.DOLocalMoveX(806, 2f);
                            CommonUtil.OpenCloudMain(this);
                        }
                    }
                    , e =>
                    {
                        BtnConfirm.enabled = true;
                        if (e is HttpException)
                        {
                            HttpException http = e as HttpException;
                            Log.E("弹吐司" + http.Message);
                        }
                    }).AddTo(this);
        }
        
        void StartResquestForDoRegister()
        {
            BtnConfirm.enabled = false;
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("mobile", InputPhone.text);
            paramDict.Add("passwd", InputPwd.text);
            HttpUtil.PostWithSign<UserInfoModel>(UrlConst.DoRegister, paramDict)
                .Subscribe(response =>
                    {
                        BtnConfirm.enabled = true;
                        PlayerPrefsUtil.SetPhone(InputPhone.text);
                        PlayerPrefsUtil.SetPwd(InputPwd.text);
                        PlayerPrefsUtil.UserInfo = response;
                        UIMgr.OpenPanel<BaByInfoPanel>(new BaByInfoPanelData(), UITransitionType.CIRCLE, this);
                    }
                    , e =>
                    {
                        BtnConfirm.enabled = true;
                        if (e is HttpException)
                        {
                            HttpException http = e as HttpException;
                            Log.E("弹吐司" + http.Message);
                        }
                    }).AddTo(this);
        }

        
        void StartResquestForModifyPasswd()
        {
            BtnConfirm.enabled = false;
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("mobile", InputPhone.text);
            paramDict.Add("passwd", InputPwd.text);
            HttpUtil.PostWithSign<object>(UrlConst.ModifyPasswd, paramDict)
                .Subscribe(response =>
                    {
                        BtnConfirm.enabled = true;
                        CommonUtil.toast("新密码设置成功");
                        PlayerPrefsUtil.SetPwd(InputPwd.text);
                        StartResquestForDoLogin(InputPhone.text);
                    }
                    , e =>
                    {
                        BtnConfirm.enabled = true;
                        if (e is HttpException)
                        {
                            HttpException http = e as HttpException;
                            Log.E("弹吐司" + http.Message);
                        }
                    }).AddTo(this);
        }
        
        void StartResquestForDoUpdatePasswd()
        {
            if (PlayerPrefsUtil.UserInfo == null || PlayerPrefsUtil.UserInfo.mobile.IsNullOrEmpty())
            {
                CommonUtil.toast("数据异常，请重新登录");
                return;
            }
            BtnConfirm.enabled = false;
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("userId", PlayerPrefsUtil.GetUserId());
            paramDict.Add("newPasswd", InputPwd.text);
            paramDict.Add("oldPasswd", "");
            HttpUtil.PostWithSign<object>(UrlConst.DoUpdatePasswd, paramDict)
                .Subscribe(response =>
                    {
                        BtnConfirm.enabled = true;
                        CommonUtil.toast("新密码设置成功");
                        PlayerPrefsUtil.SetPwd(InputPwd.text);
                        StartResquestForDoLogin(PlayerPrefsUtil.UserInfo.mobile);
                    }
                    , e =>
                    {
                        BtnConfirm.enabled = true;
                        if (e is HttpException)
                        {
                            HttpException http = e as HttpException;
                            Log.E("弹吐司" + http.Message);
                        }
                    }).AddTo(this);
        }

        protected override void OnClose()
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;
            ImgCloud1.DOKill();
            ImgCloud2.DOKill();
            ImgCloud3.DOKill();
            ImgHyrogen.DOKill();
            BtnConfirm.DOKill();
        }
    }
    
    
}


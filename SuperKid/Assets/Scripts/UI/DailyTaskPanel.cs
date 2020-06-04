//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SuperKid.Entity;
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
    
    public class DailyTaskPanelData : QFramework.UIPanelData
    {
        public int showMedal;
    }

    public class ChechSignInModel
    {
        
        public int attendanceCount;// 签到次数
        
        public int attendanceScoreConfig; // 签到获得积分配置
        public int isAttendance; // 今天是否已签到 1-是 0-否
        public int punchCount; // 打卡次数
        public int punchScoreConfig; // 打卡获得积分配置
        public int punchShareScoreConfig; // 打卡分享获得积分配置
        public int totalScore; // 当前宝宝总积分
            
        
    }
    
    
    public partial class DailyTaskPanel : QFramework.UIPanel
    {
        private ResLoader mResLoader = ResLoader.Allocate();
        private List<MedalModel.MedalBean> mMedalsLiast;

        private int isAttendance;
        
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
            throw new System.NotImplementedException ();
        }
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as DailyTaskPanelData ?? new DailyTaskPanelData();

            RequestDailyTask();
            RequestMedalList();

            if (mData.showMedal == 2)
            {
                ToggleDailyTask.isOn = false;
                ToggleDailyMedal.isOn = true;
            }
            else
            {
                ToggleDailyTask.isOn = true;
                ToggleDailyMedal.isOn = false;
            }
            
            BtnBack.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                Back();
            }).AddTo(this);
            BtnIntegralRule.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<IntegralDescripPanel>(new IntegralDescripPanelData()
                {
                    mMedalsLiast = this.mMedalsLiast
                },UITransitionType.CIRCLE);
            }).AddTo(this);

            BtnIntegral.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<IntegralDetailPanel>(new IntegralDetailPanelData()
                {
                    panelToggleType = 2,
                },UITransitionType.CIRCLE,this);
            }).AddTo(this);

            /// 签到规则
            BtnTaskRule.OnClickAsObservable().Subscribe(_ => {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<AttendanceRulePanel>(new AttendanceRulePanelData(),UITransitionType.CIRCLE, this);
            }).AddTo(this);
            // 勋章规则
            BtnMedalRule.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<IntegralDescripPanel>(new IntegralDescripPanelData()
                {
                    mMedalsLiast = this.mMedalsLiast
                },UITransitionType.CIRCLE);
            }).AddTo(this);

            BtnAttendance.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                Debug.Log("点击签到");
                RequestAttendance();
            }).AddTo(this);
            
            BtnPunch.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                Debug.Log("学习任务打卡 前往");
                // CommonUtil.toast("此功能尚未开放,敬请期待");
                
                // Back();
                UIMgr.OpenPanel<AttendanceSelectPanel>(new AttendanceSelectPanelData(),UITransitionType.NULL);
            }).AddTo(this);

            BtnExchange.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<GiftListPanel>(new GiftListPanelData(),UITransitionType.CIRCLE);
            }).AddTo(this);
        }

        /**
        *  每日任务请求
        */
        private void RequestDailyTask()
        {
            
            Dictionary<string, object> signInParamDict = new Dictionary<string, object>();
            signInParamDict.Add("babyId", PlayerPrefsUtil.GetBabyId());
            HttpUtil.GetWithSign<ChechSignInModel>(UrlConst.ChechIsSignIn,signInParamDict)
                .Subscribe(response =>
                    {
                        TextAttendanceScoreConfig.text = String.Format("x{0}",response.attendanceScoreConfig.ToString());
                        TextPunchScoreConfig.text = String.Format("x{0}",response.punchScoreConfig.ToString());
                        TextPunchScoreShareConfig.text = String.Format("x{0}",response.punchShareScoreConfig.ToString());
                        TextSignInDayNum.text = response.attendanceCount.ToString();
                        TextClockNum.text = response.punchCount.ToString();
                        TextIntegralNum.text = response.totalScore.ToString();
                        Debug.Log("------"+ response);

                        isAttendance = response.isAttendance;
                        var btnText = BtnAttendance.transform.Find("Text").GetComponent<Text>();

                        Color BtnTextColor;
                        // var texture2D = mResLoader.LoadSync<Texture2D>("ic_dailyClocksBtn");

                        if (isAttendance == 0)
                        {
                            ColorUtility.TryParseHtmlString("#FFFFFF", out BtnTextColor);
                            // texture2D = mResLoader.LoadSync<Texture2D>("ic_dailyClocksBtn");
                            btnText.text = "签到";
                            ImageAttendanceDone.gameObject.SetActive(false);
                        }
                        else
                        {
                            ColorUtility.TryParseHtmlString("#4A4A4A", out BtnTextColor);
                            // texture2D = mResLoader.LoadSync<Texture2D>("ic_dailyClocksBtn");
                            btnText.text = "已签到";
                            ImageAttendanceDone.gameObject.SetActive(true);
                        }

                        btnText.color = BtnTextColor;
                        // BtnAttendance.image.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);

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


        /**
         * 签到
         */
        private void RequestAttendance()
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("relBabyId", PlayerPrefsUtil.GetBabyId());
            paramDict.Add("creator", PlayerPrefsUtil.GetUserId());
            Debug.Log(paramDict);
            
            HttpUtil.PostWithSign<MedalModel>(UrlConst.AttendanceAdd, paramDict)
                .Subscribe(response =>
                    {
                        RequestDailyTask();
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
        
        /**
         * 勋章数据
         */
        private void RequestMedalList()
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("babyId", PlayerPrefsUtil.GetBabyId());
            Debug.Log(paramDict);
            
            HttpUtil.GetWithSign<MedalModel>(UrlConst.FindBabyAllMedalList, paramDict)
                .Subscribe(response =>
                    {
                        this.CreatMedalItem(response.medalList);
                        this.mMedalsLiast = response.medalList;
                        this.TextMedalNum.text = response.medalCount.ToString();
                        this.TextMedalShareNum.text = response.medalShareCount.ToString();
                        this.TextMedalStudyDaysNum.text = response.totalDays.ToString();
                        TextTaskShareNum.text = response.studyShareCount.ToString();

                        Debug.Log("------"+ response);
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

        /**
         * 展示 Medal 
         */
        private void CreatMedalItem(List<MedalModel.MedalBean> modelslist)
        {
            Debug.Log("初始化 Medal 相关信息 ");
            foreach (var model in modelslist)
            {
                mResLoader.LoadSync<GameObject>("ItemDailyMedalPanel")
                    .Instantiate()
                    .transform
                    .LocalIdentity()
                    .Parent(MedalScrollView.Find("Viewport").Find("Content"))
                    .LocalScale(1, 1, 1)
                    .ApplySelfTo(ItemDailyMedalPanel =>
                    {
                        var ImageBG = ItemDailyMedalPanel.transform.Find("ImageBG").GetComponent<Image>();
                        // 领取 btn
                        var BtnMedalReceived = ImageBG.transform.Find("BtnMedalReceived").GetComponent<Button>();
                        var BtnText = BtnMedalReceived.transform.Find("Text").GetComponent<Text>();
                        var TextDays = ImageBG.transform.Find("Image/GameObjectDays").Find("TextStudyDayNum")
                            .GetComponent<Text>();
                        var TextScore = ImageBG.transform.Find("Image/GameObjectIntegral").Find("TextMedalNum")
                            .GetComponent<Text>();
                        var ImgLockCover = ImageBG.transform.Find("ImageLockCover").GetComponent<Image>();
                        TextDays.text = model.medalDays.ToString();
                        TextScore.text = model.medalScore.ToString();
    
                        
                        var texture2D = mResLoader.LoadSync<Texture2D>("ic_dailyClocksBtn");

                        Color BtnTextColor;
                        if (model.isDraw == 1)
                        {
                            ColorUtility.TryParseHtmlString("#FFFFFF", out BtnTextColor);
                            texture2D = mResLoader.LoadSync<Texture2D>("ic_dailyClocksBtn");
                            BtnText.text = "已领取";
                            ImgLockCover.enabled = false;
                        }
                        else
                        {
                            ColorUtility.TryParseHtmlString("#4A4A4A", out BtnTextColor);
                            texture2D = mResLoader.LoadSync<Texture2D>("ic_dailyItemunReceive");
                            ImgLockCover.enabled = true;
                            BtnText.text = "未获得";
                        }

                        BtnText.color = BtnTextColor;
                        BtnMedalReceived.image.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);

                        
                        BtnMedalReceived.OnClickAsObservable().Subscribe(_ =>
                        {
                            if (model.isDraw == 1)
                            {
                                UIMgr.OpenPanel<MedalSharePanel>(new MedalSharePanelData()
                                {
                                    medalModel = model,
                                },UITransitionType.CIRCLE, this);
                            }
                            
                        }).AddTo(this);
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

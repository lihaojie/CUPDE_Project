//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using EnhancedUI.EnhancedScroller;
using QFramework;
using UniRx;

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using SuperKid.Entity;
    
    public class WorksWallPanelData : QFramework.UIPanelData
    {
        public int BoxId;
        public int BoxDay;
    }
    
    public partial class WorksWallPanel : QFramework.UIPanel
    {
        private List<WorksWall> records = new List<WorksWall>();
        private ResLoader mResLoader = ResLoader.Allocate();
        
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
            throw new System.NotImplementedException ();
        }
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as WorksWallPanelData ?? new WorksWallPanelData();
            BtnBack.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                Back(); 
            });
            BtnHistoryPhoto.gameObject.SetActive(false);
            if (mData.BoxId > 0)
            {
                TextTitle.text = "作品墙";
                BtnAttendanceAdd.onClick.AddListener(() =>
                {
                    AudioManager.PlaySound("Button_Audio");
                    UIMgr.OpenPanel<AttendanceMainPanel>(new AttendanceMainPanelData()
                    {
                        BoxId = mData.BoxId,
                        BoxDay = mData.BoxDay
                    }, UITransitionType.CIRCLE);
                    Close();
                });
                BtnAttendanceInstructions.onClick.AddListener(() =>
                {
                    AudioManager.PlaySound("Button_Audio");
                    UIMgr.OpenPanel<AttendanceInstructionsPanel>(new AttendanceInstructionsPanelData(), UITransitionType.CIRCLE);
                });
            }
            else
            {
                TextTitle.text = "我的作品墙";
                BtnAttendanceAdd.gameObject.SetActive(false);
                BtnAttendanceInstructions.gameObject.SetActive(false);
                BtnHistoryPhoto.onClick.AddListener(() =>
                {
                    AudioManager.PlaySound("Button_Audio");
                    UIMgr.OpenPanel<HistoricalPhotoWallPanel>(new HistoricalPhotoWallPanelData(), UITransitionType.CIRCLE);
                });
                RequestHistoryPhotoData();
            }
            StartRequestForWorksWall();
        }

        protected override void OnClose()
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;
            records.Clear();
            records = null;
        }

        private void RequestHistoryPhotoData()
        {
            
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("babyId", PlayerPrefsUtil.GetBabyId());
            Debug.Log(paramDict);
            HttpUtil.GetWithSign<List<PhotoWallModel>>(UrlConst.FindPhotoWall, paramDict)
                .Subscribe(response =>
                    {
                        if (response.Count > 0)
                        {
                            BtnHistoryPhoto.gameObject.SetActive(true);
                        }
                        else
                        {
                            BtnHistoryPhoto.gameObject.SetActive(false);
                        }
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
         * 作品墙列表
         */
        private void StartRequestForWorksWall()
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("babyId", PlayerPrefsUtil.UserInfo.relBabyId);
            if (mData.BoxDay > 0)
            {
                 paramDict.Add("boxDay", mData.BoxDay);
            }
            if (mData.BoxId > 0)
            { 
                paramDict.Add("relBoxId", mData.BoxId );
            }
            HttpUtil.PostWithSign<WorksWallModel>(UrlConst.WorksWall, paramDict)
                .Subscribe(response =>
                    {
                        if (records.IsNotNull())
                        {
                            records = response.records;
                            if (records.Count > 0)
                            {
                                NoDataConnect.gameObject.SetActive(false);
                            }
                            else
                            {
                                NoDataConnect.gameObject.SetActive(true);
                            }
                            for (var j = 0; j < records.Count;j++)
                            {
                                mResLoader.LoadSync<GameObject>("WorksWallItem")
                                    .Instantiate()
                                    .transform
                                    .Parent(Content)
                                    .Identity()
                                    .ApplySelfTo(self =>
                                    {
                                        self.GetComponent<WorksWallItem>().SetData(records[j]);
                                    }).Show();;
                            }
                        }
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
    }
}

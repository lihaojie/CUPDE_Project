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

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using QFramework;
    using UniRx;
    
    public class BookDayListPanelData : QFramework.UIPanelData
    {
        public int PlanId;
        public int Month;
    }
    
    public partial class BookDayListPanel : QFramework.UIPanel
    {
        private ResLoader mResLoader = ResLoader.Allocate();
        private GameObject mPlayer;
        private BoxModel BoxModel;
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as BookDayListPanelData ?? new BookDayListPanelData();
            PlayerPrefsUtil.SelectPlanId = mData.PlanId;
            string prefabName = "LionPrefab";
            string BGMusic = "XXQG_BG_Music";
            if (PlayerPrefsUtil.ContentModel.planId == 100099)
            {
                prefabName = "TongtongPrefab";
                BGMusic = "XXQG_BG_Music";
            }
            else if (PlayerPrefsUtil.ContentModel.subject == 2)
            {
                prefabName = "LionPrefab";
                BGMusic = "CN_BG_Music";
            }
            else if (PlayerPrefsUtil.ContentModel.subject == 1)
            {
                prefabName = "DinoPrefab";
                BGMusic = "EN_BG_Music";
            }
            AudioManager.SetMusicOn();
            AudioManager.PlayMusic(BGMusic);
            if (mData.Month != 0)
            {
                PlayerPrefsUtil.SaveBoxMonthIndex(mData.Month - 1);
            }
            // 3
            if (PlayerPrefsUtil.ContentModel.boxList.Count <= PlayerPrefsUtil.GetBoxMonthIndex())
            {
                PlayerPrefsUtil.SaveBoxMonthIndex(0);
            }
            var boxMonthIndex = PlayerPrefsUtil.GetBoxMonthIndex();
            Log.I("boxMonthIndex: " + boxMonthIndex);
            BoxModel = PlayerPrefsUtil.ContentModel.boxList[boxMonthIndex];
            Log.I("BoxModel.name: " + BoxModel.name);

            TvTitle.text = BoxModel.name;
            if (prefabName.IsNullOrEmpty())
            {
                CommonUtil.toast("prefabName为空");
            }
            mPlayer = mResLoader.LoadSync<GameObject>(prefabName)
                .Instantiate()
                .transform
                .Identity()
                .ApplySelfTo(self =>
                {
                    self.GetComponent<MasterController>().SetData(BoxModel);
                })
                .gameObject;
            BtnTitle.onClick.AddListener(()=> {     
                AudioManager.PlaySound("Button_Audio"); 
                BookMonthObj.IsShow.Value = !BookMonthObj.IsShow.Value; });
            BookMonthObj.transform.localPosition = new Vector3(0,GetFixed(Screen.height));
            BtnBookMonthBack.onClick.AddListener(() => {   
                AudioManager.PlaySound("Button_Audio");
                BookMonthObj.IsShow.Value = !BookMonthObj.IsShow.Value;});
            BtnTarget.onClick.AddListener(()=>
            {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<TargetDetailPanel>(new TargetDetailPanelData()
                {
                    BoxModel = BoxModel
                }, UITransitionType.CIRCLE, this);
            });
            BtnVideo.onClick.AddListener(() => { 
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<ListItemPanel>(new ListItemPanelData()
                {
                    FunTag = ItemType.VIDEO
                }, UITransitionType.CIRCLE, this);
                
            });
            BtnAudio.onClick.AddListener(() => {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<ListItemPanel>(new ListItemPanelData()
                {
                    FunTag = ItemType.AUDIO
                }, UITransitionType.CIRCLE, this);
            });
            BtnBook.onClick.AddListener(() => {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<ListItemPanel>(new ListItemPanelData()
                {
                    FunTag = ItemType.PICBOOK
                }, UITransitionType.CIRCLE, this);
            });
            SimpleEventSystem.GetEvent<CanvasAnimationFinish>().Subscribe(_ =>
                {
                    UIMgr.OpenPanel<BookDayPanel>(new BookDayPanelData()
                    {
                        BoxDayDetailModel = mPlayer.GetComponent<MasterController>().SelectDayDetailModel
                    }, UITransitionType.CIRCLE, this);
                }).AddTo(this);
            BtnBookDayListBack.onClick.AddListener(() => {AudioManager.PlaySound("Button_Audio"); Back();});
            Observable.EveryUpdate().Where(_=>Input.GetKeyDown(KeyCode.Escape))
                .Subscribe(_ =>
                {
                    if (BookMonthObj.IsShow.Value)
                    {
                        BookMonthObj.IsShow.Value = false;
                        return;
                    }
                    Back();
                }).AddTo(this);
            BookMonthObj.UpdateItemDate();
        }
        
        protected override void OnClose()
        {
            AudioManager.SetMusicOff();
            mPlayer.DestroySelf();
            mPlayer = null;
            if (mResLoader.IsNotNull())
            {
                mResLoader.Recycle2Cache();
                mResLoader = null;
            }

            BoxModel = null;
        }

        public void UpdateMonth()
        {
            SimpleEventSystem.Publish(new CanvasCanMove());
            var boxMonthIndex = PlayerPrefsUtil.GetBoxMonthIndex();
            BoxModel = PlayerPrefsUtil.ContentModel.boxList[boxMonthIndex];
            TvTitle.text = BoxModel.name;
            mPlayer.GetComponent<MasterController>().SetData(BoxModel);
        }
        
        //Screen坐标值适配Canvas画布
        public static float GetFixed(float value)
        {
            if (UIManager.Instance.GetCanvasScaler().matchWidthOrHeight == 0)
                //匹配宽度时仅按照宽度计算
                return value * UIManager.Instance.GetCanvasScaler().referenceResolution.x / Screen.width;
            else
                //匹配高度时仅按照高度计算
                return value * UIManager.Instance.GetCanvasScaler().referenceResolution.y / Screen.height;
        }
    }
}

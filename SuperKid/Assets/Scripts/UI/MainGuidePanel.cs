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

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    public class MainGuidePanelData : QFramework.UIPanelData
    {
    }
    
    public partial class MainGuidePanel : QFramework.UIPanel
    {
        private ResLoader mResLoader = ResLoader.Allocate();
        private GameObject mMainAnimationGo;
        private MainAnimation mMainAnimation;
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
             
        }
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as MainGuidePanelData ?? new MainGuidePanelData();
            mMainAnimationGo = mResLoader.LoadSync<GameObject>("MainAnimation")
                .Instantiate()
                .transform
                .LocalScale(1.35475f,1.35475f,1.35475f)
                .Position(-11.436f,-5.028f,0)
                .ApplySelfTo((self) =>
                {
                    mMainAnimation =self.GetComponent<MainAnimation>();

                })
                .gameObject;
            mMainAnimation.LionNotify();
            ImgChineseNotify.onClick.AddListener(() =>
            {
                ImgChineseNotify.gameObject.SetActive(false);
                mMainAnimation.DionNotify();
                ImgEnglishNotify.gameObject.SetActive(true);
            });
            ImgEnglishNotify.onClick.AddListener(() =>
            {
                ImgEnglishNotify.gameObject.SetActive(false);
                mMainAnimation.TongTongNotify();
                ImgLearningNotify.gameObject.SetActive(true);
            });
            ImgWifiNotify.onClick.AddListener(() =>
            {
                SimpleEventSystem.Publish(new MainPanelGuideDismiss());
                Back();
            });
            ImgLearningNotify.onClick.AddListener(() =>
            {
                ImgLearningNotify.gameObject.SetActive(false);
                mMainAnimation.WifiNotify();
                ImgWifiNotify.gameObject.SetActive(true);
            });
        }
        
        protected override void OnClose()
        {
            mMainAnimationGo.DestroySelf();
            mMainAnimationGo = null;
            mResLoader.Recycle2Cache();
            mResLoader = null;
            mMainAnimation = null;
            PlayerPrefsUtil.SetFirstIn(false);
        }
    }
}

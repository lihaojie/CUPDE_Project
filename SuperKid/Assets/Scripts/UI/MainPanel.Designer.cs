//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    // Generate Id:62356146-2f1a-4a24-8ac5-bdacfc6b0686
    public partial class MainPanel
    {
        
        public const string NAME = "MainPanel";
        
        [SerializeField()]
        public UnityEngine.UI.Image ImgGoldBg;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnGold;
        
        [SerializeField()]
        public UnityEngine.UI.Text TvGold;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnIntegral;
        
        [SerializeField()]
        public UnityEngine.UI.Text TvMedal;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnMedal;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnRobot;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnChineseShop;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnEnglishShop;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnLearningShop;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImgChineseNoBind;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImgEnglishNoBind;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImgLearningNoBind;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnAddRobot;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnListen;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnGift;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnPhotoWall;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnMessage;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnScan;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnSetting;
        
        [SerializeField()]
        public CircleImage ImageHead;
        
        [SerializeField()]
        public UnityEngine.UI.Text TvUserName;
        
        [SerializeField()]
        public UnityEngine.UI.Text TvUserAge;
        
        private MainPanelData mPrivateData = null;
        
        public MainPanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new MainPanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            ImgGoldBg = null;
            BtnGold = null;
            TvGold = null;
            BtnIntegral = null;
            TvMedal = null;
            BtnMedal = null;
            BtnRobot = null;
            BtnChineseShop = null;
            BtnEnglishShop = null;
            BtnLearningShop = null;
            ImgChineseNoBind = null;
            ImgEnglishNoBind = null;
            ImgLearningNoBind = null;
            BtnAddRobot = null;
            BtnListen = null;
            BtnGift = null;
            BtnPhotoWall = null;
            BtnMessage = null;
            BtnScan = null;
            BtnSetting = null;
            ImageHead = null;
            TvUserName = null;
            TvUserAge = null;
            mData = null;
        }
    }
}

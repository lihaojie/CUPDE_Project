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
    
    
    // Generate Id:eecbe9c3-3243-4dac-bbfe-0e20b1c878d9
    public partial class MedalSharePanel
    {
        
        public const string NAME = "MedalSharePanel";
        
        [SerializeField()]
        public UnityEngine.UI.Image ImgWaterMask;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnBack;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImgContent;
        
        [SerializeField()]
        public UnityEngine.UI.Text TextStudyDayNum;
        
        [SerializeField()]
        public UnityEngine.UI.Text TextDays;
        
        [SerializeField()]
        public UnityEngine.UI.Text TextMedalName;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnSaveAlbum;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnShareWeChatSession;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnShareWeChatTimeLine;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImageQRCode;
        
        [SerializeField()]
        public UnityEngine.UI.Text TextName;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImageUserIcon;
        
        private MedalSharePanelData mPrivateData = null;
        
        public MedalSharePanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new MedalSharePanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            ImgWaterMask = null;
            BtnBack = null;
            ImgContent = null;
            TextStudyDayNum = null;
            TextDays = null;
            TextMedalName = null;
            BtnSaveAlbum = null;
            BtnShareWeChatSession = null;
            BtnShareWeChatTimeLine = null;
            ImageQRCode = null;
            TextName = null;
            ImageUserIcon = null;
            mData = null;
        }
    }
}

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
    
    
    // Generate Id:203c8f1f-8478-4db8-bff3-61c2c75ea33b
    public partial class MainGuidePanel
    {
        
        public const string NAME = "MainGuidePanel";
        
        [SerializeField()]
        public UnityEngine.UI.Button ImgChineseNotify;
        
        [SerializeField()]
        public UnityEngine.UI.Button ImgEnglishNotify;
        
        [SerializeField()]
        public UnityEngine.UI.Button ImgLearningNotify;
        
        [SerializeField()]
        public UnityEngine.UI.Button ImgWifiNotify;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImgChineseNoBind;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImgEnglishNoBind;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImgLearningNoBind;
        
        private MainGuidePanelData mPrivateData = null;
        
        public MainGuidePanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new MainGuidePanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            ImgChineseNotify = null;
            ImgEnglishNotify = null;
            ImgLearningNotify = null;
            ImgWifiNotify = null;
            ImgChineseNoBind = null;
            ImgEnglishNoBind = null;
            ImgLearningNoBind = null;
            mData = null;
        }
    }
}

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
    
    
    // Generate Id:36ddc5b7-7fe0-4bb5-8958-058a9bcf70f9
    public partial class BindDevicePanel
    {
        
        public const string NAME = "BindDevicePanel";
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnBack;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnStartBind;
        
        private BindDevicePanelData mPrivateData = null;
        
        public BindDevicePanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new BindDevicePanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            BtnBack = null;
            BtnStartBind = null;
            mData = null;
        }
    }
}

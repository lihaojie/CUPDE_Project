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
    
    
    // Generate Id:834bde9e-55d5-4650-8995-7b4938870f75
    public partial class FamilySettingPanel
    {
        
        public const string NAME = "FamilySettingPanel";
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnCancel;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnConfirm;
        
        private FamilySettingPanelData mPrivateData = null;
        
        public FamilySettingPanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new FamilySettingPanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            BtnCancel = null;
            BtnConfirm = null;
            mData = null;
        }
    }
}

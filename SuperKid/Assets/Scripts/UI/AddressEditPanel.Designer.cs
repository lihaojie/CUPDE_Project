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
    
    
    // Generate Id:25772744-930a-4b2b-a094-e14241b1c199
    public partial class AddressEditPanel
    {
        
        public const string NAME = "AddressEditPanel";
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnBack;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnDelete;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnCommit;
        
        [SerializeField()]
        public UnityEngine.UI.Toggle Toggle;
        
        [SerializeField()]
        public UnityEngine.UI.InputField InpName;
        
        [SerializeField()]
        public UnityEngine.UI.InputField InpMobile;
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnAddress;
        
        [SerializeField()]
        public UnityEngine.UI.Text TextAddress;
        
        [SerializeField()]
        public UnityEngine.UI.InputField InpAddressDetail;
        
        private AddressEditPanelData mPrivateData = null;
        
        public AddressEditPanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new AddressEditPanelData());
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
            BtnDelete = null;
            BtnCommit = null;
            Toggle = null;
            InpName = null;
            InpMobile = null;
            BtnAddress = null;
            TextAddress = null;
            InpAddressDetail = null;
            mData = null;
        }
    }
}

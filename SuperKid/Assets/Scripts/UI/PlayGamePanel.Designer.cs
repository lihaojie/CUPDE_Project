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
    
    
    // Generate Id:f7b79052-b805-4d3a-953f-acc3ae9b9b87
    public partial class PlayGamePanel
    {
        
        public const string NAME = "PlayGamePanel";
        
        [SerializeField()]
        public UnityEngine.UI.Button BtnBack;
        
        [SerializeField()]
        public UnityEngine.UI.Image ImageContent;
        
        private PlayGamePanelData mPrivateData = null;
        
        public PlayGamePanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new PlayGamePanelData());
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
            ImageContent = null;
            mData = null;
        }
    }
}

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
using SuperKid.Utils;

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    public class ChoosePhotoPanelData : QFramework.UIPanelData
    {
        public ChoosePhotoAction action;
        public bool showTip;
    }
    
    public partial class ChoosePhotoPanel : QFramework.UIPanel
    {
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
            throw new System.NotImplementedException ();
        }
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as ChoosePhotoPanelData ?? new ChoosePhotoPanelData();
            // please add init code here
            BtnCancel.onClick.AddListener(()=>{
                Back();
            });
            BtnAlbum.onClick.AddListener(()=>{
                SimpleEventSystem.Publish(new ChoosePhotoClick(mData.action, NativeAction.Album));
                Back();
            });
            BtnCamera.onClick.AddListener(()=>{
                SimpleEventSystem.Publish(new ChoosePhotoClick(mData.action, NativeAction.Camera));
                Back();
            });
            if (mData.showTip)
            {
                TextTip.gameObject.SetActive(true);
                TextCamera.text = "拍摄";
            }
            else
            {
                TextTip.gameObject.SetActive(false);
                TextCamera.text = "拍照";
            }
            
        }
        
        protected override void OnOpen(QFramework.IUIData uiData)
        {
        }
        
        protected override void OnShow()
        {
        }
        
        protected override void OnHide()
        {
        }
        
        protected override void OnClose()
        {
        }
    }
}

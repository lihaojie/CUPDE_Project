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
using SuperKid.Entity;
using SuperKid.Utils;
using UniRx;

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    public class PlayGamePanelData : QFramework.UIPanelData
    {
        public DayActionModel DayActionModel;
    }
    
    public partial class PlayGamePanel : QFramework.UIPanel
    {
        
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
            throw new System.NotImplementedException ();
        }
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as PlayGamePanelData ?? new PlayGamePanelData();
            // please add init code here
            BtnBack.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                Back();
            });
            if (mData.DayActionModel.introPicStr.IsNotNullAndEmpty())
            {
                ImageDownloadUtils.Instance.SetAsyncImage(mData.DayActionModel.introPicStr, ImageContent, true);
            }
            // RectTransform rect = ImageContent.GetComponent<RectTransform>();
            // rect.sizeDelta = new Vector2( 500, 2000);
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

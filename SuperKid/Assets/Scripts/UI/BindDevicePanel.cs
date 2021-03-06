//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using UniRx;

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using QFramework;
    using Entity;
    using Utils;
    
    public class BindDevicePanelData : QFramework.UIPanelData
    {
        public string ssidStr = String.Empty;
        public string pwdStr = String.Empty;
    }



    public partial class BindDevicePanel : QFramework.UIPanel
    {

        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as BindDevicePanelData ?? new BindDevicePanelData();
            BtnBack.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                Back();
            }).AddTo(this);
            
            BtnStartBind.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<ConnectDevicePanel>(new ConnectDevicePanelData()
                {
                    SSIDStr = mData.ssidStr,
                    SSIDPWD = mData.pwdStr,
                },UITransitionType.CIRCLE,this);
            }).AddTo(this);
        }

        protected override void OnClose()
        {
        }
    }

    public class BindResultModel
    {
        public int bindRelation;
    }
}

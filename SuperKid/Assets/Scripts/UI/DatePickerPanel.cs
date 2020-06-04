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

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    public class DatePickerPanelData : QFramework.UIPanelData
    {
        public string birthday;
    }
    
    public partial class DatePickerPanel : QFramework.UIPanel
    {
        
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as DatePickerPanelData ?? new DatePickerPanelData();
            // please add init code here
            // PickerPanel.Init(DateTime.Now, delegate
            // {
            //     Log.I(PickerPanel._selectDate.ToString());
            // } );
            PickerPanel.Init(DateTime.Parse(mData.birthday));
            BtnCancel.onClick.AddListener(()=>{
                Back();
            });
            BtnConfirm.onClick.AddListener(()=>{
                SimpleEventSystem.Publish(new SelectPickDate(PickerPanel._selectDate.ToString("yyyy-MM-dd")));
                Back();
            });
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

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
using UniRx;

namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    public class CollectGiftBoxPanelData : QFramework.UIPanelData
    {
        // 1-扫码计划类型  2-积分兑换类型
        public CollectGiftBoxType Type = CollectGiftBoxType.Exchange;
    }
    
    public partial class CollectGiftBoxPanel : QFramework.UIPanel
    {

        private ResLoader mResLoader = ResLoader.Allocate();
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as CollectGiftBoxPanelData ?? new CollectGiftBoxPanelData();
            BtnCheck.onClick.AddListener(() =>
            {
                SimpleEventSystem.Publish(new CollectGiftBoxPanelClosed(mData.Type));
                Back();
            });
            if (mData.Type == CollectGiftBoxType.Exchange)
            {
                var texture2D = mResLoader.LoadSync<Texture2D>("text_exchange_succee");
                ImgSuccess.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
                
                var giftTexture2D = mResLoader.LoadSync<Texture2D>("text_baby_exclusive_gift");
                ImgName.sprite = Sprite.Create(giftTexture2D, new Rect(0, 0, giftTexture2D.width, giftTexture2D.height), Vector2.one * 0.5f);

            }else if (mData.Type == CollectGiftBoxType.ChineseLearningPlan)
            {
                var texture2D = mResLoader.LoadSync<Texture2D>("text_received_succee");
                ImgSuccess.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
                var giftTexture2D = mResLoader.LoadSync<Texture2D>("text_chinese_learning_plan");
                ImgName.sprite = Sprite.Create(giftTexture2D, new Rect(0, 0, giftTexture2D.width, giftTexture2D.height), Vector2.one * 0.5f);

            }else if (mData.Type == CollectGiftBoxType.EnglishLearningPlan)
            {
                var texture2D = mResLoader.LoadSync<Texture2D>("text_received_succee");
                ImgSuccess.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
                var giftTexture2D = mResLoader.LoadSync<Texture2D>("text_english_learning_plan");
                ImgName.sprite = Sprite.Create(giftTexture2D, new Rect(0, 0, giftTexture2D.width, giftTexture2D.height), Vector2.one * 0.5f);
            }
        }
        
        protected override void OnClose()
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
    }
    
    public enum CollectGiftBoxType{
        Exchange,
        ChineseLearningPlan,
        EnglishLearningPlan
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// using NUnit.Framework.Internal;

using SuperKid.Entity;
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
    using UniRx;
    
    public class GiftListPanelData : QFramework.UIPanelData
    {
    }


    public partial class GiftListPanel : QFramework.UIPanel
    {
        
        private ResLoader mResLoader = ResLoader.Allocate();

        
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
            throw new System.NotImplementedException ();
        }

        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as GiftListPanelData ?? new GiftListPanelData();
            
            RequestGoodList();

            BtnBack.OnClickAsObservable().Subscribe(_ => {
                Back();
                AudioManager.PlaySound("Button_Audio");
                
            }).AddTo(this);
            BtnIntegral.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                UIMgr.OpenPanel<IntegralDetailPanel>(new IntegralDetailPanelData(),UITransitionType.CIRCLE, this);
            }).AddTo(this); 
        }
        
        
        /**
         * 积分列表数据
         */
        private void RequestGoodList()
        {
            
            Dictionary<string, object> paramDict = new Dictionary<string, object>();

            paramDict.Add("babyId", PlayerPrefsUtil.GetBabyId());
            Debug.Log(paramDict);
            
            HttpUtil.GetWithSign<List<GoodMedal>>(UrlConst.GoodsList, paramDict)
                .Subscribe(response =>
                    {
                        CreatGoodsItem(response);
                        Debug.Log("------"+ response);
                    }
                    , e =>
                    {
                        if (e is HttpException)
                        {
                            HttpException http = e as HttpException;
                            Log.E("弹吐司" + http.Message);
                        }
                    }).AddTo(this);
        }

        
        /**
         * 展示 商品
         */
        private void CreatGoodsItem(List<GoodMedal> modelslist)
        {
            Debug.Log("初始化 Medal 相关信息 ");
            foreach (var model in modelslist)
            {
                mResLoader.LoadSync<GameObject>("ItemGiftListPanel")
                    .Instantiate()
                    .transform
                    .LocalIdentity()
                    .Parent(GiftScrollView.transform.Find("Viewport").Find("Content"))
                    .LocalScale(1, 1, 1)
                    .ApplySelfTo(ItemGiftListPanel =>
                    {
                        var GoodItemBtn = ItemGiftListPanel.transform.Find("BtnItem").GetComponent<Button>();
                        GoodItemBtn.enabled = false;
                        var ImgGoodIcon = GoodItemBtn.transform.Find("ImageMask/ImgGiftIcon").GetComponent<Image>();
                        var TextGoodsName = GoodItemBtn.transform.Find("TextGiftName").GetComponent<Text>();

                        var TextGoodsPrice = ItemGiftListPanel.transform.Find("ImagePrice/TextPrice").GetComponent<Text>();
                        var TextExchangePrice = ItemGiftListPanel.transform.Find("ButtonIntegral/TextIntegralCount").GetComponent<Text>();
                        var BtnConvert = ItemGiftListPanel.transform.Find("ButtonConvert").GetComponent<Button>();
                        var BtnConvertText = BtnConvert.transform.Find("Text").GetComponent<Text>();
                        
                        
                        ImageDownloadUtils.Instance.SetAsyncImage(model.icon, ImgGoodIcon);
                        TextGoodsName.text = model.name;
                        TextGoodsPrice.text = String.Format("价值¥{0}",(model.price / 100).ToString());
                        TextExchangePrice.text = model.exchangePrice.ToString();


                        var texture2D = mResLoader.LoadSync<Texture2D>("ic_dailyItemReceive");
                        
                        if (PlayerPrefsUtil.UserInfo.babyInfoVo.totalScore > model.exchangePrice)
                        {
                            texture2D = mResLoader.LoadSync<Texture2D>("ic_dailyItemReceive");
                            BtnConvertText.text = "兑换";
                            BtnConvert.enabled = true;
                        }
                        else
                        {
                            
                            texture2D = mResLoader.LoadSync<Texture2D>("ic_dailyItemDontReceive");
                            BtnConvertText.text = "积分不足";
                            BtnConvert.enabled = false;
                        }

                        BtnConvert.image.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);

                        
                        BtnConvert.OnClickAsObservable().Subscribe(_ =>
                        {
                            AudioManager.PlaySound("Button_Audio");
                            UIMgr.OpenPanel<CreateOrderPanel>(new CreateOrderPanelData()
                            {
                                mGoodModel = model
                            },UITransitionType.CIRCLE, this);
                            
                        }).AddTo(this);
                    });
            }
            
        }

        protected override void OnClose()
        {
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
    }
}

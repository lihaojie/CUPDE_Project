//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using SuperKid.Entity;
using SuperKid.Utils;
using UniRx;
using UnityEditor;
using UnityEngine.Experimental.PlayerLoop;
using SuperKid.Utils;
using UniRx;
using System.IO;
using QFramework;
using SuperKid.Utils;
using UniRx;
using UnityEngine.Events;


namespace SuperKid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using System.IO;
    
    
    public class MedalSharePanelData : QFramework.UIPanelData
    {
        // public int medalGrade;
        // public int medalScore;
        // public int shareScore;
        public MedalModel.MedalBean medalModel;

    }
    
    public partial class MedalSharePanel : QFramework.UIPanel
    {

        private ResLoader mResLoader = ResLoader.Allocate();

        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
            throw new System.NotImplementedException ();
        }
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as MedalSharePanelData ?? new MedalSharePanelData();

            TextDays.text = String.Format("连续{0}日",mData.medalModel.medalDays.ToString());
            TextMedalName.text = String.Format("\"{0}勋章\"",mData.medalModel.medalName);
            TextName.text = PlayerPrefsUtil.UserInfo.babyInfoVo.name;
            
            Texture2D mTexture2DHBoy = mResLoader.LoadSync<Texture2D>("ic_head_boy");
            Texture2D mTexture2DHGirl = mResLoader.LoadSync<Texture2D>("ic_head_girl");
            if (PlayerPrefsUtil.UserInfo.babyInfoVo.babyLogoUrl.IsNotNullAndEmpty())
            {
                ImageDownloadUtils.Instance.SetAsyncImage(PlayerPrefsUtil.UserInfo.babyInfoVo.babyLogoUrl, ImageUserIcon);

            } 
            else if (PlayerPrefsUtil.UserInfo.babyInfoVo.sex == 1)
            {
                ImageUserIcon.sprite = Sprite.Create(mTexture2DHBoy,
                    new Rect(0, 0, mTexture2DHBoy.width, mTexture2DHBoy.height), Vector2.one * 0.5f);
            }
            else if (PlayerPrefsUtil.UserInfo.babyInfoVo.sex == 2)
            {
                ImageUserIcon.sprite = Sprite.Create(mTexture2DHGirl,
                    new Rect(0, 0, mTexture2DHGirl.width, mTexture2DHGirl.height), Vector2.one * 0.5f);
            }
            TextStudyDayNum.text = mData.medalModel.medalDays.ToString();
            
            // please add init code here
            BtnBack.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                Back();
            }).AddTo(this);
            if (mData.medalModel.isDraw != 1)
            {
                CommonUtil.toast("领取成功+" + mData.medalModel.medalScore + "积分");
            }
            
            BtnSaveAlbum.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                Debug.Log("保存到相册-------");
                // Rect mRect = ImgContent.transform.GetComponent<RectTransform>().rect; 
                // this.ScreenShot(mRect);
                StartResquestForSharePoster(ShareAction.Save);
            }).AddTo(this);

            BtnShareWeChatSession.OnClickAsObservable().Subscribe(_ =>
            {
                AudioManager.PlaySound("Button_Audio");
                StartResquestForSharePoster(ShareAction.Wechat);
                Debug.Log("分享到微信");
            }).AddTo(this);
            
            BtnShareWeChatTimeLine.OnClickAsObservable().Subscribe(_ =>
            { //WechatMoments
                AudioManager.PlaySound("Button_Audio");
                StartResquestForSharePoster(ShareAction.WechatMoments);
                Debug.Log("分享到朋友圈");
            }).AddTo(this);
            SimpleEventSystem.GetEvent<ShareResult>().Subscribe(res =>
            {
                //1成功2失败3取消
                string status = res.Status;
                if ("1".Equals(status))
                {
                    if (mData.medalModel.isShare == 0)
                    {
                        CommonUtil.toast("分享成功+" + mData.medalModel.medalShareScore + "积分");
                        mData.medalModel.isShare = 1;
                    }
                    StartResquestForAddRecord();
                }
                else if ("2".Equals(status))
                {
                }
                else if ("3".Equals(status))
                {
                }

            }).AddTo(this);
        }
        
        private void StartResquestForSharePoster(ShareAction shareAction)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("babyId", PlayerPrefsUtil.GetBabyId());
            paramDict.Add("shareType", 2);
            paramDict.Add("medalGrade", mData.medalModel.medalGrade);
            HttpUtil.PostWithSign<string>(UrlConst.SharePoster, paramDict)
                .Subscribe(response =>
                    {
                        Log.I("图片地址： " + response);
                        if (shareAction == ShareAction.Save)
                        {
                            if (Application.platform == RuntimePlatform.Android)
                            {
                                NativeGallery.RequestPermission((result, action) =>
                                {
                                    if (result == (int) NativeGallery.Permission.Granted)
                                    {
                                        AndroidForUnity.CallAndroidForSavePicToAlbum(response);
                                        CommonUtil.toast("保存成功");
                                    }
                                }, (int) NativeAction.Album);
                            }
                            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                            {
                                IOSClientUtil.SaveImageToAlubmCallIOSClient(response);
                            }
                        } 
                        else if (shareAction == ShareAction.Wechat)
                        {
                            if (Application.platform == RuntimePlatform.Android)
                            {
                                ShareModel shareModel = new ShareModel();
                                shareModel.imageUrl = response;
                                shareModel.platformName = AppConst.SHARE_WECHAT;
                                shareModel.type = AppConst.SHARE_IMAGE_TYPE;
                                shareModel.title = AppConst.SHARE_TITLE;
                                if (mData.medalModel.isShare == 0)
                                {
                                    shareModel.toastMessage = "分享成功+" + mData.medalModel.medalShareScore + "积分";
                                }
                                AndroidForUnity.CallAndroidForShare(SerializeHelper.ToJson(shareModel));
                            }
                            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                            {
                                Dictionary<string,object> sharParmsDic = new Dictionary<string, object>();
                                sharParmsDic.Add("platformType","WechatSession");
                                sharParmsDic.Add("shareType","image");
                                sharParmsDic.Add("image",response);
                                if (mData.medalModel.isShare == 0)
                                {
                                    string shareToastMessage = "分享成功+" + mData.medalModel.medalShareScore + "积分";
                                    sharParmsDic.Add("shareToastMsg",shareToastMessage);
                                }
                                IOSClientUtil.ShareObjectCallIOSClient(SerializeHelper.ToJson(sharParmsDic));
                            }
                        }
                        else if (shareAction == ShareAction.WechatMoments)
                        {
                            if (Application.platform == RuntimePlatform.Android)
                            {
                                ShareModel shareModel = new ShareModel();
                                shareModel.imageUrl = response;
                                shareModel.platformName = AppConst.SHARE_WECHAT_MOMENTS;
                                shareModel.type = AppConst.SHARE_IMAGE_TYPE;
                                shareModel.title = AppConst.SHARE_TITLE;
                                if (mData.medalModel.isShare == 0)
                                {
                                    shareModel.toastMessage = "分享成功+" + mData.medalModel.medalShareScore + "积分";
                                }
                                AndroidForUnity.CallAndroidForShare(SerializeHelper.ToJson(shareModel));
                            }
                            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                            {
                                Dictionary<string,object> sharParmsDic = new Dictionary<string, object>();
                                sharParmsDic.Add("platformType","WechatTimeLine");
                                sharParmsDic.Add("shareType","image");
                                sharParmsDic.Add("image",response);
                                
                                if (mData.medalModel.isShare == 0)
                                {
                                    string shareToastMessage = "分享成功+" + mData.medalModel.medalShareScore + "积分";
                                    sharParmsDic.Add("shareToastMsg",shareToastMessage);
                                }
                                IOSClientUtil.ShareObjectCallIOSClient(SerializeHelper.ToJson(sharParmsDic));
                            }
                        }
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

        private void StartResquestForAddRecord()
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("relBabyId", PlayerPrefsUtil.GetBabyId());
            paramDict.Add("shareType", 2);
            paramDict.Add("medalGrade", mData.medalModel.medalGrade);
            HttpUtil.PostWithSign<object>(UrlConst.AddRecord, paramDict)
                .Subscribe(response =>
                    {
                        
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
        
        // private IEnumerator ScreenShot(Rect mRect)
        // {
        //     Texture2D screenShot = new Texture2D((int)mRect.width,(int)mRect.height);
        //     yield return new WaitForSeconds(0.1f);
        //     screenShot.ReadPixels(mRect,0,0,false);
        //     screenShot.Apply();
        //     byte[] mbytes = screenShot.EncodeToPNG();
        //
        //     System.DateTime mNow = new System.DateTime();
        //     mNow = System.DateTime.Now;
        //     string pictureName = string.Format("screenshot{0}{1}{2}{3}.png", mNow.Day, mNow.Hour, mNow.Minute, mNow.Second);
        //     string fileName = Application.dataPath + pictureName;
        //     System.IO.File.WriteAllBytes(fileName,mbytes);
        //     AssetDatabase.Refresh();
        //     Debug.Log(string.Format("截屏了一张图片:{0}",fileName));
        // }
        
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
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
    }
}

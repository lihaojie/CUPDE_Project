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
    
    
    public class UserInfoPanelData : QFramework.UIPanelData
    {
        public bool IsEdit;
        public UserInfoModel UserInfoModel;
    }
    
    public partial class UserInfoPanel : QFramework.UIPanel
    {
        private ResLoader mResLoader = ResLoader.Allocate();
        private Texture2D mTexture2DSel, mTexture2DNor;
        private Texture2D mTexture2DSexSel, mTexture2DSexNor;
        private string mRelation;
        private string mUserHead;
        
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as UserInfoPanelData ?? new UserInfoPanelData();
            // please add init code here
            BtnUserConfirm.enabled = false;
            mData.IsEdit = mData.UserInfoModel.IsNotNull();
            BtnBack.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                Back();
            });
            // please add init code here
            
            mTexture2DSel = mResLoader.LoadSync<Texture2D>("btn_code_sel");
            mTexture2DNor = mResLoader.LoadSync<Texture2D>("btn_code_nor");
            mTexture2DSexSel = mResLoader.LoadSync<Texture2D>("bg_sex_sel");
            mTexture2DSexNor = mResLoader.LoadSync<Texture2D>("bg_sex_nor");
            
            SimpleEventSystem.GetEvent<UserPhoto>()
                .Subscribe(_ =>
                {
                    Log.I(_.GetPath);
                    // mUserHead = _.GetPath;
                    // ImageDownloadUtils.Instance.SetAsyncImage("file://" + mUserHead, ImgUserHead, false);
                }).AddTo(this);
            
            SimpleEventSystem.GetEvent<ChoosePhotoClick>()
                .Subscribe(_ =>
                {
                    Log.I("_.GetPhotoAction: " + _.GetPhotoAction + " _.GetAction: " + _.GetAction);
                    if (_.GetPhotoAction == ChoosePhotoAction.User)
                    {
                        PickImage(_.GetAction);
                    }
                }).AddTo(this);
            OnClickListener();
            if (mData.IsEdit)
            {
                ImageDownloadUtils.Instance.SetAsyncImage(mData.UserInfoModel.logoUrl, ImgUserHead);
                string babyRelation = mData.UserInfoModel.babyRelation;
                if ("父亲".Equals(babyRelation))
                {
                    OnFatherClick();   
                } else if ("母亲".Equals(babyRelation))
                {
                    OnMotherClick();   
                } else if ("祖父".Equals(babyRelation))
                {
                    OnGrandFatherClick();
                } else if ("祖母".Equals(babyRelation))
                {
                    OnGrandMotherClick();
                } else if ("哥哥".Equals(babyRelation))
                {
                    OnBrotherClick();
                } else if ("姐姐".Equals(babyRelation))
                {
                    OnSisterClick();
                }
                BtnUserConfirm.enabled = true;
                BtnUserConfirm.image.sprite = Sprite.Create(mTexture2DNor,
                    new Rect(0, 0, mTexture2DNor.width, mTexture2DNor.height), Vector2.one * 0.5f);
            }
            
        }

        private void OnClickListener()
        {
            BtnFather.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                OnFatherClick();
            });
            BtnMother.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                OnMotherClick();
            });
            BtnGrandFather.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                OnGrandFatherClick();
            });
            BtnGrandMother.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                OnGrandMotherClick();
            });
            BtnBrother.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                OnBrotherClick();
            });
            BtnSister.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                OnSisterClick();
            });
            BtnUserMask.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                ChoosePic(ChoosePhotoAction.User);
            });
            BtnUserConfirm.onClick.AddListener(() =>
            {
                AudioManager.PlaySound("Button_Audio");
                StartResquestForDoSetRelation();
            });
        }

        /**
         * type 0baby, 1用户
         */
        private void ChoosePic(ChoosePhotoAction mAction)
        {
            UIMgr.OpenPanel<ChoosePhotoPanel>(new ChoosePhotoPanelData()
            {
                action = mAction
            });
        }

        private void PickImage(NativeAction actionPick)
        {
#if UNITY_ANDROID
            NativeGallery.RequestPermission((result, action) =>
            {
                if (result == (int) NativeGallery.Permission.Granted)
                {
                    NativeGallery.GetImageFromGalleryForAndroid( ( path ) =>
                    {
                        if (path.IsNotNullAndEmpty())
                        {
                            mUserHead = path;
                            ImageDownloadUtils.Instance.SetAsyncImage("file://" + path, ImgUserHead, false);
                        }
                        Debug.Log( "Image path: " + path );
                    }, "选择图片", "image/*", true, action);
                }
            }, (int) actionPick);
#elif UNITY_IOS

            // NativeGallery.Permission rest;
            if (actionPick == NativeAction.Album)
            {
                NativeGallery.Permission rest = NativeGallery.RequestIPhonePermission(1);
                if (rest == NativeGallery.Permission.Granted)
                {
                    NativeGallery.GetImageFromGallery((backPath) =>
                    {
                        if (backPath.IsNotNullAndEmpty())
                        {
                            mUserHead = backPath;
                            ImageDownloadUtils.Instance.SetAsyncImage("file://" + backPath, ImgUserHead, false);
                        }
                    }, "选择图片", "image/*");
                }
            }
            else if (actionPick == NativeAction.Camera)
            {
                NativeGallery.Permission rest = NativeGallery.RequestIPhonePermission(4);
                if (rest == NativeGallery.Permission.Granted)
                {
                    NativeGallery.GetIPhoneCameraImageFromGallery((backPath) =>
                    {
                        if (backPath.IsNotNullAndEmpty())
                        {
                            mUserHead = backPath;
                            ImageDownloadUtils.Instance.SetAsyncImage("file://" + backPath, ImgUserHead, false);
                        }
                    }, "选择图片", "image/*");
                }
            }
#else
            
#endif
        }
        void StartResquestForDoSetRelation()
        {
            BtnUserConfirm.enabled = false;
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            paramDict.Add("relation", mRelation);
            paramDict.Add("userId", PlayerPrefsUtil.GetUserId());
            HttpUtil.PostWithSign<UserInfoModel>(UrlConst.DoSetRelation, paramDict, "userHead.jpg", mUserHead)
                .Subscribe(response =>
                    {
                        BtnUserConfirm.enabled = true;
                        response.deviceId = PlayerPrefsUtil.GetDeviceId();
                        response.token = PlayerPrefsUtil.GetToken();
                        PlayerPrefsUtil.UserInfo = response;
                        if (mData.IsEdit)
                        {
                            SimpleEventSystem.Publish(new UpdateUserDate(true));
                            Back();
                        }
                        else
                        {
                            if (PlayerPrefsUtil.GetDeviceId().IsNullOrEmpty())
                            {
                                UIMgr.OpenPanel<BindGuidePanel>(new BindGuidePanelData(), UITransitionType.CIRCLE, this);
                            }
                            else
                            {
                                CommonUtil.OpenCloudMain(this);
                            }
                        }
                    }
                    , e =>
                    {
                        BtnUserConfirm.enabled = true;
                        if (e is HttpException)
                        {
                            HttpException http = e as HttpException;
                            Log.E("弹吐司" + http.Message);
                        }
                    }).AddTo(this);
        }


        private void OnFatherClick()
        {
            mRelation = "父亲";
            BtnFather.enabled = false;
            BtnFather.image.sprite = Sprite.Create(mTexture2DSexSel,
                new Rect(0, 0, mTexture2DSexSel.width, mTexture2DSexSel.height), Vector2.one * 0.5f);

            BtnMother.enabled = true;
            BtnMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandFather.enabled = true;
            BtnGrandFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandMother.enabled = true;
            BtnGrandMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnBrother.enabled = true;
            BtnBrother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnSister.enabled = true;
            BtnSister.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnUserConfirm.enabled = true;
            BtnUserConfirm.image.sprite = Sprite.Create(mTexture2DNor,
                new Rect(0, 0, mTexture2DNor.width, mTexture2DNor.height), Vector2.one * 0.5f);
        }
        private void OnMotherClick()
        {
            mRelation = "母亲";
            BtnMother.enabled = false;
            BtnMother.image.sprite = Sprite.Create(mTexture2DSexSel,
                new Rect(0, 0, mTexture2DSexSel.width, mTexture2DSexSel.height), Vector2.one * 0.5f);

            BtnFather.enabled = true;
            BtnFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandFather.enabled = true;
            BtnGrandFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandMother.enabled = true;
            BtnGrandMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnBrother.enabled = true;
            BtnBrother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnSister.enabled = true;
            BtnSister.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnUserConfirm.enabled = true;
            BtnUserConfirm.image.sprite = Sprite.Create(mTexture2DNor,
                new Rect(0, 0, mTexture2DNor.width, mTexture2DNor.height), Vector2.one * 0.5f);
        }
        private void OnGrandFatherClick()
        {
            mRelation = "祖父";
            BtnGrandFather.enabled = false;
            BtnGrandFather.image.sprite = Sprite.Create(mTexture2DSexSel,
                new Rect(0, 0, mTexture2DSexSel.width, mTexture2DSexSel.height), Vector2.one * 0.5f);

            BtnFather.enabled = true;
            BtnFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnMother.enabled = true;
            BtnMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandMother.enabled = true;
            BtnGrandMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnBrother.enabled = true;
            BtnBrother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnSister.enabled = true;
            BtnSister.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnUserConfirm.enabled = true;
            BtnUserConfirm.image.sprite = Sprite.Create(mTexture2DNor,
                new Rect(0, 0, mTexture2DNor.width, mTexture2DNor.height), Vector2.one * 0.5f);
        }
        private void OnGrandMotherClick()
        {
            mRelation = "祖母";
            BtnGrandMother.enabled = false;
            BtnGrandMother.image.sprite = Sprite.Create(mTexture2DSexSel,
                new Rect(0, 0, mTexture2DSexSel.width, mTexture2DSexSel.height), Vector2.one * 0.5f);

            BtnFather.enabled = true;
            BtnFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnMother.enabled = true;
            BtnMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandFather.enabled = true;
            BtnGrandFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnBrother.enabled = true;
            BtnBrother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnSister.enabled = true;
            BtnSister.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnUserConfirm.enabled = true;
            BtnUserConfirm.image.sprite = Sprite.Create(mTexture2DNor,
                new Rect(0, 0, mTexture2DNor.width, mTexture2DNor.height), Vector2.one * 0.5f);
        }
        private void OnBrotherClick()
        {
            mRelation = "哥哥";
            BtnBrother.enabled = false;
            BtnBrother.image.sprite = Sprite.Create(mTexture2DSexSel,
                new Rect(0, 0, mTexture2DSexSel.width, mTexture2DSexSel.height), Vector2.one * 0.5f);

            BtnFather.enabled = true;
            BtnFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnMother.enabled = true;
            BtnMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandFather.enabled = true;
            BtnGrandFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandMother.enabled = true;
            BtnGrandMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnSister.enabled = true;
            BtnSister.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnUserConfirm.enabled = true;
            BtnUserConfirm.image.sprite = Sprite.Create(mTexture2DNor,
                new Rect(0, 0, mTexture2DNor.width, mTexture2DNor.height), Vector2.one * 0.5f);
        }
        private void OnSisterClick()
        {
            mRelation = "姐姐";
            BtnSister.enabled = false;
            BtnSister.image.sprite = Sprite.Create(mTexture2DSexSel,
                new Rect(0, 0, mTexture2DSexSel.width, mTexture2DSexSel.height), Vector2.one * 0.5f);

            BtnFather.enabled = true;
            BtnFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnMother.enabled = true;
            BtnMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandFather.enabled = true;
            BtnGrandFather.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnGrandMother.enabled = true;
            BtnGrandMother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnBrother.enabled = true;
            BtnBrother.image.sprite = Sprite.Create(mTexture2DSexNor,
                new Rect(0, 0, mTexture2DSexNor.width, mTexture2DSexNor.height), Vector2.one * 0.5f);

            BtnUserConfirm.enabled = true;
            BtnUserConfirm.image.sprite = Sprite.Create(mTexture2DNor,
                new Rect(0, 0, mTexture2DNor.width, mTexture2DNor.height), Vector2.one * 0.5f);
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
            mResLoader.Recycle2Cache();
            mResLoader = null;
        }
    }
}

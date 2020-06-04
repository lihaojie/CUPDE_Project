/****************************************************************************
 * 2020.4 爵色的MacBook Pro
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

using QFramework;
using SuperKid.Entity;
using SuperKid.Utils;
using UniRx;


namespace SuperKid
{
	public partial class FeedBackPanel : UIElement
	{
		private string mPhotoPath;
		private Texture2D texture2DNor;
		private Texture2D texture2DSel;
		private Texture2D texture2DAddPhoto;
		private ResLoader mResLoader = ResLoader.Allocate();
		
		
		private void Awake()
		{
			BtnConfirm.onClick.AddListener(() =>
			{
				AudioManager.PlaySound("Button_Audio");
				if (Application.platform == RuntimePlatform.Android)
				{
					NativeGallery.GetSomethingFromNative((json, action1) =>
					{
						if (json.IsNotNullAndEmpty())
						{
							AppInfo model = SerializeHelper.FromJson<AppInfo>(json);
							StartResquestForAddFeedBack(model.version, model.build, "2");
						}
						else
						{
							StartResquestForAddFeedBack("未知", 0, "2");
						}
					}, (int) NativeAction.VersionJson);
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					string version = Application.version;
					NativeGallery.GetSomethingFromIPhone(res =>
					{
						StartResquestForAddFeedBack(version, res.ToInt(), "1");
					},4);
				}
				else
				{
					StartResquestForAddFeedBack("未知", 0, "0");
				}
			});
			BtnDel.onClick.AddListener(() =>
			{
				AudioManager.PlaySound("Button_Audio");
				if (mPhotoPath.IsNotNullAndEmpty())
				{
					UIMgr.OpenPanel<TipPanel>(new TipPanelData()
					{
						action = TipAction.FeedBackDel,
						message = "确定要删除当前图片吗？"
					});
				}
				
			});
			SimpleEventSystem.GetEvent<TipConfirmClick>()
				.Subscribe(_ =>
				{
					if (_.GetAction == TipAction.FeedBackDel)
					{
						mPhotoPath = "";
						ImgPhoto.sprite = Sprite.Create(texture2DAddPhoto, new Rect(0, 0, texture2DAddPhoto.width, texture2DAddPhoto.height), Vector2.one * 0.5f);
						BtnDel.gameObject.SetActive(false);
					}
				}).AddTo(this);
			BtnAddPhoto.onClick.AddListener(() =>
			{
				AudioManager.PlaySound("Button_Audio");
				ChoosePic(ChoosePhotoAction.FeedBack);
			});
			InputContent.onValueChange.AddListener(ChangeInput);
			SimpleEventSystem.GetEvent<ChoosePhotoClick>()
				.Subscribe(_ =>
				{
					Log.I("_.GetPhotoAction: " + _.GetPhotoAction + " _.GetAction: " + _.GetAction);
					if (_.GetPhotoAction == ChoosePhotoAction.FeedBack)
					{
						PickImage(_.GetAction);
					}
				}).AddTo(this);
			texture2DSel = mResLoader.LoadSync<Texture2D>("btn_code_sel");
			texture2DNor = mResLoader.LoadSync<Texture2D>("btn_code_nor");
			texture2DAddPhoto = mResLoader.LoadSync<Texture2D>("btn_add_photo");
			BtnConfirm.image.sprite = Sprite.Create(texture2DSel, new Rect(0, 0, texture2DSel.width, texture2DSel.height), Vector2.one * 0.5f);
			BtnConfirm.enabled = false;
		}
		
		void ChangeInput(string str)
		{
			if (!str.IsNullOrEmpty())
			{
				BtnConfirm.image.sprite = Sprite.Create(texture2DNor, new Rect(0, 0, texture2DNor.width, texture2DNor.height), Vector2.one * 0.5f);
				BtnConfirm.enabled = true;
			}
			else
			{
				BtnConfirm.image.sprite = Sprite.Create(texture2DSel, new Rect(0, 0, texture2DSel.width, texture2DSel.height), Vector2.one * 0.5f);
				BtnConfirm.enabled = false;
			}
		}
		
		/**
         * type  0baby, 1用户,3意见反馈
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
							mPhotoPath = path;
							ImageDownloadUtils.Instance.SetAsyncImage("file://" + path, ImgPhoto);
							BtnDel.gameObject.SetActive(true);
						}
						Debug.Log( "Image path: " + path );
					}, "选择图片", "image/*", false, action);
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
							mPhotoPath = backPath;
                            ImageDownloadUtils.Instance.SetAsyncImage("file://" + backPath, ImgPhoto, false);
							BtnDel.gameObject.SetActive(true);
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
							mPhotoPath = backPath;
                            ImageDownloadUtils.Instance.SetAsyncImage("file://" + backPath, ImgPhoto, false);
							BtnDel.gameObject.SetActive(true);
                        }
                    }, "选择图片", "image/*");
                }
            }
#else

            
#endif
			
			
			
			
		}

		void StartResquestForAddFeedBack(String version, int build, string station)
		{
			BtnConfirm.enabled = false;
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("userId", PlayerPrefsUtil.GetUserId());
			if (PlayerPrefsUtil.UserInfo != null && PlayerPrefsUtil.UserInfo.babyInfoVo != null)
			{
				paramDict.Add("userName", PlayerPrefsUtil.UserInfo.babyInfoVo.name);
			}
			else
			{
				paramDict.Add("userName", "未知");
			}
			paramDict.Add("content", InputContent.text);
			paramDict.Add("version", version);
			paramDict.Add("build", build);
			paramDict.Add("station", station);
			if (PlayerPrefsUtil.UserInfo != null)
			{
				paramDict.Add("userAccount", PlayerPrefsUtil.UserInfo.mobile);
			}
			else
			{
				paramDict.Add("userAccount", "未知");
			}
			HttpUtil.PostWithSign<object>(UrlConst.AddFeedBack, paramDict, "feedback.jpg", mPhotoPath)
				.Subscribe(response =>
					{
						CommonUtil.toast("反馈成功");
						BtnConfirm.enabled = true;
						InputContent.text = "";
						mPhotoPath = "";
						ImgPhoto.sprite = Sprite.Create(texture2DAddPhoto, new Rect(0, 0, texture2DAddPhoto.width, texture2DAddPhoto.height), Vector2.one * 0.5f);
						BtnDel.gameObject.SetActive(false);
					}
					, e =>
					{
						BtnConfirm.enabled = true;
						if (e is HttpException)
						{
							HttpException http = e as HttpException;
						}
					}).AddTo(this);
		}

		
		protected override void OnBeforeDestroy()
		{
			mResLoader.Recycle2Cache();
			mResLoader = null;
		}
	}
}
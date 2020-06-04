/****************************************************************************
 * 2020.4 爵色的MacBook Pro
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using SuperKid.Utils;
using QFramework;
using SuperKid.Utils;
using UniRx;


namespace SuperKid
{
	public partial class ModifyPasswordPanel : UIElement
	{
		private bool mOld, mNew, mConfirm;
		private ResLoader mResLoader = ResLoader.Allocate();
		private Texture2D texture2DNor;
		private Texture2D texture2DSel;
		
		private void Awake()
		{
			BtnConfirm.enabled = false;
			BtnForget.onClick.AddListener(() =>
			{
				AudioManager.PlaySound("Button_Audio");
				UIMgr.OpenPanel<LoginPanel>(new LoginPanelData()
				{
					action = 4	
				}, UITransitionType.CIRCLE);
			});
			texture2DSel = mResLoader.LoadSync<Texture2D>("btn_code_sel");
			texture2DNor = mResLoader.LoadSync<Texture2D>("btn_code_nor");
			InputOld.onValueChange.AddListener(ChangeOld);
			InputNew.onValueChange.AddListener(ChangeNew);
			InputConfirm.onValueChange.AddListener(ChangeConfirm);
			BtnConfirm.onClick.AddListener(() =>
			{
				AudioManager.PlaySound("Button_Audio");
				if (!StringUtil.checkPassword(InputNew.text))
				{
					Log.I("密码必须由至少8位的字母+数字组成");
					CommonUtil.toast("密码必须由至少8位的字母+数字组成");

				} 
				else if (!InputNew.text.Equals(InputConfirm.text))
				{
					Log.I("新密码必须相同");
					CommonUtil.toast("新密码必须相同");
				}
				else
				{
					StartResquestForDoUpdatePasswd();
				}

			});
		}

		void ChangeOld(string str)
		{
			if (!str.IsNullOrEmpty())
			{
				mOld = true;
				if (mOld && mNew && mConfirm)
				{
					BtnConfirm.image.sprite = Sprite.Create(texture2DNor, new Rect(0, 0, texture2DNor.width, texture2DNor.height), Vector2.one * 0.5f);
					BtnConfirm.enabled = true;
				}
			}
			else
			{
				mOld = false;
				BtnConfirm.image.sprite = Sprite.Create(texture2DSel, new Rect(0, 0, texture2DSel.width, texture2DSel.height), Vector2.one * 0.5f);
				BtnConfirm.enabled = false;
			}
		}
		void ChangeNew(string str)
		{
			if (!str.IsNullOrEmpty())
			{
				mNew = true;
				if (mOld && mNew && mConfirm)
				{
					BtnConfirm.image.sprite = Sprite.Create(texture2DNor, new Rect(0, 0, texture2DNor.width, texture2DNor.height), Vector2.one * 0.5f);
					BtnConfirm.enabled = true;
				}
			}
			else
			{
				mNew = false;
				BtnConfirm.image.sprite = Sprite.Create(texture2DSel, new Rect(0, 0, texture2DSel.width, texture2DSel.height), Vector2.one * 0.5f);
				BtnConfirm.enabled = false;
			}
		}
		void ChangeConfirm(string str)
		{
			if (!str.IsNullOrEmpty())
			{
				mConfirm = true;
				if (mOld && mNew && mConfirm)
				{
					BtnConfirm.image.sprite = Sprite.Create(texture2DNor, new Rect(0, 0, texture2DNor.width, texture2DNor.height), Vector2.one * 0.5f);
					BtnConfirm.enabled = true;
				}
			}
			else
			{
				mConfirm = false;
				BtnConfirm.image.sprite = Sprite.Create(texture2DSel, new Rect(0, 0, texture2DSel.width, texture2DSel.height), Vector2.one * 0.5f);
				BtnConfirm.enabled = false;
			}
		}
		
		void StartResquestForDoUpdatePasswd()
		{
			if (PlayerPrefsUtil.UserInfo == null || PlayerPrefsUtil.UserInfo.mobile.IsNullOrEmpty())
			{
				CommonUtil.toast("数据异常，请重新登录");
				return;
			}
			BtnConfirm.enabled = false;
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("userId", PlayerPrefsUtil.GetUserId());
			paramDict.Add("newPasswd", InputConfirm.text);
			paramDict.Add("oldPasswd", InputOld.text);
			HttpUtil.PostWithSign<object>(UrlConst.DoUpdatePasswd, paramDict)
				.Subscribe(response =>
					{
						PlayerPrefsUtil.SetPwd(InputConfirm.text);
						CommonUtil.toast("新密码设置成功");
						InputOld.text = "";
						InputNew.text = "";
						InputConfirm.text = "";
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
		
		
		protected override void OnBeforeDestroy()
		{
			mResLoader.Recycle2Cache();
			mResLoader = null;
		}
	}
}
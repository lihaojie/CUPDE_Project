/****************************************************************************
 * 2020.4 爵色的MacBook Pro
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using SuperKid.Entity;
using SuperKid.Utils;
using UniRx;

namespace SuperKid
{
	public partial class DevicePanel : UIElement
	{
		private void Awake()
		{
			if (PlayerPrefsUtil.GetDeviceId().IsNullOrEmpty())
			{
				BtnUnbind.gameObject.SetActive(true);
			}
			else
			{
				TextID.text = PlayerPrefsUtil.GetDeviceId();
				StartResquestForCheckDeviceVersion();
				StartResquestForGetDeviceStatus();
				BtnUnbind.gameObject.SetActive(true);
			}
			if (Application.platform == RuntimePlatform.Android)
			{
				NativeGallery.GetSomethingFromNative(( json , action1) => { TextAppVersion.text = json; }, (int) NativeAction.Version);
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				NativeGallery.GetSomethingFromIPhone(res => { TextAppVersion.text = res; },2);
			}
			BtnUnbind.onClick.AddListener(() =>
			{
				AudioManager.PlaySound("Button_Audio");
				UIMgr.OpenPanel<TipPanel>(new TipPanelData()
				{
					action = TipAction.Unbind,
					message = "确定解除绑定 Dola 设备吗？\n解绑后，将删除此设备的历史学习记录",
					strTitle = "解绑提示"
				});
			});
			SimpleEventSystem.GetEvent<TipConfirmClick>()
				.Subscribe(_ =>
				{
					if (_.GetAction == TipAction.Unbind)
					{
						StartResquestForUnbind();
					}
				}).AddTo(this);
		}

		private void OnEnable()
		{
			
		}
		
		
		void StartResquestForUnbind()
		{
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("deviceId", PlayerPrefsUtil.GetDeviceId());
			paramDict.Add("userId", PlayerPrefsUtil.GetUserId());
			HttpUtil.PostWithSign<object>(UrlConst.Unbind, paramDict)
				.Subscribe(response =>
					{
						Log.I("解绑成功");
						CommonUtil.toast("解除绑定成功");
						CommonUtil.logout();
						UIMgr.CloseAllPanel();
						UIMgr.OpenPanel<LoginPanel>(new LoginPanelData(), UITransitionType.CIRCLE);
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
		
		
		void StartResquestForGetDeviceStatus()
		{
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("deviceId", PlayerPrefsUtil.GetDeviceId());
			paramDict.Add("userId", PlayerPrefsUtil.GetUserId());
			HttpUtil.GetWithSign<DeviceModel>(UrlConst.GetDeviceStatus, paramDict)
				.Subscribe(response =>
					{
						if (response.deviceOnline)
						{
							TextWifi.text = "已连接";
							ImgWifi.gameObject.SetActive(false);
						}
						else
						{
							TextWifi.text = "未连接";
							ImgWifi.gameObject.SetActive(true);
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
		
		void StartResquestForCheckDeviceVersion()
		{
			Dictionary<string, object> paramDict = new Dictionary<string, object>();
			paramDict.Add("deviceId", PlayerPrefsUtil.GetDeviceId());
			paramDict.Add("userId", PlayerPrefsUtil.GetUserId());
			HttpUtil.GetWithSign<DeviceModel>(UrlConst.CheckDeviceVersion, paramDict)
				.Subscribe(response =>
					{
						TextVersion.text = response.current;
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
		}
	}
}
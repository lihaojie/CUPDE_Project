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
	public partial class ClearCachePanel : UIElement
	{
		private void Awake()
		{
			BtnClearCache.onClick.AddListener(() =>
			{
				AudioManager.PlaySound("Button_Audio");
				UIMgr.OpenPanel<TipPanel>(new TipPanelData()
				{
					action = TipAction.ClearCache,
					message = "本操作将会清除下载的图片视频音频内容",
					strTitle = "清除缓存提示"
				});
			});
			SimpleEventSystem.GetEvent<TipConfirmClick>()
				.Subscribe(_ =>
				{
					if (_.GetAction == TipAction.ClearCache)
					{
						if (Application.platform == RuntimePlatform.Android)
						{
							NativeGallery.GetSomethingFromNative((json, action) =>
							{
								TextCache.text = "0KB";
								CommonUtil.toast("清除缓存成功");
							}, (int)NativeAction.ClearCache);
						}
						else if (Application.platform == RuntimePlatform.IPhonePlayer)
						{
							IOSClientUtil.ClearCacheCallIOSClient();
							TextCache.text = "0KB";
							CommonUtil.toast("清除缓存成功");
						}
					}
				}).AddTo(this);
			if (Application.platform == RuntimePlatform.Android)
			{
				NativeGallery.GetSomethingFromNative((json, action) => { TextCache.text = json; }, (int)NativeAction.Cache);
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				NativeGallery.GetSomethingFromIPhone(res => { TextCache.text = res; },5);
			}
		}

		private void OnEnable()
		{
			
		}

		protected override void OnBeforeDestroy()
		{
		}
	}
}
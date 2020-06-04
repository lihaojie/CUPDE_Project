/****************************************************************************
 * 2020.4 爵色的MacBook Pro
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using SuperKid.Utils;

namespace SuperKid
{
	public partial class AboutPanel : UIElement
	{
		private void Awake()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				NativeGallery.GetSomethingFromNative(( json , action1) => { TextVersion.text = json; }, (int) NativeAction.Version);
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				NativeGallery.GetSomethingFromIPhone(res => { TextVersion.text = res; },2);
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
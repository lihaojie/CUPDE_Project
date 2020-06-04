/****************************************************************************
 * 2020.5 爵色的MacBook Pro
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace SuperKid
{
	public partial class DevicePanel
	{
		[SerializeField] public UnityEngine.UI.Image ImgWifi;
		[SerializeField] public UnityEngine.UI.Text TextWifi;
		[SerializeField] public UnityEngine.UI.Text TextAppVersion;
		[SerializeField] public UnityEngine.UI.Text TextVersion;
		[SerializeField] public UnityEngine.UI.Text TextID;
		[SerializeField] public UnityEngine.UI.Button BtnUnbind;

		public void Clear()
		{
			ImgWifi = null;
			TextWifi = null;
			TextAppVersion = null;
			TextVersion = null;
			TextID = null;
			BtnUnbind = null;
		}

		public override string ComponentName
		{
			get { return "DevicePanel";}
		}
	}
}

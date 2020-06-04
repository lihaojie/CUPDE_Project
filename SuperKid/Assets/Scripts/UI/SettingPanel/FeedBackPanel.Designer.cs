/****************************************************************************
 * 2020.5 爵色的MacBook Pro
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace SuperKid
{
	public partial class FeedBackPanel
	{
		[SerializeField] public UnityEngine.UI.InputField InputContent;
		[SerializeField] public UnityEngine.UI.Button BtnAddPhoto;
		[SerializeField] public UnityEngine.UI.Image ImgPhoto;
		[SerializeField] public UnityEngine.UI.Button BtnDel;
		[SerializeField] public UnityEngine.UI.Button BtnConfirm;

		public void Clear()
		{
			InputContent = null;
			BtnAddPhoto = null;
			ImgPhoto = null;
			BtnDel = null;
			BtnConfirm = null;
		}

		public override string ComponentName
		{
			get { return "FeedBackPanel";}
		}
	}
}

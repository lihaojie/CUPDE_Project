/****************************************************************************
 * 2020.5 爵色的MacBook Pro
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace SuperKid
{
	public partial class ModifyPasswordPanel
	{
		[SerializeField] public UnityEngine.UI.InputField InputOld;
		[SerializeField] public UnityEngine.UI.InputField InputNew;
		[SerializeField] public UnityEngine.UI.InputField InputConfirm;
		[SerializeField] public UnityEngine.UI.Button BtnForget;
		[SerializeField] public UnityEngine.UI.Button BtnConfirm;

		public void Clear()
		{
			InputOld = null;
			InputNew = null;
			InputConfirm = null;
			BtnForget = null;
			BtnConfirm = null;
		}

		public override string ComponentName
		{
			get { return "ModifyPasswordPanel";}
		}
	}
}

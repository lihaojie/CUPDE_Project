/****************************************************************************
 * 2020.5 爵色的MacBook Pro
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace SuperKid
{
	public partial class PrivacyPanel
	{
		[SerializeField] public UnityEngine.UI.Text TextPrivacy;

		public void Clear()
		{
			TextPrivacy = null;
		}

		public override string ComponentName
		{
			get { return "PrivacyPanel";}
		}
	}
}

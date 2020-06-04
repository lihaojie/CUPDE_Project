/****************************************************************************
 * 2020.5 爵色的MacBook Pro
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace SuperKid
{
	public partial class UserAgreementPanel
	{
		[SerializeField] public UnityEngine.UI.Text TextUserAgreement;

		public void Clear()
		{
			TextUserAgreement = null;
		}

		public override string ComponentName
		{
			get { return "UserAgreementPanel";}
		}
	}
}

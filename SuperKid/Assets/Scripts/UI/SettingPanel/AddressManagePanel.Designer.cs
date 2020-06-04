/****************************************************************************
 * 2020.5 爵色的MacBook Pro
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace SuperKid
{
	public partial class AddressManagePanel
	{
		[SerializeField] public UnityEngine.RectTransform Content;

		public void Clear()
		{
			Content = null;
		}

		public override string ComponentName
		{
			get { return "AddressManagePanel";}
		}
	}
}

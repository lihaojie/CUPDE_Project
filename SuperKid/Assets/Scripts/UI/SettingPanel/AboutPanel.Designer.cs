/****************************************************************************
 * 2020.5 爵色的MacBook Pro
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace SuperKid
{
	public partial class AboutPanel
	{
		[SerializeField] public UnityEngine.UI.Text TextVersion;

		public void Clear()
		{
			TextVersion = null;
		}

		public override string ComponentName
		{
			get { return "AboutPanel";}
		}
	}
}

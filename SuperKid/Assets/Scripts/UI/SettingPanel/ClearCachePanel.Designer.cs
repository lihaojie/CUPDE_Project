/****************************************************************************
 * 2020.5 爵色的MacBook Pro
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace SuperKid
{
	public partial class ClearCachePanel
	{
		[SerializeField] public UnityEngine.UI.Button BtnClearCache;
		[SerializeField] public UnityEngine.UI.Text TextCache;

		public void Clear()
		{
			BtnClearCache = null;
			TextCache = null;
		}

		public override string ComponentName
		{
			get { return "ClearCachePanel";}
		}
	}
}

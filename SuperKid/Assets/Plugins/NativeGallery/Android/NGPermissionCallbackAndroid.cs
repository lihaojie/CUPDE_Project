#if !UNITY_EDITOR && UNITY_ANDROID
using System.Threading;
using UnityEngine;

namespace NativeGalleryNamespace
{
	public class NGPermissionCallbackAndroid : AndroidJavaProxy
	{
		private readonly NativeGallery.PermissionCallback callbackPermission;
		private readonly NGCallbackHelper callbackHelper;
		private object threadLock;
		
		public int Result { get; private set; }

		public NGPermissionCallbackAndroid(object threadLock ) : base( "com.yasirkula.unity.NativeGalleryPermissionReceiver" )
		{
			Result = -1;
			this.threadLock = threadLock;
		}	

		public NGPermissionCallbackAndroid(NativeGallery.PermissionCallback callbackPermission) : base( "com.yasirkula.unity.NativeGalleryPermissionReceiver" )
		{
			Result = -1;
			this.callbackPermission = callbackPermission;
			callbackHelper = new GameObject( "NGCallbackHelper" ).AddComponent<NGCallbackHelper>();
		}

		public void OnPermissionResult( int result )
		{
			Result = result;

			lock( threadLock )
			{
				Monitor.Pulse( threadLock );
			}
		}

		public void OnPermissionResult( int result, int action )
		{
			Result = result;
			if( result != 2 && PlayerPrefs.GetInt( "NativePermission" + action, -1 ) != result )
			{
				PlayerPrefs.SetInt( "NativePermission" + action, result );
				PlayerPrefs.Save();
			}
			callbackHelper.CallOnMainThread( () => OnPermissionResultCallback( result, action ) );
		}

		private void OnPermissionResultCallback( int result, int action )
		{

			try
			{
				if( callbackPermission != null )
					callbackPermission( result, action );
			}
			finally
			{
				Object.Destroy( callbackHelper );
			}
		}

	}
}
#endif
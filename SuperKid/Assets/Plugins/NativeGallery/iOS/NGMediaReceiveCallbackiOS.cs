#if !UNITY_EDITOR && UNITY_IOS
using UnityEngine;

namespace NativeGalleryNamespace
{
	public class NGMediaReceiveCallbackiOS : MonoBehaviour
	{
		private static NGMediaReceiveCallbackiOS instance;
		private NativeGallery.MediaPickCallback callback;

		private float nextBusyCheckTime;

		public static bool IsBusy { get; private set; }
		public static int mAction  { get; private set; }

		
		[System.Runtime.InteropServices.DllImport( "__Internal" )]
		private static extern int _NativeGallery_IsMediaPickerBusy();

		[System.Runtime.InteropServices.DllImport( "__Internal" )]
		private static extern int _NativeGallery_IsGetSSIDBusy();

		[System.Runtime.InteropServices.DllImport( "__Internal" )]
		private static extern int _NativeGallery_IsBuildVersionBusy();


		public static void Initialize( NativeGallery.MediaPickCallback callback )
		{
			if( IsBusy )
				return;

			if( instance == null )
			{
				instance = new GameObject( "NGMediaReceiveCallbackiOS" ).AddComponent<NGMediaReceiveCallbackiOS>();
				DontDestroyOnLoad( instance.gameObject );
			}

			instance.callback = callback;

			instance.nextBusyCheckTime = Time.realtimeSinceStartup + 1f;
			IsBusy = true;
			mAction = 0;
		}



		public static void Initialize( NativeGallery.MediaPickCallback callback ,int action)
		{
			if( IsBusy )
				return;

			if( instance == null )
			{
				instance = new GameObject( "NGMediaReceiveCallbackiOS" ).AddComponent<NGMediaReceiveCallbackiOS>();
				DontDestroyOnLoad( instance.gameObject );
			}

			instance.callback = callback;
			mAction = 1;
			
			instance.nextBusyCheckTime = Time.realtimeSinceStartup + 1f;
			IsBusy = true;
		}
		// private void Update()
		// {
			
		// 	if( IsBusy )
		// 	{
		// 		if( Time.realtimeSinceStartup >= nextBusyCheckTime )
		// 		{
		// 			nextBusyCheckTime = Time.realtimeSinceStartup + 1f;

		// 			if (action == 1)
		// 			{


		// 			}
		// 			else if( _NativeGallery_IsMediaPickerBusy() == 0 )
		// 			{
		// 				IsBusy = false;
						
		// 				if( callback != null )
		// 				{
		// 					callback( null );
		// 					callback = null;
		// 				}
		// 			}
		// 		}
		// 	}
		// }

		public void OnMediaReceived( string path )
		{
			IsBusy = false;
			
			if( string.IsNullOrEmpty( path ) )
				path = null;
			
			if( callback != null )
			{
				callback( path );
				callback = null;
			}
		}

		public void GetSSIDResult( string path )
		{
			IsBusy = false;
			if( string.IsNullOrEmpty( path ) )
				path = null;
			
			if( callback != null )
			{
				callback( path );
				callback = null;
			}
		}


		public void GetBuildVersionResult( string path )
		{
			
			IsBusy = false;
			if( string.IsNullOrEmpty( path ) )
				path = null;
			
			if( callback != null )
			{
				callback( path );
				callback = null;
			}
		}

		public void GetCacheSizeResult( string path )
		{
			
			IsBusy = false;
			if( string.IsNullOrEmpty( path ) )
				path = null;
			
			if( callback != null )
			{
				callback( path );
				callback = null;
			}
		}
	}
}
#endif
﻿#if !UNITY_EDITOR && UNITY_ANDROID
using UnityEngine;

namespace NativeGalleryNamespace
{
	public class NGMediaReceiveCallbackAndroid : AndroidJavaProxy
	{
		private readonly NativeGallery.MediaPickCallback callback;
		private readonly NativeGallery.MediaPickMultipleCallback callbackMultiple;
		private readonly NativeGallery.GetSomethingCallback callbackSomething;

		private readonly NGCallbackHelper callbackHelper;

		public NGMediaReceiveCallbackAndroid( NativeGallery.MediaPickCallback callback, NativeGallery.MediaPickMultipleCallback callbackMultiple ) : base( "com.yasirkula.unity.NativeGalleryMediaReceiver" )
		{
			this.callback = callback;
			this.callbackMultiple = callbackMultiple;
			callbackHelper = new GameObject( "NGCallbackHelper" ).AddComponent<NGCallbackHelper>();
		}
		public NGMediaReceiveCallbackAndroid( NativeGallery.GetSomethingCallback callbackSomething) : base( "com.yasirkula.unity.NativeGalleryMediaReceiver" )
		{
			this.callbackSomething = callbackSomething;
			callbackHelper = new GameObject( "NGCallbackHelper" ).AddComponent<NGCallbackHelper>();
		}

		public void OnSomethingReceived(string json, int action) 
		{
			callbackHelper.CallOnMainThread( () => SomethingReceiveCallback( json, action ) );
		}
		public void OnMediaReceived( string path )
		{
			callbackHelper.CallOnMainThread( () => MediaReceiveCallback( path ) );
		}

		public void OnMultipleMediaReceived( string paths )
		{
			string[] result = null;
			if( !string.IsNullOrEmpty( paths ) )
			{
				string[] pathsSplit = paths.Split( '>' );

				int validPathCount = 0;
				for( int i = 0; i < pathsSplit.Length; i++ )
				{
					if( !string.IsNullOrEmpty( pathsSplit[i] ) )
						validPathCount++;
				}

				if( validPathCount == 0 )
					pathsSplit = new string[0];
				else if( validPathCount != pathsSplit.Length )
				{
					string[] validPaths = new string[validPathCount];
					for( int i = 0, j = 0; i < pathsSplit.Length; i++ )
					{
						if( !string.IsNullOrEmpty( pathsSplit[i] ) )
							validPaths[j++] = pathsSplit[i];
					}

					pathsSplit = validPaths;
				}

				result = pathsSplit;
			}

			callbackHelper.CallOnMainThread( () => MediaReceiveMultipleCallback( result ) );
		}

		private void SomethingReceiveCallback( string json, int action )
		{
			if( string.IsNullOrEmpty( json ) )
				json = null;

			try
			{
				if( callbackSomething != null )
					callbackSomething( json, action );
			}
			finally
			{
				Object.Destroy( callbackHelper );
			}
		}

		private void MediaReceiveCallback( string path )
		{
			if( string.IsNullOrEmpty( path ) )
				path = null;

			try
			{
				if( callback != null )
					callback( path );
			}
			finally
			{
				Object.Destroy( callbackHelper );
			}
		}

		private void MediaReceiveMultipleCallback( string[] paths )
		{
			if( paths != null && paths.Length == 0 )
				paths = null;

			try
			{
				if( callbackMultiple != null )
					callbackMultiple( paths );
			}
			finally
			{
				Object.Destroy( callbackHelper );
			}
		}
	}
}
#endif
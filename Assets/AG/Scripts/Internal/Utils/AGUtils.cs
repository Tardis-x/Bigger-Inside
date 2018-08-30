#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	public static class AGUtils
	{
		static AndroidJavaObject _activity;

		public static AndroidJavaObject Activity
		{
			get
			{
				if (_activity == null)
				{
					var unityPlayer = new AndroidJavaClass(C.ComUnity3DPlayerUnityPlayer);
					_activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
				}
				return _activity;
			}
		}

		public static AndroidJavaObject ExternalCacheDirectory
		{
			get { return Activity.CallAJOSafe("getExternalCacheDir"); }
		}

		public static AndroidJavaObject CacheDirectory
		{
			get { return Activity.CallAJOSafe("getCacheDir"); }
		}

		public static AndroidJavaObject CodeCacheDirectory
		{
			get { return Activity.CallAJOSafe("getCodeCacheDir"); }
		}
		
		public static AndroidJavaObject DataDir
		{
			get { return Activity.CallAJOSafe("getDataDir"); }
		}
		
		public static AndroidJavaObject ObbDir
		{
			get { return Activity.CallAJOSafe("getObbDir"); }
		}

		public static AndroidJavaObject ActivityDecorView
		{
			get { return Window.Call<AndroidJavaObject>("getDecorView"); }
		}

		public static AndroidJavaObject Window
		{
			get { return Activity.Call<AndroidJavaObject>("getWindow"); }
		}

		public static AndroidJavaObject PackageManager
		{
			get { return Activity.CallAJO("getPackageManager"); }
		}

		public static AndroidJavaObject ContentResolver
		{
			get { return Activity.CallAJO("getContentResolver"); }
		}

		public static bool HasSystemFeature(string feature)
		{
			using (var pm = PackageManager)
			{
				return pm.CallBool("hasSystemFeature", feature);
			}
		}

		public static long CurrentTimeMillis
		{
			get
			{
				using (var system = new AndroidJavaClass(C.JavaLangSystem))
				{
					return system.CallStaticLong("currentTimeMillis");
				}
			}
		}

		#region reflection

		public static AndroidJavaObject ClassForName(string className)
		{
			using (var clazz = new AndroidJavaClass(C.JavaLangClass))
			{
				return clazz.CallStaticAJO("forName", className);
			}
		}

		public static AndroidJavaObject Cast(this AndroidJavaObject source, string destClass)
		{
			using (var destClassAJC = ClassForName(destClass))
			{
				return destClassAJC.Call<AndroidJavaObject>("cast", source);
			}
		}

		public static bool IsJavaNull(this AndroidJavaObject ajo)
		{
			return ajo == null || ajo.GetRawObject().ToInt32() == 0;
		}

		#endregion

		public static bool IsNotAndroidCheck()
		{
			bool isAndroid = Application.platform == RuntimePlatform.Android;

			if (isAndroid)
			{
				GoodiesSceneHelper.Init();
			}

			return !isAndroid;
		}

		public static void RunOnUiThread(Action action)
		{
			Activity.Call("runOnUiThread", new AndroidJavaRunnable(action));
		}

		public static void StartActivity(AndroidJavaObject intent, Action fallback = null)
		{
			try
			{
				Activity.Call("startActivity", intent);
			}
			catch (AndroidJavaException exception)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("Could not start the activity with " + intent.JavaToString() + ": " + exception.Message);
				}
				if (fallback != null)
				{
					fallback();
				}
			}
			finally
			{
				intent.Dispose();
			}
		}

		public static void StartActivityWithChooser(AndroidJavaObject intent, string chooserTitle)
		{
			try
			{
				var jChooser = intent.CallStaticAJO("createChooser", intent, chooserTitle);
				Activity.Call("startActivity", jChooser);
			}
			catch (AndroidJavaException exception)
			{
				Debug.LogWarning("Could not start the activity with " + intent.JavaToString() + ": " + exception.Message);
			}
			finally
			{
				intent.Dispose();
			}
		}

		public static void SendBroadcast(AndroidJavaObject intent)
		{
			Activity.Call("sendBroadcast", intent);
		}

		[PublicAPI]
		public static AndroidJavaObject GetMainActivityClass()
		{
			var packageName = AGDeviceInfo.GetApplicationPackage();
			using (PackageManager)
			{
				var launchIntent = PackageManager.CallAJO("getLaunchIntentForPackage", packageName);
				try
				{
					var className = launchIntent.CallAJO("getComponent").CallStr("getClassName");
					return ClassForName(className);
				}
				catch (Exception e)
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("Unable to find Main Activity Class: " + e.Message);
					}
					return null;
				}
			}
		}

		public static AndroidJavaObject NewJavaFile(string path)
		{
			return new AndroidJavaObject(C.JavaIoFile, path);
		}

		#region images

		public static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D tex2D, ImageFormat format = ImageFormat.PNG)
		{
			byte[] encoded = tex2D.Encode(format);
			return C.AndroidGraphicsBitmapFactory.AJCCallStaticOnceAJO("decodeByteArray", encoded, 0, encoded.Length);
		}

		/// <summary>
		/// Loads Texture2D from URI
		/// </summary>
		/// <returns>The from URI internal.</returns>
		/// <param name="uri">URI.</param>
		public static Texture2D TextureFromUriInternal(string uri)
		{
			if (string.IsNullOrEmpty(uri))
			{
				return null;
			}

			using (var resolver = ContentResolver)
			{
				var uriAjo = AndroidUri.Parse(uri);
				try
				{
					var stream = resolver.CallAJO("openInputStream", uriAjo);
					var bitmapAjo = C.AndroidGraphicsBitmapFactory.AJCCallStaticOnceAJO("decodeStream", stream);

					return Texture2DFromBitmap(bitmapAjo);
				}
				catch (Exception ex)
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogException(ex);
					}
					return null;
				}
			}
		}

		static Texture2D Texture2DFromBitmap(AndroidJavaObject bitmapAjo)
		{
			var compressFormatPng = new AndroidJavaClass(C.AndroidGraphicsBitmapCompressFormat).GetStatic<AndroidJavaObject>("PNG");
			var outputStream = new AndroidJavaObject(C.JavaIoBytearrayOutputStream);
			bitmapAjo.CallBool("compress", compressFormatPng, 100, outputStream);
			var buffer = outputStream.Call<byte[]>("toByteArray");

			var tex = new Texture2D(2, 2);
			tex.LoadImage(buffer);
			return tex;
		}

		public static Texture2D Texture2DFromFile(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}

			try
			{
				var bitmapAjo = DecodeBitmap(path);
				bitmapAjo = RotateBitmap(bitmapAjo, path);
				return Texture2DFromBitmap(bitmapAjo);
			}
			catch (Exception ex)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogException(ex);
				}
				return null;
			}
		}

		public static AndroidJavaObject DecodeBitmap(string path)
		{
			return C.AndroidGraphicsBitmapFactory.AJCCallStaticOnceAJO("decodeFile", path);
		}

		static AndroidJavaObject RotateBitmap(AndroidJavaObject bitmap, string photoPath)
		{
			try
			{
				var ei = new AndroidJavaObject(C.AndroidMediaExifinterface, photoPath);

				const int ORIENTATION_ROTATE_90 = 6;
				const int ORIENTATION_ROTATE_180 = 3;
				const int ORIENTATION_ROTATE_270 = 8;
				int orientation = ei.CallInt("getAttributeInt", "Orientation", 0);
				switch (orientation)
				{
					case ORIENTATION_ROTATE_90:
						return RotateBitmap(bitmap, 90f);
					case ORIENTATION_ROTATE_180:
						return RotateBitmap(bitmap, 180f);
					case ORIENTATION_ROTATE_270:
						return RotateBitmap(bitmap, 270f);
					default:
						return bitmap;
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Failed rotating bitmap");
				Debug.LogException(e);
				return bitmap;
			}
		}

		static AndroidJavaObject RotateBitmap(AndroidJavaObject bitmap, float angle)
		{
			using (var matrix = new AndroidJavaObject(C.AndroidGraphicsMatrix))
			{
				matrix.CallBool("postRotate", angle);
				return C.AndroidGraphicsBitmap.AJCCallStaticOnceAJO("createBitmap", bitmap, 0, 0, bitmap.CallInt("getWidth"), bitmap.CallInt("getHeight"), matrix, true);
			}
		}

		#endregion
	}
}
#endif
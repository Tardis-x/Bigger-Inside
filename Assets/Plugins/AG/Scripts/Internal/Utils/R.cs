
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using UnityEngine;

	public static class R
	{
		const string DrawableRes = "drawable";
		const string UnityAppIconDefaultName = "app_icon";

		public static int UnityLauncherIcon
		{
			get { return GetAppDrawableId(UnityAppIconDefaultName); }
		}

		public static int GetAppDrawableId(string drawableName)
		{
			return GetAppResourceId(drawableName, DrawableRes);
		}

		public static int GetAppResourceId(string variableName, string resourceName)
		{
			try
			{
				return AGUtils.Activity.CallAJO("getResources")
					.Call<int>("getIdentifier", variableName, resourceName, AGDeviceInfo.GetApplicationPackage());
			}
			catch (Exception)
			{
#if DEVELOPMENT_BUILD
				Debug.LogWarning("Could not get " + variableName);
#endif
				return 0;
			}
		}

		public static int GetAndroidDrawableId(string name)
		{
			using (var r = new AndroidJavaClass(C.AndroidRDrawable))
			{
				return r.GetStatic<int>(name);
			}
		}
	}
}

#endif
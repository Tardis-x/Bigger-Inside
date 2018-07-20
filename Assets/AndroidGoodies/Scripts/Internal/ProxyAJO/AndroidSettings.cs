
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using UnityEngine;

	public static class AndroidSettings
	{
		public const string ACTION_MANAGE_WRITE_SETTINGS = "android.settings.action.MANAGE_WRITE_SETTINGS";

		public static class System
		{
			public const string SCREEN_BRIGHTNESS = "screen_brightness";
			public const string SCREEN_BRIGHTNESS_MODE = "screen_brightness_mode";
			public const int SCREEN_BRIGHTNESS_MODE_MANUAL = 0;

			static AndroidJavaClass SettingsSystemClass
			{
				get { return new AndroidJavaClass(C.AndroidProviderSettingsSystem); }
			}

			public static bool CanWrite()
			{
				using (var systemSettings = SettingsSystemClass)
				{
					return systemSettings.CallStaticBool("canWrite", AGUtils.Activity);
				}
			}

			public static bool PutInt(string name, int value)
			{
				using (var systemSettings = SettingsSystemClass)
				{
					return systemSettings.CallStaticBool("putInt", AGUtils.ContentResolver, name, value);
				}
			}

			public static int GetInt(string name, int defValue)
			{
				using (var systemSettings = SettingsSystemClass)
				{
					return systemSettings.CallStaticInt("getInt", AGUtils.ContentResolver, name, defValue);
				}
			}
		}
	}
}
#endif
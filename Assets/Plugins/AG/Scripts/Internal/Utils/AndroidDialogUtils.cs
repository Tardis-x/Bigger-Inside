
#if UNITY_ANDROID

namespace DeadMosquito.AndroidGoodies.Internal
{
	using UnityEngine;

	public static class AndroidDialogUtils
	{
		// Added in API level 22
		const int Theme_DeviceDefault_Dialog_Alert = 0x010302d1;

		const int Theme_DeviceDefault_Light_Dialog_Alert = 0x010302d2;

		const int Theme_Material_Dialog_Alert = 0x01030226;
		const int Theme_Material_Light_Dialog_Alert = 16974394;

		// Added in API level 14
		const int THEME_DEVICE_DEFAULT_DARK = 0x00000004;

		const int THEME_DEVICE_DEFAULT_LIGHT = 0x00000005;

		// Added in API level 11
		const int THEME_HOLO_DARK = 0x00000002;

		const int THEME_HOLO_LIGHT = 0x00000003;

		const int ThemeDefault = -1;

		public static bool IsDefault(int theme)
		{
			return theme == ThemeDefault;
		}

		// Bug with date picker: http://stackoverflow.com/questions/38315419/unity-android-datepicker-size-on-nexus-7-2-gen
		public static int GetDialogTheme(AGDialogTheme theme)
		{
			if (theme == AGDialogTheme.Default)
			{
				return ThemeDefault;
			}

			int deviceSdkInt = AGDeviceInfo.SDK_INT;
			if (deviceSdkInt >= AGDeviceInfo.VersionCodes.LOLLIPOP)
			{
				return theme == AGDialogTheme.Light ? Theme_Material_Light_Dialog_Alert : Theme_Material_Dialog_Alert;
			}

			if (deviceSdkInt >= AGDeviceInfo.VersionCodes.ICE_CREAM_SANDWICH)
			{
				return theme == AGDialogTheme.Light ? THEME_DEVICE_DEFAULT_LIGHT : THEME_DEVICE_DEFAULT_DARK;
			}

			if (deviceSdkInt >= AGDeviceInfo.VersionCodes.HONEYCOMB)
			{
				return theme == AGDialogTheme.Light ? THEME_HOLO_LIGHT : THEME_HOLO_DARK;
			}

			return ThemeDefault;
		}

		// https://stackoverflow.com/questions/22794049/how-do-i-maintain-the-immersive-mode-in-dialogs
		public static void ShowWithImmersiveModeWorkaround(AndroidJavaObject dialog)
		{
			var dialogWindow = dialog.CallAJO("getWindow");
			const int FLAG_NOT_FOCUSABLE = 8;
			dialogWindow.Call("setFlags", FLAG_NOT_FOCUSABLE, FLAG_NOT_FOCUSABLE);
			dialog.Call("show");
			var currentVisibility = AGUtils.Window.CallAJO("getDecorView").CallInt("getSystemUiVisibility");
			dialogWindow.CallAJO("getDecorView").Call("setSystemUiVisibility", currentVisibility);
			dialogWindow.Call("clearFlags", FLAG_NOT_FOCUSABLE);
		}
	}
}
#endif
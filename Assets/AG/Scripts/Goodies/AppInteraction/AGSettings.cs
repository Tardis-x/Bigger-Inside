// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGSettings.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using UnityEngine;

	/// <summary>
	/// Allows to perform tasks on Android settings.
	/// <see href="https://developer.android.com/reference/android/provider/Settings.html">Android Settings Docs</see>
	/// </summary>
	public static class AGSettings
	{
		/// <summary>
		/// Show system settings.
		/// </summary>
		public const string ACTION_SETTINGS = "android.settings.SETTINGS";
		// API 1

		/// <summary>
		/// Show settings for accessibility modules.
		/// </summary>
		public const string ACTION_ACCESSIBILITY_SETTINGS = "android.settings.ACCESSIBILITY_SETTINGS";
		// API 5

		/// <summary>
		/// Show add account screen for creating a new account.
		/// </summary>
		public const string ACTION_ADD_ACCOUNT = "android.settings.ADD_ACCOUNT_SETTINGS";
		// API 5

		/// <summary>
		/// Show settings to allow entering/exiting airplane mode.
		/// </summary>
		public const string ACTION_AIRPLANE_MODE_SETTINGS = "android.settings.AIRPLANE_MODE_SETTINGS";
		// API 3

		/// <summary>
		/// Activity Action: Show settings to allow configuration of APNs.
		/// </summary>
		public const string ACTION_APN_SETTINGS = "android.settings.APN_SETTINGS";
		// API 1

		/// <summary>
		/// Show screen of details about a particular application.
		/// </summary>
		public const string ACTION_APPLICATION_DETAILS_SETTINGS = "android.settings.APPLICATION_DETAILS_SETTINGS";
		// API 9

		/// <summary>
		/// Show settings to allow configuration of application development-related settings. As of JELLY_BEAN_MR1 this action is a required part of the platform.
		/// </summary>
		public const string ACTION_APPLICATION_DEVELOPMENT_SETTINGS =
			"android.settings.APPLICATION_DEVELOPMENT_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to allow configuration of application-related settings.
		/// </summary>
		public const string ACTION_APPLICATION_SETTINGS = "android.settings.APPLICATION_SETTINGS";
		// API 1

		/// <summary>
		/// Show battery saver settings.
		/// </summary>
		public const string ACTION_BATTERY_SAVER_SETTINGS = "android.settings.BATTERY_SAVER_SETTINGS";
		// API 22

		/// <summary>
		/// Show settings to allow configuration of Bluetooth.
		/// </summary>
		public const string ACTION_BLUETOOTH_SETTINGS = "android.settings.BLUETOOTH_SETTINGS";
		// API 1

		/// <summary>
		/// Show settings for video captioning.
		/// </summary>
		public const string ACTION_CAPTIONING_SETTINGS = "android.settings.CAPTIONING_SETTINGS";
		// API 19

		/// <summary>
		/// Show settings to allow configuration of cast endpoints.
		/// </summary>
		public const string ACTION_CAST_SETTINGS = "android.settings.CAST_SETTINGS";
		// API 21

		/// <summary>
		/// Show settings for selection of 2G/3G.
		/// </summary>
		public const string ACTION_DATA_ROAMING_SETTINGS = "android.settings.DATA_ROAMING_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to allow configuration of date and time.
		/// </summary>
		public const string ACTION_DATE_SETTINGS = "android.settings.DATE_SETTINGS";
		// API 1

		/// <summary>
		/// Show general device information settings (serial number, software version, phone number, etc.).
		/// </summary>
		public const string ACTION_DEVICE_INFO_SETTINGS = "android.settings.DEVICE_INFO_SETTINGS";
		// API 8

		/// <summary>
		/// Show settings to allow configuration of display.
		/// </summary>
		public const string ACTION_DISPLAY_SETTINGS = "android.settings.DISPLAY_SETTINGS";
		// API 1

		/// <summary>
		/// Show Daydream settings.
		/// </summary>
		public const string ACTION_DREAM_SETTINGS = "android.settings.DREAM_SETTINGS";
		// API 18

		/// <summary>
		/// If there are multiple activities that can satisfy the CATEGORY_HOME intent, this screen allows you to pick your preferred activity.
		/// </summary>
		public const string ACTION_HOME_SETTINGS = "android.settings.HOME_SETTINGS";
		// API 21

		/// <summary>
		/// Show settings to configure input methods, in particular allowing the user to enable input methods.
		/// </summary>
		public const string ACTION_INPUT_METHOD_SETTINGS = "android.settings.INPUT_METHOD_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to enable/disable input method subtypes.
		/// </summary>
		public const string ACTION_INPUT_METHOD_SUBTYPE_SETTINGS = "android.settings.INPUT_METHOD_SUBTYPE_SETTINGS";
		// API 11

		/// <summary>
		/// Show settings for internal storage.
		/// </summary>
		public const string ACTION_INTERNAL_STORAGE_SETTINGS = "android.settings.INTERNAL_STORAGE_SETTINGS";
		// API 3

		/// <summary>
		///  Show settings to allow configuration of locale.
		/// </summary>
		public const string ACTION_LOCALE_SETTINGS = "android.settings.LOCALE_SETTINGS";
		// API 1

		/// <summary>
		/// Show settings to allow configuration of current location sources.
		/// </summary>
		public const string ACTION_LOCATION_SOURCE_SETTINGS = "android.settings.LOCATION_SOURCE_SETTINGS";
		// API 1

		/// <summary>
		/// Show settings to manage all applications.
		/// </summary>
		public const string ACTION_MANAGE_ALL_APPLICATIONS_SETTINGS =
			"android.settings.MANAGE_ALL_APPLICATIONS_SETTINGS";
		// API 9

		/// <summary>
		/// Show settings to manage installed applications.
		/// </summary>
		public const string ACTION_MANAGE_APPLICATIONS_SETTINGS = "android.settings.MANAGE_APPLICATIONS_SETTINGS";
		// API 3

		/// <summary>
		/// Show screen for controlling which apps can draw on top of other apps.
		/// </summary>
		public const string ACTION_MANAGE_OVERLAY_PERMISSION = "android.settings.MANAGE_APPLICATIONS_SETTINGS";
		// API 23

		/// <summary>
		/// Show screen for controlling which apps are allowed to write/modify system settings.
		/// </summary>
		public const string ACTION_MANAGE_WRITE_SETTINGS = "android.settings.MANAGE_WRITE_SETTINGS";
		// API 23

		/// <summary>
		/// Show settings for memory card storage.
		/// </summary>
		public const string ACTION_MEMORY_CARD_SETTINGS = "android.settings.MEMORY_CARD_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings for selecting the network operator.
		/// </summary>
		public const string ACTION_NETWORK_OPERATOR_SETTINGS = "android.settings.NETWORK_OPERATOR_SETTINGS";
		// API 3

		/// <summary>
		/// Show NFC Sharing settings.
		/// </summary>
		public const string ACTION_NFCSHARING_SETTINGS = "android.settings.NFCSHARING_SETTINGS";
		// API 14

		/// <summary>
		/// Show NFC Tap & Pay settings
		/// </summary>
		public const string ACTION_NFC_PAYMENT_SETTINGS = "android.settings.NFC_PAYMENT_SETTINGS";
		// API 19

		/// <summary>
		/// Show NFC settings.
		/// </summary>
		public const string ACTION_NFC_SETTINGS = "android.settings.NFC_SETTINGS";
		// API 16

		/// <summary>
		/// Show Notification listener settings.
		/// </summary>
		public const string ACTION_NOTIFICATION_LISTENER_SETTINGS =
			"android.settings.ACTION_NOTIFICATION_LISTENER_SETTINGS";
		// API 22

		/// <summary>
		/// Show Do Not Disturb access settings.
		/// </summary>
		public const string ACTION_NOTIFICATION_POLICY_ACCESS_SETTINGS =
			"android.settings.NOTIFICATION_POLICY_ACCESS_SETTINGS";
		// API 23

		/// <summary>
		/// Show the top level print settings.
		/// </summary>
		public const string ACTION_PRINT_SETTINGS = "android.settings.ACTION_PRINT_SETTINGS";
		// API 19

		/// <summary>
		/// Show settings to allow configuration of privacy options.
		/// </summary>
		public const string ACTION_PRIVACY_SETTINGS = "android.settings.PRIVACY_SETTINGS";
		// API 5

		/// <summary>
		/// Show settings to allow configuration of quick launch shortcuts.
		/// </summary>
		public const string ACTION_QUICK_LAUNCH_SETTINGS = "android.settings.QUICK_LAUNCH_SETTINGS";
		// API 3

		/// <summary>
		/// Ask the user to allow an app to ignore battery optimizations (that is, put them on the whitelist of apps shown by ACTION_IGNORE_BATTERY_OPTIMIZATION_SETTINGS). For an app to use this, it also must hold the REQUEST_IGNORE_BATTERY_OPTIMIZATIONS permission.
		/// </summary>
		public const string ACTION_REQUEST_IGNORE_BATTERY_OPTIMIZATIONS =
			"android.settings.REQUEST_IGNORE_BATTERY_OPTIMIZATIONS";
		// API 23

		/// <summary>
		/// Show settings for global search.
		/// </summary>
		public const string ACTION_SEARCH_SETTINGS = "android.settings.SEARCH_SETTINGS";
		// API 8

		/// <summary>
		/// Show settings to allow configuration of security and location privacy.
		/// </summary>
		public const string ACTION_SECURITY_SETTINGS = "android.settings.SECURITY_SETTINGS";
		// API 1

		/// <summary>
		/// Show the regulatory information screen for the device.
		/// </summary>
		public const string ACTION_SHOW_REGULATORY_INFO = "android.settings.SHOW_REGULATORY_INFO";
		// API 1

		/// <summary>
		/// Show settings to allow configuration of sound and volume.
		/// </summary>
		public const string ACTION_SOUND_SETTINGS = "android.settings.SOUND_SETTINGS";
		// API 1

		/// <summary>
		/// Show settings to allow configuration of sync settings.
		/// </summary>
		public const string ACTION_SYNC_SETTINGS = "android.settings.SYNC_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to control access to usage information.
		/// </summary>
		public const string ACTION_USAGE_ACCESS_SETTINGS = "android.settings.USAGE_ACCESS_SETTINGS";
		// API 21

		/// <summary>
		/// Show settings to manage the user input dictionary.
		/// </summary>
		public const string ACTION_USER_DICTIONARY_SETTINGS = "android.settings.USER_DICTIONARY_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to configure input methods, in particular allowing the user to enable input methods.
		/// </summary>
		public const string ACTION_VOICE_INPUT_SETTINGS = "android.settings.VOICE_INPUT_SETTINGS";
		// API 21

		/// <summary>
		/// Show settings to allow configuration of a static IP address for Wi-Fi.
		/// </summary>
		public const string ACTION_WIFI_IP_SETTINGS = "android.settings.WIFI_IP_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to allow configuration of Wi-Fi.
		/// </summary>
		public const string ACTION_WIFI_SETTINGS = "android.settings.WIFI_SETTINGS";
		// API 3

		/// <summary>
		/// Show settings to allow configuration of wireless controls such as Wi-Fi, Bluetooth and Mobile networks.
		/// </summary>
		public const string ACTION_WIRELESS_SETTINGS = "android.settings.WIRELESS_SETTINGS";
		// API 1

		/// <summary>
		/// Check if the current device can open the specified settings screen.
		/// </summary>
		/// <returns><c>true</c> if the current device can open the specified settings screen; otherwise, <c>false</c>.</returns>
		/// <param name="action">Action.</param>
		public static bool CanOpenSettingsScreen(string action)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			var intent = new AndroidIntent(action);
			return intent.ResolveActivity();
		}

		#region API

		/// <summary>
		/// Opens android main settings
		/// </summary>
		public static void OpenSettings()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			OpenSettingsScreen(ACTION_SETTINGS);
		}

		#endregion

		/// <summary>
		/// Opens the provided settings screen
		/// </summary>
		/// <param name="action">
		/// Screen to open. Use on of actions provided as constants in this class. Check android.provider.Settings java class for more info
		/// </param>
		public static void OpenSettingsScreen(string action)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			var intent = new AndroidIntent(action);
			if (intent.ResolveActivity())
			{
				AGUtils.StartActivity(intent.AJO);
			}
			else
			{
				Debug.LogWarning("Could not launch " + action + " settings. Check the device API level");
			}
		}

		/// <summary>
		/// Open application details settings
		/// </summary>
		/// <param name="package">Package of the application to open settings</param>
		public static void OpenApplicationDetailsSettings(string package)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			var intent = new AndroidIntent(ACTION_APPLICATION_DETAILS_SETTINGS);
			intent.SetData(AndroidUri.Parse(string.Format("package:{0}", package)));
			if (intent.ResolveActivity())
			{
				AGUtils.StartActivity(intent.AJO);
			}
			else
			{
				Debug.LogWarning("Could not open application details settings for package " + package +
				                 ". Most likely application is not installed.");
			}
		}

		#region system_settings_general

		/// <summary>
		/// Determines if the application can write system settings.
		/// </summary>
		/// <returns><c>true</c> if the application can write system settings; otherwise, <c>false</c>.</returns>
		/// 
		/// See: http://stackoverflow.com/questions/32083410/cant-get-write-settings-permission
		public static bool CanWriteSystemSettings()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			return AndroidSettings.System.CanWrite();
		}

		/// <summary>
		/// Opens the activity to modify system settings activity.
		/// </summary>
		/// <param name="onFailure">Invoked if activity could not be started.</param>
		public static void OpenModifySystemSettingsActivity(Action onFailure)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			var intent = new AndroidIntent(AndroidSettings.ACTION_MANAGE_WRITE_SETTINGS).AJO;
			AGUtils.StartActivity(intent, onFailure);
		}

		/// <summary>
		/// Gets the system screen brightness. The value is between 0 and 1
		/// </summary>
		/// <returns>The system screen brightness.</returns>
		public static float GetSystemScreenBrightness()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0;
			}

			var brightnessInt = AndroidSettings.System.GetInt(AndroidSettings.System.SCREEN_BRIGHTNESS, 1);
			return brightnessInt / 255f;
		}

		#endregion

		#region screen_brightness

		/// <summary>
		/// Sets the system screen brightness. The vaue must be between 0 and 1 and will be clamped if it is not.
		/// 
		/// Before invoking the method you have to check with <see cref="CanWriteSystemSettings"/>  if user allowed to write system settings.
		/// If not prompt the user to open the screen where user can modify permissions for your app using <see cref="OpenModifySystemSettingsActivity"/>
		/// 
		/// REQUIRED PERMISSION:
		///      <uses-permission android:name="android.permission.WRITE_SETTINGS"/>
		/// </summary>
		/// <param name="newBrightness">New brightness to set.</param>
		public static void SetSystemScreenBrightness(float newBrightness)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			newBrightness = Mathf.Clamp01(newBrightness);

			if (!CanWriteSystemSettings())
			{
				Debug.LogError("The application does not have the permission to modify system settings." +
				               " Check before invoking this method by invoking 'CanWriteSystemSettings()' and use 'OpenModifySystemSettingsActivity()' to prompt the use to allow");
				return;
			}

			try
			{
				AndroidSettings.System.PutInt(AndroidSettings.System.SCREEN_BRIGHTNESS_MODE,
					AndroidSettings.System.SCREEN_BRIGHTNESS_MODE_MANUAL);
				int brightnessInt = (int) (newBrightness * 255);
				AndroidSettings.System.PutInt(AndroidSettings.System.SCREEN_BRIGHTNESS, brightnessInt);
			}
			catch (Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogException(e);
				}
			}
		}

		// TODO Window Brightness
		// WindowManager.LayoutParams layout = getWindow().getAttributes();
		// layout.screenBrightness = 1F;
		// getWindow().setAttributes(layout);
		static void SetWindowScreenBrightness(float newBrightness)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			newBrightness = Mathf.Clamp01(newBrightness);

			var window = AGUtils.Window;
			var layout = window.CallAJO("getAttributes");
			layout.SetStatic<float>("screenBrightness", newBrightness);
			window.Call("setAttributes", layout);
		}

		#endregion
	}
}

#endif
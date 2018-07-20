namespace AndroidGoodiesExamples
{
	using DeadMosquito.AndroidGoodies;
	using UnityEngine;
	using UnityEngine.UI;

	public class OpenSettingsTest : MonoBehaviour
	{
		public Slider slider;

#if UNITY_ANDROID
		void Awake()
		{
			InitModifyScreenBrightness();
		}

		void InitModifyScreenBrightness()
		{
			slider.onValueChanged.AddListener(newBrightness =>
			{
				if (!AGSettings.CanWriteSystemSettings())
				{
					AGUIMisc.ShowToast("Can't modify system settings because user did not allow it!");
					return;
				}
				AGSettings.SetSystemScreenBrightness(newBrightness);
			});
		}

		public void OnOpenSettings()
		{
			AGSettings.OpenSettings();
		}

		public void OnOpenWifiSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_WIFI_SETTINGS);
		}

		public void OnOpenBluetoothSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_BLUETOOTH_SETTINGS);
		}

		public void OnOpenDataSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_DATA_ROAMING_SETTINGS);
		}

		public void OnOpenDisplaySettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_DISPLAY_SETTINGS);
		}

		public void OnOpenSoundSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_SOUND_SETTINGS);
		}

		public void OnOpenAppsSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_APPLICATION_SETTINGS);
		}

		public void OnOpenStorageSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_INTERNAL_STORAGE_SETTINGS);
		}

		public void OnOpenBatterySettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_BATTERY_SAVER_SETTINGS);
		}

		public void OnOpenMemoryCardSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_MEMORY_CARD_SETTINGS);
		}

		public void OnOpenLocationSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_LOCATION_SOURCE_SETTINGS);
		}

		public void OnOpenSecuritySettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_SECURITY_SETTINGS);
		}

		public void OnOpenLocaleSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_LOCALE_SETTINGS);
		}

		public void OnOpenDateSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_DATE_SETTINGS);
		}

		public void OnOpenAccessibilitySettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_ACCESSIBILITY_SETTINGS);
		}

		public void OnOpenDeveloperSettings()
		{
			AGSettings.OpenSettingsScreen(AGSettings.ACTION_APPLICATION_DEVELOPMENT_SETTINGS);
		}

		public void OnOpenCurrentAppSettings()
		{
			AGSettings.OpenApplicationDetailsSettings(AGDeviceInfo.GetApplicationPackage());
		}

		#region system_settings

		public void OpenModifySystemSettings()
		{
			AGSettings.OpenModifySystemSettingsActivity(() =>
				AGUIMisc.ShowToast("Could Not find Modify System Settings Activity"));
		}

		public void CheckIfAppCanWriteSystemSettings()
		{
			bool canWriteSystemSettings = AGSettings.CanWriteSystemSettings();
			AGUIMisc.ShowToast("Can write system settings? : " + canWriteSystemSettings);
		}

		public void CheckBrightness()
		{
			var brightness = AGSettings.GetSystemScreenBrightness();
			AGUIMisc.ShowToast("System screen brighness is : " + brightness);
		}

		#endregion
#endif
	}
}

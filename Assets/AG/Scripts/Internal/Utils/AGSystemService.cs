#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using UnityEngine;

	public static class AGSystemService
	{
		const string VIBRATOR_SERVICE = "vibrator";
		static AndroidJavaObject _vibratorService;

		const string LOCATION_SERVICE = "location";
		static AndroidJavaObject _locationService;

		const string CONNECTIVITY_SERVICE = "connectivity";
		static AndroidJavaObject _connectivityService;

		const string WIFI_SERVICE = "wifi";
		static AndroidJavaObject _wifiService;

		const string TELEPHONY_SERVICE = "phone";
		static AndroidJavaObject _telephonyService;

		const string NOTIFICATION_SERVICE = "notification";
		static AndroidJavaObject _notificationService;

		const string CAMERA_SERVICE = "camera";
		static AndroidJavaObject _cameraService;

		const string CLIPBOARD_SERVICE = "clipboard";
		static AndroidJavaObject _clipboardService;

		public static AndroidJavaObject VibratorService
		{
			get { return _vibratorService ?? (_vibratorService = GetSystemService(VIBRATOR_SERVICE, C.AndroidOsVibrator)); }
		}

		public static AndroidJavaObject LocationService
		{
			get { return _locationService ?? (_locationService = GetSystemService(LOCATION_SERVICE, C.AndroidLocaltionLocationManager)); }
		}

		public static AndroidJavaObject ConnectivityService
		{
			get { return _connectivityService ?? (_connectivityService = GetSystemService(CONNECTIVITY_SERVICE, C.AndroidNetConnectivityManager)); }
		}

		public static AndroidJavaObject WifiService
		{
			get { return _wifiService ?? (_wifiService = GetSystemService(WIFI_SERVICE, C.AndroidNetWifiManager)); }
		}

		public static AndroidJavaObject TelephonyService
		{
			get { return _telephonyService ?? (_telephonyService = GetSystemService(TELEPHONY_SERVICE, C.AndroidTelephonyTelephonyManager)); }
		}

		public static AndroidJavaObject NotificationService
		{
			get { return _notificationService ?? (_notificationService = GetSystemService(NOTIFICATION_SERVICE, C.AndroidAppNotificationManager)); }
		}

		public static AndroidJavaObject NotificationServiceCompat
		{
			get { return _notificationService ?? (_notificationService = C.AndroidAppNotificationManagerCompat.AJCCallStaticOnceAJO("from", AGUtils.Activity)); }
		}

		public static AndroidJavaObject CameraService
		{
			get { return _cameraService ?? (_cameraService = GetSystemService(CAMERA_SERVICE, C.AndroidHardwareCamera2CameraManager)); }
		}

		public static AndroidJavaObject ClipboardService
		{
			get { return _clipboardService ?? (_clipboardService = GetSystemService(CLIPBOARD_SERVICE, C.AndroidContentClipboardManager)); }
		}

		static AndroidJavaObject GetSystemService(string name, string serviceClass)
		{
			try
			{
				var serviceObj = AGUtils.Activity.CallAJO("getSystemService", name);
				return serviceObj.Cast(serviceClass);
			}
			catch (Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("Failed to get " + name + " service. Error: " + e.Message);
				}

				return null;
			}
		}
	}
}
#endif
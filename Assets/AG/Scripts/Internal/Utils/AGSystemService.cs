
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
			get
			{
				if (_vibratorService == null)
				{
					_vibratorService = GetSystemService(VIBRATOR_SERVICE, C.AndroidOsVibrator);
				}
				return _vibratorService;
			}
		}

		public static AndroidJavaObject LocationService
		{
			get
			{
				if (_locationService == null)
				{
					_locationService = GetSystemService(LOCATION_SERVICE, C.AndroidLocaltionLocationManager);
				}
				return _locationService;
			}
		}

		public static AndroidJavaObject ConnectivityService
		{
			get
			{
				if (_connectivityService == null)
				{
					_connectivityService = GetSystemService(CONNECTIVITY_SERVICE, C.AndroidNetConnectivityManager);
				}
				return _connectivityService;
			}
		}

		public static AndroidJavaObject WifiService
		{
			get
			{
				if (_wifiService == null)
				{
					_wifiService = GetSystemService(WIFI_SERVICE, C.AndroidNetWifiManager);
				}
				return _wifiService;
			}
		}

		public static AndroidJavaObject TelephonyService
		{
			get
			{
				if (_telephonyService == null)
				{
					_telephonyService = GetSystemService(TELEPHONY_SERVICE, C.AndroidTelephonyTelephonyManager);
				}
				return _telephonyService;
			}
		}

		public static AndroidJavaObject NotificationService
		{
			get
			{
				if (_notificationService == null)
				{
					_notificationService = GetSystemService(NOTIFICATION_SERVICE, C.AndroidAppNotificationManager);
				}
				return _notificationService;
			}
		}

		public static AndroidJavaObject CameraService
		{
			get
			{
				if (_cameraService == null)
				{
					_cameraService = GetSystemService(CAMERA_SERVICE, C.AndroidHardwareCamera2CameraManager);
				}
				return _cameraService;
			}
		}
		
		public static AndroidJavaObject ClipboardService
		{
			get
			{
				if (_clipboardService == null)
				{
					_clipboardService = GetSystemService(CLIPBOARD_SERVICE, C.AndroidContentClipboardManager);
				}
				return _clipboardService;
			}
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
// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGDeviceInfo.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	public static class AGDeviceInfo
	{
		/// <summary>
		/// Enumeration of the currently known SDK version codes. These are the values that can be found in SDK.
		/// Version numbers increment monotonically with each official platform release.
		/// </summary>
		[PublicAPI]
		public static class VersionCodes
		{
			/// <summary>
			/// October 2008: The original, first, version of Android. Yay!
			/// </summary>
			[PublicAPI]
			public const int BASE = 1;

			/// <summary>
			/// February 2009: First Android update, officially called 1.1.
			/// </summary>
			[PublicAPI]
			public const int BASE_1_1 = 2;

			/// <summary>
			/// The CUPCAKE.
			/// </summary>
			[PublicAPI]
			public const int CUPCAKE = 3;

			/// <summary>
			/// Magic version number for a current development build, which has not yet turned into an official release.
			/// </summary>
			[PublicAPI]
			public const int CUR_DEVELOPMENT = 10000;

			/// <summary>
			/// September 2009: Android 1.6.
			/// </summary>
			[PublicAPI]
			public const int DONUT = 4;

			/// <summary>
			/// November 2009: Android 2.0
			/// </summary>
			[PublicAPI]
			public const int ECLAIR = 5;

			/// <summary>
			/// December 2009: Android 2.0.1
			/// </summary>
			[PublicAPI]
			public const int ECLAIR_0_1 = 6;

			/// <summary>
			/// January 2010: Android 2.1
			/// </summary>
			[PublicAPI]
			public const int ECLAIR_MR1 = 7;

			/// <summary>
			/// June 2010: Android 2.2
			/// </summary>
			[PublicAPI]
			public const int FROYO = 8;

			/// <summary>
			/// November 2010: Android 2.3
			/// </summary>
			[PublicAPI]
			public const int GINGERBREAD = 9;

			/// <summary>
			/// February 2011: Android 2.3.3.
			/// </summary>
			[PublicAPI]
			public const int GINGERBREAD_MR1 = 10;

			/// <summary>
			/// February 2011: Android 3.0.
			/// </summary>
			[PublicAPI]
			public const int HONEYCOMB = 11;

			/// <summary>
			/// May 2011: Android 3.1.
			/// </summary>
			[PublicAPI]
			public const int HONEYCOMB_MR1 = 12;

			/// <summary>
			/// June 2011: Android 3.2.
			/// </summary>
			[PublicAPI]
			public const int HONEYCOMB_MR2 = 13;

			/// <summary>
			/// October 2011: Android 4.0.
			/// </summary>
			[PublicAPI]
			public const int ICE_CREAM_SANDWICH = 14;

			/// <summary>
			/// December 2011: Android 4.0.3.
			/// </summary>
			[PublicAPI]
			public const int ICE_CREAM_SANDWICH_MR1 = 15;

			/// <summary>
			/// June 2012: Android 4.1.
			/// </summary>
			[PublicAPI]
			public const int JELLY_BEAN = 16;

			/// <summary>
			/// November 2012: Android 4.2, Moar jelly beans!
			/// </summary>
			[PublicAPI]
			public const int JELLY_BEAN_MR1 = 17;

			/// <summary>
			/// July 2013: Android 4.3, the revenge of the beans.
			/// </summary>
			[PublicAPI]
			public const int JELLY_BEAN_MR2 = 18;

			/// <summary>
			/// October 2013: Android 4.4, KitKat, another tasty treat.
			/// </summary>
			[PublicAPI]
			public const int KITKAT = 19;

			/// <summary>
			/// June 2014: Android 4.4W. KitKat for watches, snacks on the run.
			/// </summary>
			[PublicAPI]
			public const int KITKAT_WATCH = 20;

			/// <summary>
			/// November 2014: Lollipop. A flat one with beautiful shadows. But still tasty.
			/// </summary>
			[PublicAPI]
			public const int LOLLIPOP = 21;

			/// <summary>
			/// March 2015: Lollipop with an extra sugar coating on the outside!
			/// </summary>
			[PublicAPI]
			public const int LOLLIPOP_MR1 = 22;

			/// <summary>
			/// M is for Marshmallow!
			/// </summary>
			[PublicAPI]
			public const int M = 23;

			/// <summary>
			/// N is for Nougat.
			/// </summary>
			[PublicAPI]
			public const int N = 24;
			
			/// <summary>
			/// N MR1: Nougat++.
			/// </summary>
			[PublicAPI]
			public const int N_MR1 = 25;
			
			/// <summary>
			/// Android O
			/// </summary>
			[PublicAPI]
			public const int O = 26;
		}

		/// <summary>
		/// Class to check if device has the specified system feature
		/// </summary>
		[PublicAPI]
		public static class SystemFeatures
		{
			[PublicAPI]
			public const string FEATURE_AUDIO_LOW_LATENCY = "android.hardware.audio.low_latency";
			[PublicAPI]
			public const string FEATURE_AUDIO_OUTPUT = "android.hardware.audio.output";
			[PublicAPI]
			public const string FEATURE_AUDIO_PRO = "android.hardware.audio.pro";
			[PublicAPI]
			public const string FEATURE_BLUETOOTH = "android.hardware.bluetooth";
			[PublicAPI]
			public const string FEATURE_BLUETOOTH_LE = "android.hardware.bluetooth_le";
			[PublicAPI]
			public const string FEATURE_CAMERA = "android.hardware.camera";
			[PublicAPI]
			public const string FEATURE_CAMERA_AUTOFOCUS = "android.hardware.camera.autofocus";
			[PublicAPI]
			public const string FEATURE_CAMERA_ANY = "android.hardware.camera.any";
			[PublicAPI]
			public const string FEATURE_CAMERA_EXTERNAL = "android.hardware.camera.external";
			[PublicAPI]
			public const string FEATURE_CAMERA_FLASH = "android.hardware.camera.flash";
			[PublicAPI]
			public const string FEATURE_CAMERA_FRONT = "android.hardware.camera.front";
			[PublicAPI]
			public const string FEATURE_CAMERA_LEVEL_FULL = "android.hardware.camera.level.full";
			[PublicAPI]
			public const string FEATURE_CAMERA_CAPABILITY_MANUAL_SENSOR = "android.hardware.camera.capability.manual_sensor";
			[PublicAPI]
			public const string FEATURE_CAMERA_CAPABILITY_MANUAL_POST_PROCESSING = "android.hardware.camera.capability.manual_post_processing";
			[PublicAPI]
			public const string FEATURE_CAMERA_CAPABILITY_RAW = "android.hardware.camera.capability.raw";
			[PublicAPI]
			public const string FEATURE_CONSUMER_IR = "android.hardware.consumerir";
			[PublicAPI]
			public const string FEATURE_LOCATION = "android.hardware.location";
			[PublicAPI]
			public const string FEATURE_LOCATION_GPS = "android.hardware.location.gps";
			[PublicAPI]
			public const string FEATURE_LOCATION_NETWORK = "android.hardware.location.network";
			[PublicAPI]
			public const string FEATURE_MICROPHONE = "android.hardware.microphone";
			[PublicAPI]
			public const string FEATURE_NFC = "android.hardware.nfc";
			[PublicAPI]
			public const string FEATURE_NFC_HCE = "android.hardware.nfc.hce";
			[PublicAPI]
			public const string FEATURE_NFC_HOST_CARD_EMULATION = "android.hardware.nfc.hce";
			[PublicAPI]
			public const string FEATURE_OPENGLES_EXTENSION_PACK = "android.hardware.opengles.aep";
			[PublicAPI]
			public const string FEATURE_SENSOR_ACCELEROMETER = "android.hardware.sensor.accelerometer";
			[PublicAPI]
			public const string FEATURE_SENSOR_BAROMETER = "android.hardware.sensor.barometer";
			[PublicAPI]
			public const string FEATURE_SENSOR_COMPASS = "android.hardware.sensor.compass";
			[PublicAPI]
			public const string FEATURE_SENSOR_GYROSCOPE = "android.hardware.sensor.gyroscope";
			[PublicAPI]
			public const string FEATURE_SENSOR_LIGHT = "android.hardware.sensor.light";
			[PublicAPI]
			public const string FEATURE_SENSOR_PROXIMITY = "android.hardware.sensor.proximity";
			[PublicAPI]
			public const string FEATURE_SENSOR_STEP_COUNTER = "android.hardware.sensor.stepcounter";
			[PublicAPI]
			public const string FEATURE_SENSOR_STEP_DETECTOR = "android.hardware.sensor.stepdetector";
			[PublicAPI]
			public const string FEATURE_SENSOR_HEART_RATE = "android.hardware.sensor.heartrate";
			[PublicAPI]
			public const string FEATURE_SENSOR_HEART_RATE_ECG = "android.hardware.sensor.heartrate.ecg";
			[PublicAPI]
			public const string FEATURE_SENSOR_RELATIVE_HUMIDITY = "android.hardware.sensor.relative_humidity";
			[PublicAPI]
			public const string FEATURE_SENSOR_AMBIENT_TEMPERATURE = "android.hardware.sensor.ambient_temperature";
			[PublicAPI]
			public const string FEATURE_HIFI_SENSORS = "android.hardware.sensor.hifi_sensors";
			[PublicAPI]
			public const string FEATURE_TELEPHONY = "android.hardware.telephony";
			[PublicAPI]
			public const string FEATURE_TELEPHONY_CDMA = "android.hardware.telephony.cdma";
			[PublicAPI]
			public const string FEATURE_TELEPHONY_GSM = "android.hardware.telephony.gsm";
			[PublicAPI]
			public const string FEATURE_USB_HOST = "android.hardware.usb.host";
			[PublicAPI]
			public const string FEATURE_USB_ACCESSORY = "android.hardware.usb.accessory";
			[PublicAPI]
			public const string FEATURE_SIP = "android.software.sip";
			[PublicAPI]
			public const string FEATURE_SIP_VOIP = "android.software.sip.voip";
			[PublicAPI]
			public const string FEATURE_CONNECTION_SERVICE = "android.software.connectionservice";
			[PublicAPI]
			public const string FEATURE_TOUCHSCREEN = "android.hardware.touchscreen";
			[PublicAPI]
			public const string FEATURE_TOUCHSCREEN_MULTITOUCH = "android.hardware.touchscreen.multitouch";
			[PublicAPI]
			public const string FEATURE_TOUCHSCREEN_MULTITOUCH_DISTINCT = "android.hardware.touchscreen.multitouch.distinct";
			[PublicAPI]
			public const string FEATURE_TOUCHSCREEN_MULTITOUCH_JAZZHAND = "android.hardware.touchscreen.multitouch.jazzhand";
			[PublicAPI]
			public const string FEATURE_FAKETOUCH = "android.hardware.faketouch";
			[PublicAPI]
			public const string FEATURE_FAKETOUCH_MULTITOUCH_DISTINCT = "android.hardware.faketouch.multitouch.distinct";
			[PublicAPI]
			public const string FEATURE_FAKETOUCH_MULTITOUCH_JAZZHAND = "android.hardware.faketouch.multitouch.jazzhand";
			[PublicAPI]
			public const string FEATURE_FINGERPRINT = "android.hardware.fingerprint";
			[PublicAPI]
			public const string FEATURE_SCREEN_PORTRAIT = "android.hardware.screen.portrait";
			[PublicAPI]
			public const string FEATURE_SCREEN_LANDSCAPE = "android.hardware.screen.landscape";
			[PublicAPI]
			public const string FEATURE_LIVE_WALLPAPER = "android.software.live_wallpaper";
			[PublicAPI]
			public const string FEATURE_APP_WIDGETS = "android.software.app_widgets";
			[PublicAPI]
			public const string FEATURE_VOICE_RECOGNIZERS = "android.software.voice_recognizers";
			[PublicAPI]
			public const string FEATURE_HOME_SCREEN = "android.software.home_screen";
			[PublicAPI]
			public const string FEATURE_INPUT_METHODS = "android.software.input_methods";
			[PublicAPI]
			public const string FEATURE_DEVICE_ADMIN = "android.software.device_admin";
			[PublicAPI]
			public const string FEATURE_LEANBACK = "android.software.leanback";
			[PublicAPI]
			public const string FEATURE_LEANBACK_ONLY = "android.software.leanback_only";
			[PublicAPI]
			public const string FEATURE_LIVE_TV = "android.software.live_tv";
			[PublicAPI]
			public const string FEATURE_WIFI = "android.hardware.wifi";
			[PublicAPI]
			public const string FEATURE_WIFI_DIRECT = "android.hardware.wifi.direct";
			[PublicAPI]
			public const string FEATURE_AUTOMOTIVE = "android.hardware.type.automotive";
			[PublicAPI]
			public const string FEATURE_WATCH = "android.hardware.type.watch";
			[PublicAPI]
			public const string FEATURE_PRINTING = "android.software.print";
			[PublicAPI]
			public const string FEATURE_BACKUP = "android.software.backup";
			[PublicAPI]
			public const string FEATURE_MANAGED_USERS = "android.software.managed_users";
			[PublicAPI]
			public const string FEATURE_MANAGED_PROFILES = "android.software.managed_users";
			[PublicAPI]
			public const string FEATURE_VERIFIED_BOOT = "android.software.verified_boot";
			[PublicAPI]
			public const string FEATURE_SECURELY_REMOVES_USERS = "android.software.securely_removes_users";
			[PublicAPI]
			public const string FEATURE_WEBVIEW = "android.software.webview";
			[PublicAPI]
			public const string FEATURE_ETHERNET = "android.hardware.ethernet";
			[PublicAPI]
			public const string FEATURE_HDMI_CEC = "android.hardware.hdmi.cec";
			[PublicAPI]
			public const string FEATURE_GAMEPAD = "android.hardware.gamepad";
			[PublicAPI]
			public const string FEATURE_MIDI = "android.software.midi";

			[PublicAPI]
			public static bool HasFlashlight
			{
				get { return AGUtils.HasSystemFeature(FEATURE_CAMERA_FLASH); }
			}

			/// <summary>
			/// Check if the device supports the feature, else false.
			/// </summary>
			/// <returns><c>true</c> if the device supports the feature; otherwise, <c>false</c>.</returns>
			/// <param name="feature">Feature to check.</param>
			[PublicAPI]
			public static bool HasSystemFeature(string feature)
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return false;
				}

				return AGUtils.HasSystemFeature(feature);
			}
		}

		// https://developer.android.com/reference/android/provider/Settings.Secure.html#ANDROID_ID
		/// <summary>
		/// A 64-bit number (as a hex string) that is randomly generated when the user first sets up the device and should remain constant for the lifetime of the user's device.
		/// The value may change if a factory reset is performed on the device.
		/// </summary>
		/// <returns>The unique identifier of the device.</returns>
		[PublicAPI]
		public static string GetAndroidId()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}

			try
			{
				using (var objResolver = AGUtils.ContentResolver)
				{
					using (var clsSecure = new AndroidJavaClass(C.AndroidProviderSettingsSecure))
					{
						string ANDROID_ID = clsSecure.GetStaticStr("ANDROID_ID");
						string androidId = clsSecure.CallStaticStr("getString", objResolver, ANDROID_ID);
						return androidId;
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogWarning("Failed to get Android Id, reasong: " + e.Message);
				return string.Empty;
			}
		}

		#region android.os.Build

		/// <summary>
		/// The name of the industrial design.
		/// </summary>
		[PublicAPI]
		public static string DEVICE
		{
			get { return GetBuildClassStaticStr("DEVICE"); }
		}

		/// <summary>
		/// The end-user-visible name for the end product.
		/// </summary>
		[PublicAPI]
		public static string MODEL
		{
			get { return GetBuildClassStaticStr("MODEL"); }
		}

		/// <summary>
		/// The name of the overall product.
		/// </summary>
		[PublicAPI]
		public static string PRODUCT
		{
			get { return GetBuildClassStaticStr("PRODUCT"); }
		}

		/// <summary>
		/// The manufacturer of the product/hardware.
		/// </summary>
		[PublicAPI]
		public static string MANUFACTURER
		{
			get { return GetBuildClassStaticStr("MANUFACTURER"); }
		}

		static string GetBuildClassStaticStr(string fieldName)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}

			try
			{
				using (var version = new AndroidJavaClass(C.AndroidOsBuild))
				{
					return version.GetStaticStr(fieldName);
				}
			}
			catch (Exception e)
			{
				Debug.LogWarning("Failed to get property " + fieldName + ". Check device API level. " + e.Message);
				return string.Empty;
			}
		}

		#endregion

		#region android.os.Build$VERSION

		/// <summary>
		/// The base OS build the product is based on.
		/// </summary>
		[PublicAPI]
		public static string BASE_OS
		{
			get { return GetDeviceStrProperty<string>(C.AndroidOsBuildVersion, "BASE_OS"); }
		}

		/// <summary>
		/// The current development codename, or the string "REL" if this is a release build.
		/// </summary>
		[PublicAPI]
		public static string CODENAME
		{
			get { return GetDeviceStrProperty<string>(C.AndroidOsBuildVersion, "CODENAME"); }
		}

		/// <summary>
		/// The internal value used by the underlying source control to represent this build.
		/// </summary>
		[PublicAPI]
		public static string INCREMENTAL
		{
			get { return GetDeviceStrProperty<string>(C.AndroidOsBuildVersion, "INCREMENTAL"); }
		}

		/// <summary>
		/// The developer preview revision of a prerelease SDK.
		/// </summary>
		[PublicAPI]
		public static int PREVIEW_SDK_INT
		{
			get { return GetDeviceStrProperty<int>(C.AndroidOsBuildVersion, "PREVIEW_SDK_INT"); }
		}

		/// <summary>
		/// The user-visible version string.
		/// </summary>
		[PublicAPI]
		public static string RELEASE
		{
			get { return GetDeviceStrProperty<string>(C.AndroidOsBuildVersion, "RELEASE"); }
		}

		/// <summary>
		/// The user-visible SDK version of the framework; its possible values are defined in Build.VERSION_CODES.
		/// </summary>
		[PublicAPI]
		public static int SDK_INT
		{
			get { return GetDeviceStrProperty<int>(C.AndroidOsBuildVersion, "SDK_INT"); }
		}

		/// <summary>
		/// The user-visible security patch level.
		/// </summary>
		[PublicAPI]
		public static string SECURITY_PATCH
		{
			get { return GetDeviceStrProperty<string>(C.AndroidOsBuildVersion, "SECURITY_PATCH"); }
		}

		static T GetDeviceStrProperty<T>(string className, string propertyName)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return default(T);
			}

			try
			{
				using (var version = new AndroidJavaClass(className))
				{
					return version.GetStatic<T>(propertyName);
				}
			}
			catch (Exception e)
			{
				Debug.LogWarning(String.Format("Failed to get property: {0} of class {1}, reason: {2}", propertyName, className, e.Message));
				return default(T);
			}
		}

		#endregion

		/// <summary>
		/// Gets the application package.
		/// </summary>
		/// <returns>The application package.</returns>
		[PublicAPI]
		public static string GetApplicationPackage()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}

			return AGUtils.Activity.CallAJO("getApplicationContext").CallStr("getPackageName");
		}

		/// <summary>
		/// Check if the application with the provided package is installed on device
		/// </summary>
		/// <returns><c>true</c> if application with this package is installed; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool IsPackageInstalled(string package)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			using (AndroidJavaObject pm = AGUtils.PackageManager)
			{
				try
				{
					const int GET_ACTIVITIES = 1;
					pm.Call<AndroidJavaObject>("getPackageInfo", package, GET_ACTIVITIES);
					return true;
				}
				catch (AndroidJavaException e)
				{
					Debug.LogWarning(e);
					return false;
				}
			}
		}

		/// <summary>
		/// Return a List of all packages that are installed on the device.
		/// </summary>
		/// <returns>List of all packages that are installed on the device.</returns>
		[PublicAPI]
		public static List<PackageInfo> GetInstalledPackages()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return new List<PackageInfo>();
			}

			using (var pm = AGUtils.PackageManager)
			{
				var result = new List<PackageInfo>();
				var pkgAppsList = pm.CallAJO("getInstalledPackages", 0).FromJavaList<AndroidJavaObject>();
				foreach (var resolveInfo in pkgAppsList)
				{
					result.Add(PackageInfo.FromJavaObj(resolveInfo));
				}
				return result;
			}
		}
	}
}
#endif
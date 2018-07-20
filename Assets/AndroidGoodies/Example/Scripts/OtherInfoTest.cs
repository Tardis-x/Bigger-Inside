
namespace AndroidGoodiesExamples
{
	using System.Linq;
	using System.Text;
	using DeadMosquito.AndroidGoodies;
	using UnityEngine;
	using UnityEngine.UI;

	public class OtherInfoTest : MonoBehaviour
	{
		const string BoolKey = "bool_key";
		const string FloatKey = "float_key";
		const string IntKey = "int_key";
		const string LongKey = "long_key";
		const string StringKey = "string_key";

		string _sharedPrefsFileKey;

		public Text infoText;

#if UNITY_ANDROID
		void Start()
		{
			SetupSharedPrefs();

			var builder = new StringBuilder();
			// Device info
			builder.AppendLine("ANDROID_ID? : " + AGDeviceInfo.GetAndroidId());

			builder.AppendLine("----------- TELEPHONY------------");
			builder.AppendLine("Telephony device id? : " + AGTelephony.TelephonyDeviceId);
			builder.AppendLine("Sim serial number? : " + AGTelephony.TelephonySimSerialNumber);
			builder.AppendLine("Network country ISO? : " + AGTelephony.NetworkCountryIso);

			builder.AppendLine("----------- NETWORK------------");
			builder.AppendLine("Internet connected? : " + AGNetwork.IsInternetAvailable());
			builder.AppendLine("Wifi enabled? : " + AGNetwork.IsWifiEnabled());
			builder.AppendLine("Wifi connected? : " + AGNetwork.IsWifiConnected());
			builder.AppendLine("Wifi signal level? : " + AGNetwork.GetWifiSignalLevel() + "/100");
			builder.AppendLine("Wifi info? : " + AGNetwork.GetWifiConnectionInfo());
			
			builder.AppendLine("Ethernet mac address? : " + AGNetwork.EthernetMacAddress);

			builder.AppendLine("Mobile connected? : " + AGNetwork.IsMobileConnected());

			builder.AppendLine("----------- Build class------------");
			builder.AppendLine("DEVICE : " + AGDeviceInfo.DEVICE);
			builder.AppendLine("MODEL : " + AGDeviceInfo.MODEL);
			builder.AppendLine("PRODUCT : " + AGDeviceInfo.PRODUCT);
			builder.AppendLine("MANUFACTURER : " + AGDeviceInfo.MANUFACTURER);

			// Build.VERSION
			builder.AppendLine("-----------Build.VERSION class------------");
			builder.AppendLine("BASE_OS : " + AGDeviceInfo.BASE_OS);
			builder.AppendLine("CODENAME : " + AGDeviceInfo.CODENAME);
			builder.AppendLine("INCREMENTAL : " + AGDeviceInfo.INCREMENTAL);
			builder.AppendLine("PREVIEW_SDK_INT : " + AGDeviceInfo.PREVIEW_SDK_INT);
			builder.AppendLine("RELEASE : " + AGDeviceInfo.RELEASE);
			builder.AppendLine("SDK_INT : " + AGDeviceInfo.SDK_INT);
			builder.AppendLine("SECURITY_PATCH : " + AGDeviceInfo.SECURITY_PATCH);
			builder.AppendLine("---------------------------");

			builder.AppendLine("Twitter installed? : " + AGShare.IsTwitterInstalled());
			builder.AppendLine("Has mail app? : " + AGShare.UserHasEmailApp());
			builder.AppendLine("Has sms app? : " + AGShare.UserHasSmsApp());
			builder.AppendLine("Has maps app? : " + AGMaps.UserHasMapsApp());
			builder.AppendLine("Has calendar app? : " + AGCalendar.UserHasCalendarApp());
			builder.AppendLine("Can show alarms? : " + AGAlarmClock.CanShowListOfAlarms());
			builder.AppendLine("Can set alarms? : " + AGAlarmClock.CanSetAlarm());
			builder.AppendLine("Can set timer? : " + AGAlarmClock.CanSetTimer());
			builder.AppendLine("Has timer app? : " + AGAlarmClock.UserHasTimerApp());
			builder.AppendLine("---------------------------");
			builder.AppendLine("Battery level? : " + AGBattery.GetBatteryChargeLevel());
			builder.AppendLine("Has vibrator? : " + AGVibrator.HasVibrator());
			builder.AppendLine("Has flashlight? : " + AGFlashLight.HasFlashlight());
			builder.AppendLine("GPS enabled? : " + AGGPS.IsGPSEnabled());
			builder.AppendLine("Package? : " + AGDeviceInfo.GetApplicationPackage());

			// Environment
			builder.AppendLine("-------------ENVIRONMENT DIRS-------------");
			builder.AppendLine("Alarms dir? : " + AGEnvironment.DirectoryAlarms);
			builder.AppendLine("DCIM dir? : " + AGEnvironment.DirectoryDCIM);
			builder.AppendLine("Documents dir? : " + AGEnvironment.DirectoryDocuments);
			builder.AppendLine("Downloads dir? : " + AGEnvironment.DirectoryDownloads);
			builder.AppendLine("Movies dir? : " + AGEnvironment.DirectoryMovies);
			builder.AppendLine("Music dir? : " + AGEnvironment.DirectoryMusic);
			builder.AppendLine("Notifications dir? : " + AGEnvironment.DirectoryNotifications);
			builder.AppendLine("Pictures dir? : " + AGEnvironment.DirectoryPictures);
			builder.AppendLine("Podcasts dir? : " + AGEnvironment.DirectoryPodcasts);
			builder.AppendLine("Ringtones dir? : " + AGEnvironment.DirectoryRingtones);
			
			builder.AppendLine("-------------OTHER DIRS-------------");
			builder.AppendLine("External cache dir? : " + AGFileUtils.ExternalCacheDirectory);
			builder.AppendLine("Cache dir? : " + AGFileUtils.CacheDirectory);
			builder.AppendLine("Code cache dir? : " + AGFileUtils.CodeCacheDirectory);
			builder.AppendLine("Data dir? : " + AGFileUtils.DataDir);
			builder.AppendLine("Obb dir? : " + AGFileUtils.ObbDir);

			builder.AppendLine("\n-------------ENVIRONMENT OTHER-------------");
			builder.AppendLine("Data dir? : " + AGEnvironment.DataDirectoryPath);
			builder.AppendLine("Root dir? : " + AGEnvironment.RootDirectoryPath);
			builder.AppendLine("Download/Cache dir? : " + AGEnvironment.DownloadCacheDirectoryPath);
			builder.AppendLine("External storage dir? : " + AGEnvironment.ExternalStorageDirectoryPath);
			builder.AppendLine("External storage state? : " + AGEnvironment.ExternalStorageState);
			builder.AppendLine("External storage removable? : " + AGEnvironment.IsExternalStorageRemovable);
			builder.AppendLine("External storage emulated? : " + AGEnvironment.IsExternalStorageEmulated);
			builder.AppendLine("Alarms full path? : " +
			                   AGEnvironment.GetExternalStoragePublicDirectoryPath(AGEnvironment.DirectoryAlarms));

			builder.AppendLine("\n-------------Permissions-------------");
			builder.AppendLine("Has location permission? " +
			                   AGPermissions.IsPermissionGranted(AGPermissions.ACCESS_FINE_LOCATION));
			builder.AppendLine("Has read contacts permission? " +
			                   AGPermissions.IsPermissionGranted(AGPermissions.READ_CONTACTS));
			builder.AppendLine("Has access network state permission? " +
			                   AGPermissions.IsPermissionGranted(AGPermissions.ACCESS_NETWORK_STATE));

			builder.AppendLine("\n-------------Location-------------");
			builder.AppendLine(TestDistanceInfo());
			builder.AppendLine("Last known location: " + AGGPS.GetLastKnownGPSLocation());
			builder.AppendLine("Has GPS?: " + AGGPS.DeviceHasGPS());

			builder.AppendLine("\n-------------Camera-------------");
			builder.AppendLine("Has camera? " + AGCamera.DeviceHasCamera());
			builder.AppendLine("Has autofocus? " + AGCamera.DeviceHasCameraWithAutoFocus());
			builder.AppendLine("Has flashlight? " + AGCamera.DeviceHasCameraWithFlashlight());
			builder.AppendLine("Has frontal camera? " + AGCamera.DeviceHasFrontalCamera());

			builder.AppendLine("\n-------------Shared Prefs-------------");
			builder.AppendLine("Get bool? " + AGSharedPrefs.GetBool(_sharedPrefsFileKey, BoolKey, true));
			builder.AppendLine("Get float? " + AGSharedPrefs.GetFloat(_sharedPrefsFileKey, FloatKey, float.MaxValue));
			builder.AppendLine("Get int? " + AGSharedPrefs.GetInt(_sharedPrefsFileKey, IntKey, int.MaxValue));
			builder.AppendLine("Get long? " + AGSharedPrefs.GetLong(_sharedPrefsFileKey, LongKey, long.MaxValue));
			builder.AppendLine("Get string? " + AGSharedPrefs.GetString(_sharedPrefsFileKey, StringKey, "default"));
			var allPrefs = AGSharedPrefs.GetAll(_sharedPrefsFileKey);
			foreach (var key in allPrefs.Keys)
			{
				Debug.Log(string.Format("{0} : {1}", key, allPrefs[key]));
			}

			builder.AppendLine("\n-------------Installed Apps-------------");
			foreach (var app in AGDeviceInfo.GetInstalledPackages().Take(10))
			{
				if (!string.IsNullOrEmpty(app.PackageName))
				{
					builder.Append(app);
					builder.Append(", ");
				}
			}

			infoText.text = builder.ToString();
		}

		void SetupSharedPrefs()
		{
			_sharedPrefsFileKey = AGDeviceInfo.GetApplicationPackage() + AGDeviceInfo.GetAndroidId();

			AGSharedPrefs.Clear(_sharedPrefsFileKey);
			AGSharedPrefs.SetBool(_sharedPrefsFileKey, BoolKey, false);
			AGSharedPrefs.SetFloat(_sharedPrefsFileKey, FloatKey, float.MinValue);
			AGSharedPrefs.SetInt(_sharedPrefsFileKey, IntKey, int.MinValue);
			AGSharedPrefs.SetLong(_sharedPrefsFileKey, LongKey, long.MinValue);
			AGSharedPrefs.SetString(_sharedPrefsFileKey, StringKey, "custom");
			AGSharedPrefs.GetAll(_sharedPrefsFileKey);
		}

		string TestDistanceInfo()
		{
			var amsterdamLocation = new AGGPS.Location(GPSTest.AmsterdamLatitude, GPSTest.AmsterdamLongitude, false, 0,
				0);
			var brusselsLocation = new AGGPS.Location(GPSTest.BrusselsLatitude, GPSTest.BrusselsLongitude, false, 0, 0);
			return string.Format("Distance between Amsterdam and Brussels is approx: {0} metres",
				amsterdamLocation.DistanceTo(brusselsLocation));
		}
#endif
	}
}


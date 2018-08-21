// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGPermissions.cs
//


using JetBrains.Annotations;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using MiniJSON;
	using UnityEngine;

	/// <summary>
	/// 
	///     REQUIREMENTS:
	///         * android-support-v4.jar must be in Plugins/Android folder
	///         * goodies-bridge-debug.release.jar must be in Plugins/Android folder
	///         * You MUST specify all runtime permissions that you are requesting at runtime in your AndroidManifest.xml
	/// 
	///     Beginning in Android 6.0 (API level 23), users grant permissions to apps while the app is running, not when they
	///     install the app. When calling these mthods on Android versions below 23 they will do nothing.
	/// 
	///     More about runtime permissions: https://developer.android.com/training/permissions/requesting.html
	/// </summary>
	[PublicAPI]
	public static class AGPermissions
	{
		const int PERMISSION_GRANTED = 0;
		const int PERMISSION_DENIED = -1;

		/// <summary>
		/// Permission request result.
		/// </summary>
		[PublicAPI]
		public class PermissionRequestResult
		{
			PermissionRequestResult()
			{
			}

			public static PermissionRequestResult FromJson(Dictionary<string, object> serialized)
			{
				var result = new PermissionRequestResult
				{
					Permission = serialized.GetStr("permission"),
					ShouldShowRequestPermissionRationale = (bool) serialized["shouldShowRequestPermissionRationale"],
					Status = (PermissionStatus) (int) (long) serialized["result"]
				};
				return result;
			}

			/// <summary>
			/// Gets the requested permission.
			/// </summary>
			/// <value>The requested permission.</value>
			[PublicAPI]
			public string Permission { get; private set; }

			/// <summary>
			/// Gets the requested permission status.
			/// </summary>
			/// <value>The status of requested permssion.</value>
			[PublicAPI]
			public PermissionStatus Status { get; private set; }

			/// <summary>
			/// Gets whether you should show UI with rationale for requesting a permission.
			/// 
			/// </summary>
			/// <value><c>true</c> if should show explanation why permission is needed; otherwise, <c>false</c>.</value>
			[PublicAPI]
			public bool ShouldShowRequestPermissionRationale { get; private set; }

			public override string ToString()
			{
				return string.Format(
					"[PermissionRequestResult: Permission={0}, Status={1}, ShouldShowRequestPermissionRationale={2}]",
					Permission, Status, ShouldShowRequestPermissionRationale);
			}
		}

		/// <summary>
		/// Permission status
		/// </summary>
		[PublicAPI]
		public enum PermissionStatus
		{
			/// <summary>
			/// The permission has been granted.
			/// </summary>
			[PublicAPI]
			Granted = PERMISSION_GRANTED,

			/// <summary>
			/// The permission has not been granted.
			/// </summary>
			[PublicAPI]
			Denied = PERMISSION_DENIED
		}

		/// <summary>
		///     Checks whether the specified permission was granted by the user.
		/// </summary>
		/// <param name="permission">Permission to check</param>
		/// <returns>Whether the specified permission was granted by the user.</returns>
		[PublicAPI]
		public static bool IsPermissionGranted(string permission)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			if (string.IsNullOrEmpty(permission))
			{
				throw new ArgumentException("Permission must not be null or empty", "permission");
			}

			try
			{
				using (var c = new AndroidJavaClass(C.AndroidSupportV4ContentContextCompat))
				{
					return c.CallStaticInt("checkSelfPermission", AGUtils.Activity, permission) == PERMISSION_GRANTED;
				}
			}
			catch (Exception ex)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning(
						"Could not check if runtime permission is granted. Check if Android version is 6.0 (API level 23) or higher. " +
						ex.Message);
				}

				return false;
			}
		}

		static Action<PermissionRequestResult[]> _onRequestPermissionsResults;

		/// <summary>
		/// Requests the required runtime permissions for your application.
		/// </summary>
		/// <param name="permissions">Permissions to request.</param>
		/// <param name="onRequestPermissionsResults">Callback with results.</param>
		[PublicAPI]
		public static void RequestPermissions(string[] permissions,
			Action<PermissionRequestResult[]> onRequestPermissionsResults)
		{
			if (permissions == null || permissions.Length == 0)
			{
				throw new ArgumentException("Requested permissions array must not be null or empty", "permissions");
			}

			if (onRequestPermissionsResults == null)
			{
				throw new ArgumentNullException("onRequestPermissionsResults", "Listener cannot be null");
			}

			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (IsPriorToMarshmellow())
			{
				return;
			}

			try
			{
				_onRequestPermissionsResults = onRequestPermissionsResults;
				AGActivityUtils.RequestPermissions(permissions);
			}
			catch (Exception ex)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("Could not request runtime permissions. " + ex.Message);
				}
			}
		}

		static bool IsPriorToMarshmellow()
		{
			bool result = AGDeviceInfo.SDK_INT < AGDeviceInfo.VersionCodes.M;
			if (result)
			{
				Debug.Log(
					"Runtime permissions on Android below version 23 are not required and this method will do nothing");
			}

			return result;
		}

		#region permissions
		
		[PublicAPI]
		public const string ACCEPT_HANDOVER = "android.permission.ACCEPT_HANDOVER";

		[PublicAPI]
		public const string ACCESS_CHECKIN_PROPERTIES = "android.permission.ACCESS_CHECKIN_PROPERTIES";

		[PublicAPI]
		public const string ACCESS_COARSE_LOCATION = "android.permission.ACCESS_COARSE_LOCATION";

		[PublicAPI]
		public const string ACCESS_FINE_LOCATION = "android.permission.ACCESS_FINE_LOCATION";

		[PublicAPI]
		public const string ACCESS_LOCATION_EXTRA_COMMANDS = "android.permission.ACCESS_LOCATION_EXTRA_COMMANDS";

		[PublicAPI]
		public const string ACCESS_NETWORK_STATE = "android.permission.ACCESS_NETWORK_STATE";

		[PublicAPI]
		public const string ACCESS_NOTIFICATION_POLICY = "android.permission.ACCESS_NOTIFICATION_POLICY";

		[PublicAPI]
		public const string ACCESS_WIFI_STATE = "android.permission.ACCESS_WIFI_STATE";

		[PublicAPI]
		public const string ACCOUNT_MANAGER = "android.permission.ACCOUNT_MANAGER";

		[PublicAPI]
		public const string ADD_VOICEMAIL = "com.android.voicemail.permission.ADD_VOICEMAIL";
		
		[PublicAPI]
		public const string ANSWER_PHONE_CALLS = "android.permission.ANSWER_PHONE_CALLS";

		[PublicAPI]
		public const string BATTERY_STATS = "android.permission.BATTERY_STATS";

		[PublicAPI]
		public const string BIND_ACCESSIBILITY_SERVICE = "android.permission.BIND_ACCESSIBILITY_SERVICE";

		[PublicAPI]
		public const string BIND_APPWIDGET = "android.permission.BIND_APPWIDGET";
		
		[PublicAPI]
		public const string BIND_AUTOFILL_SERVICE = "android.permission.BIND_AUTOFILL_SERVICE";

		[Obsolete]
		[PublicAPI]
		public const string BIND_CARRIER_MESSAGING_SERVICE = "android.permission.BIND_CARRIER_MESSAGING_SERVICE";

		[PublicAPI]
		public const string BIND_CARRIER_SERVICES = "android.permission.BIND_CARRIER_SERVICES";

		[PublicAPI]
		public const string BIND_CHOOSER_TARGET_SERVICE = "android.permission.BIND_CHOOSER_TARGET_SERVICE";
		
		[PublicAPI]
		public const string BIND_CONDITION_PROVIDER_SERVICE = "android.permission.BIND_CONDITION_PROVIDER_SERVICE";

		[PublicAPI]
		public const string BIND_DEVICE_ADMIN = "android.permission.BIND_DEVICE_ADMIN";

		[PublicAPI]
		public const string BIND_DREAM_SERVICE = "android.permission.BIND_DREAM_SERVICE";

		[PublicAPI]
		public const string BIND_INCALL_SERVICE = "android.permission.BIND_INCALL_SERVICE";

		[PublicAPI]
		public const string BIND_INPUT_METHOD = "android.permission.BIND_INPUT_METHOD";

		[PublicAPI]
		public const string BIND_MIDI_DEVICE_SERVICE = "android.permission.BIND_MIDI_DEVICE_SERVICE";

		[PublicAPI]
		public const string BIND_NFC_SERVICE = "android.permission.BIND_NFC_SERVICE";

		[PublicAPI]
		public const string BIND_NOTIFICATION_LISTENER_SERVICE =
			"android.permission.BIND_NOTIFICATION_LISTENER_SERVICE";

		[PublicAPI]
		public const string BIND_PRINT_SERVICE = "android.permission.BIND_PRINT_SERVICE";
		
		[PublicAPI]
		public const string BIND_QUICK_SETTINGS_TILE = "android.permission.BIND_QUICK_SETTINGS_TILE";

		[PublicAPI]
		public const string BIND_REMOTEVIEWS = "android.permission.BIND_REMOTEVIEWS";
		
		[PublicAPI]
		public const string BIND_SCREENING_SERVICE = "android.permission.BIND_SCREENING_SERVICE";

		[PublicAPI]
		public const string BIND_TELECOM_CONNECTION_SERVICE =
			"android.permission.BIND_TELECOM_CONNECTION_SERVICE";

		[PublicAPI]
		public const string BIND_TEXT_SERVICE = "android.permission.BIND_TEXT_SERVICE";

		[PublicAPI]
		public const string BIND_TV_INPUT = "android.permission.BIND_TV_INPUT";
		
		[PublicAPI]
		public const string BIND_VISUAL_VOICEMAIL_SERVICE = "android.permission.BIND_VISUAL_VOICEMAIL_SERVICE";

		[PublicAPI]
		public const string BIND_VOICE_INTERACTION = "android.permission.BIND_VOICE_INTERACTION";

		[PublicAPI]
		public const string BIND_VPN_SERVICE = "android.permission.BIND_VPN_SERVICE";
		
		[PublicAPI]
		public const string BIND_VR_LISTENER_SERVICE = "android.permission.BIND_VR_LISTENER_SERVICE";

		[PublicAPI]
		public const string BIND_WALLPAPER = "android.permission.BIND_WALLPAPER";

		[PublicAPI]
		public const string BLUETOOTH = "android.permission.BLUETOOTH";

		[PublicAPI]
		public const string BLUETOOTH_ADMIN = "android.permission.BLUETOOTH_ADMIN";

		[PublicAPI]
		public const string BLUETOOTH_PRIVILEGED = "android.permission.BLUETOOTH_PRIVILEGED";

		[PublicAPI]
		public const string BODY_SENSORS = "android.permission.BODY_SENSORS";

		[PublicAPI]
		public const string BROADCAST_PACKAGE_REMOVED = "android.permission.BROADCAST_PACKAGE_REMOVED";

		[PublicAPI]
		public const string BROADCAST_SMS = "android.permission.BROADCAST_SMS";

		[PublicAPI]
		public const string BROADCAST_STICKY = "android.permission.BROADCAST_STICKY";

		[PublicAPI]
		public const string BROADCAST_WAP_PUSH = "android.permission.BROADCAST_WAP_PUSH";

		[PublicAPI]
		public const string CALL_PHONE = "android.permission.CALL_PHONE";

		[PublicAPI]
		public const string CALL_PRIVILEGED = "android.permission.CALL_PRIVILEGED";

		[PublicAPI]
		public const string CAMERA = "android.permission.CAMERA";

		[PublicAPI]
		public const string CAPTURE_AUDIO_OUTPUT = "android.permission.CAPTURE_AUDIO_OUTPUT";

		[PublicAPI]
		public const string CAPTURE_SECURE_VIDEO_OUTPUT = "android.permission.CAPTURE_SECURE_VIDEO_OUTPUT";

		[PublicAPI]
		public const string CAPTURE_VIDEO_OUTPUT = "android.permission.CAPTURE_VIDEO_OUTPUT";

		[PublicAPI]
		public const string CHANGE_COMPONENT_ENABLED_STATE =
			"android.permission.CHANGE_COMPONENT_ENABLED_STATE";

		[PublicAPI]
		public const string CHANGE_CONFIGURATION = "android.permission.CHANGE_CONFIGURATION";

		[PublicAPI]
		public const string CHANGE_NETWORK_STATE = "android.permission.CHANGE_NETWORK_STATE";

		[PublicAPI]
		public const string CHANGE_WIFI_MULTICAST_STATE = "android.permission.CHANGE_WIFI_MULTICAST_STATE";

		[PublicAPI]
		public const string CHANGE_WIFI_STATE = "android.permission.CHANGE_WIFI_STATE";

		[PublicAPI]
		public const string CLEAR_APP_CACHE = "android.permission.CLEAR_APP_CACHE";

		[PublicAPI]
		public const string CONTROL_LOCATION_UPDATES = "android.permission.CONTROL_LOCATION_UPDATES";

		[PublicAPI]
		public const string DELETE_CACHE_FILES = "android.permission.DELETE_CACHE_FILES";

		[PublicAPI]
		public const string DELETE_PACKAGES = "android.permission.DELETE_PACKAGES";

		[PublicAPI]
		public const string DIAGNOSTIC = "android.permission.DIAGNOSTIC";

		[PublicAPI]
		public const string DISABLE_KEYGUARD = "android.permission.DISABLE_KEYGUARD";

		[PublicAPI]
		public const string DUMP = "android.permission.DUMP";

		[PublicAPI]
		public const string EXPAND_STATUS_BAR = "android.permission.EXPAND_STATUS_BAR";

		[PublicAPI]
		public const string FACTORY_TEST = "android.permission.FACTORY_TEST";
		
		[PublicAPI]
		public const string FOREGROUND_SERVICE = "android.permission.FOREGROUND_SERVICE";

		//Is not listed in documentation (15.08.2018)
		[PublicAPI]
		public const string FLASHLIGHT = "android.permission.FLASHLIGHT";

		[PublicAPI]
		public const string GET_ACCOUNTS = "android.permission.GET_ACCOUNTS";

		[PublicAPI]
		public const string GET_ACCOUNTS_PRIVILEGED = "android.permission.GET_ACCOUNTS_PRIVILEGED";

		[PublicAPI]
		public const string GET_PACKAGE_SIZE = "android.permission.GET_PACKAGE_SIZE";

		[Obsolete]
		[PublicAPI]
		public const string GET_TASKS = "android.permission.GET_TASKS";

		[PublicAPI]
		public const string GLOBAL_SEARCH = "android.permission.GLOBAL_SEARCH";

		[PublicAPI]
		public const string INSTALL_LOCATION_PROVIDER = "android.permission.INSTALL_LOCATION_PROVIDER";

		[PublicAPI]
		public const string INSTALL_PACKAGES = "android.permission.INSTALL_PACKAGES";

		[PublicAPI]
		public const string INSTALL_SHORTCUT = "com.android.launcher.permission.INSTALL_SHORTCUT";
		
		[PublicAPI]
		public const string INSTANT_APP_FOREGROUND_SERVICE 
			= "com.android.launcher.permission.INSTANT_APP_FOREGROUND_SERVICE";

		[PublicAPI]
		public const string INTERNET = "android.permission.INTERNET";

		[PublicAPI]
		public const string KILL_BACKGROUND_PROCESSES = "android.permission.KILL_BACKGROUND_PROCESSES";

		[PublicAPI]
		public const string LOCATION_HARDWARE = "android.permission.LOCATION_HARDWARE";

		[PublicAPI]
		public const string MANAGE_DOCUMENTS = "android.permission.MANAGE_DOCUMENTS";
		
		[PublicAPI]
		public const string MANAGE_OWN_CALLS = "android.permission.MANAGE_OWN_CALLS";

		[PublicAPI]
		public const string MASTER_CLEAR = "android.permission.MASTER_CLEAR";

		[PublicAPI]
		public const string MEDIA_CONTENT_CONTROL = "android.permission.MEDIA_CONTENT_CONTROL";

		[PublicAPI]
		public const string MODIFY_AUDIO_SETTINGS = "android.permission.MODIFY_AUDIO_SETTINGS";

		[PublicAPI]
		public const string MODIFY_PHONE_STATE = "android.permission.MODIFY_PHONE_STATE";

		[PublicAPI]
		public const string MOUNT_FORMAT_FILESYSTEMS = "android.permission.MOUNT_FORMAT_FILESYSTEMS";

		[PublicAPI]
		public const string MOUNT_UNMOUNT_FILESYSTEMS = "android.permission.MOUNT_UNMOUNT_FILESYSTEMS";

		[PublicAPI]
		public const string NFC = "android.permission.NFC";
		
		[PublicAPI]
		public const string NFC_TRANSACTION_EVENT = "android.permission.NFC_TRANSACTION_EVENT";

		[PublicAPI]
		public const string PACKAGE_USAGE_STATS = "android.permission.PACKAGE_USAGE_STATS";

		[Obsolete]
		[PublicAPI]
		public const string PERSISTENT_ACTIVITY = "android.permission.PERSISTENT_ACTIVITY";

		[PublicAPI]
		public const string PROCESS_OUTGOING_CALLS = "android.permission.PROCESS_OUTGOING_CALLS";

		[PublicAPI]
		public const string READ_CALENDAR = "android.permission.READ_CALENDAR";

		[PublicAPI]
		public const string READ_CALL_LOG = "android.permission.READ_CALL_LOG";

		[PublicAPI]
		public const string READ_CONTACTS = "android.permission.READ_CONTACTS";

		[PublicAPI]
		public const string READ_EXTERNAL_STORAGE = "android.permission.READ_EXTERNAL_STORAGE";

		[PublicAPI]
		public const string READ_FRAME_BUFFER = "android.permission.READ_FRAME_BUFFER";

		[Obsolete]
		[PublicAPI]
		public const string READ_INPUT_STATE = "android.permission.READ_INPUT_STATE";

		[PublicAPI]
		public const string READ_LOGS = "android.permission.READ_LOGS";
		
		[PublicAPI]
		public const string READ_PHONE_NUMBERS = "android.permission.READ_PHONE_NUMBERS";

		[PublicAPI]
		public const string READ_PHONE_STATE = "android.permission.READ_PHONE_STATE";

		[PublicAPI]
		public const string READ_SMS = "android.permission.READ_SMS";

		[PublicAPI]
		public const string READ_SYNC_SETTINGS = "android.permission.READ_SYNC_SETTINGS";

		[PublicAPI]
		public const string READ_SYNC_STATS = "android.permission.READ_SYNC_STATS";

		[PublicAPI]
		public const string READ_VOICEMAIL = "com.android.voicemail.permission.READ_VOICEMAIL";

		[PublicAPI]
		public const string REBOOT = "android.permission.REBOOT";

		[PublicAPI]
		public const string RECEIVE_BOOT_COMPLETED = "android.permission.RECEIVE_BOOT_COMPLETED";

		[PublicAPI]
		public const string RECEIVE_MMS = "android.permission.RECEIVE_MMS";

		[PublicAPI]
		public const string RECEIVE_SMS = "android.permission.RECEIVE_SMS";

		[PublicAPI]
		public const string RECEIVE_WAP_PUSH = "android.permission.RECEIVE_WAP_PUSH";

		[PublicAPI]
		public const string RECORD_AUDIO = "android.permission.RECORD_AUDIO";

		[PublicAPI]
		public const string REORDER_TASKS = "android.permission.REORDER_TASKS";
		
		[PublicAPI]
		public const string REQUEST_COMPANION_RUN_IN_BACKGROUND
			= "android.permission.REQUEST_COMPANION_RUN_IN_BACKGROUND";
		
		[PublicAPI]
		public const string REQUEST_COMPANION_USE_DATA_IN_BACKGROUND
			= "android.permission.REQUEST_COMPANION_USE_DATA_IN_BACKGROUND";
		
		[PublicAPI]
		public const string REQUEST_DELETE_PACKAGES = "android.permission.REQUEST_DELETE_PACKAGES";

		[PublicAPI]
		public const string REQUEST_IGNORE_BATTERY_OPTIMIZATIONS = "android.permission.REQUEST_IGNORE_BATTERY_OPTIMIZATIONS";

		[PublicAPI]
		public const string REQUEST_INSTALL_PACKAGES = "android.permission.REQUEST_INSTALL_PACKAGES";

		[Obsolete]
		[PublicAPI]
		public const string RESTART_PACKAGES = "android.permission.RESTART_PACKAGES";

		[PublicAPI]
		public const string SEND_RESPOND_VIA_MESSAGE = "android.permission.SEND_RESPOND_VIA_MESSAGE";

		[PublicAPI]
		public const string SEND_SMS = "android.permission.SEND_SMS";

		[PublicAPI]
		public const string SET_ALARM = "com.android.alarm.permission.SET_ALARM";

		[PublicAPI]
		public const string SET_ALWAYS_FINISH = "android.permission.SET_ALWAYS_FINISH";

		[PublicAPI]
		public const string SET_ANIMATION_SCALE = "android.permission.SET_ANIMATION_SCALE";

		[PublicAPI]
		public const string SET_DEBUG_APP = "android.permission.SET_DEBUG_APP";

		[Obsolete]
		[PublicAPI]
		public const string SET_PREFERRED_APPLICATIONS = "android.permission.SET_PREFERRED_APPLICATIONS";

		[PublicAPI]
		public const string SET_PROCESS_LIMIT = "android.permission.SET_PROCESS_LIMIT";

		[PublicAPI]
		public const string SET_TIME = "android.permission.SET_TIME";

		[PublicAPI]
		public const string SET_TIME_ZONE = "android.permission.SET_TIME_ZONE";

		[PublicAPI]
		public const string SET_WALLPAPER = "android.permission.SET_WALLPAPER";

		[PublicAPI]
		public const string SET_WALLPAPER_HINTS = "android.permission.SET_WALLPAPER_HINTS";

		[PublicAPI]
		public const string SIGNAL_PERSISTENT_PROCESSES = "android.permission.SIGNAL_PERSISTENT_PROCESSES";

		[PublicAPI]
		public const string STATUS_BAR = "android.permission.STATUS_BAR";

		[PublicAPI]
		public const string SYSTEM_ALERT_WINDOW = "android.permission.SYSTEM_ALERT_WINDOW";

		[PublicAPI]
		public const string TRANSMIT_IR = "android.permission.TRANSMIT_IR";

		/// <summary>
		/// Permission is no longer supported. Do not use!
		/// </summary>
		[Obsolete]
		[PublicAPI]
		public const string UNINSTALL_SHORTCUT = "com.android.launcher.permission.UNINSTALL_SHORTCUT";

		[PublicAPI]
		public const string UPDATE_DEVICE_STATS = "android.permission.UPDATE_DEVICE_STATS";
		
		[PublicAPI]
		public const string USE_BIOMETRIC = "android.permission.USE_BIOMETRIC";

		/// <summary>
		/// This constant was deprecated in API level 28. Applications should request USE_BIOMETRIC instead.
		/// </summary>
		[Obsolete]
		[PublicAPI]
		public const string USE_FINGERPRINT = "android.permission.USE_FINGERPRINT";

		[PublicAPI]
		public const string USE_SIP = "android.permission.USE_SIP";

		[PublicAPI]
		public const string VIBRATE = "android.permission.VIBRATE";

		[PublicAPI]
		public const string WAKE_LOCK = "android.permission.WAKE_LOCK";

		[PublicAPI]
		public const string WRITE_APN_SETTINGS = "android.permission.WRITE_APN_SETTINGS";

		[PublicAPI]
		public const string WRITE_CALENDAR = "android.permission.WRITE_CALENDAR";

		[PublicAPI]
		public const string WRITE_CALL_LOG = "android.permission.WRITE_CALL_LOG";

		[PublicAPI]
		public const string WRITE_CONTACTS = "android.permission.WRITE_CONTACTS";

		[PublicAPI]
		public const string WRITE_EXTERNAL_STORAGE = "android.permission.WRITE_EXTERNAL_STORAGE";

		[PublicAPI]
		public const string WRITE_GSERVICES = "android.permission.WRITE_GSERVICES";

		[PublicAPI]
		public const string WRITE_SECURE_SETTINGS = "android.permission.WRITE_SECURE_SETTINGS";

		[PublicAPI]
		public const string WRITE_SETTINGS = "android.permission.WRITE_SETTINGS";

		[PublicAPI]
		public const string WRITE_SYNC_SETTINGS = "android.permission.WRITE_SYNC_SETTINGS";

		[PublicAPI]
		public const string WRITE_VOICEMAIL = "com.android.voicemail.permission.WRITE_VOICEMAIL";

		#endregion

		public static void OnRequestPermissionsResult(string message)
		{
			if (_onRequestPermissionsResults != null)
			{
				var dic = Json.Deserialize(message) as Dictionary<string, object>;
				var resultsArr = dic["result"] as List<object>;

				var permissionResults = new PermissionRequestResult[resultsArr.Count];

				for (int i = 0; i < resultsArr.Count; i++)
				{
					var parsed = PermissionRequestResult.FromJson(resultsArr[i] as Dictionary<string, object>);
					permissionResults[i] = parsed;
				}

				_onRequestPermissionsResults(permissionResults);
				_onRequestPermissionsResults = null;
			}
		}
	}
}

#endif
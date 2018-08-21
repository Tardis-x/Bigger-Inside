// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGEnvironment.cs
//


#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using UnityEngine;

	/// <summary>
	/// Provides access to Android environment variables.
	/// <see href="https://developer.android.com/reference/android/os/Environment.html">Android Environment Docs</see>
	/// </summary>
	public static class AGEnvironment
	{
		#region constants

		/// <summary>
		/// Storage state if the media was removed before it was unmounted.
		/// </summary>
		public const string MEDIA_BAD_REMOVAL = "bad_removal";

		/// <summary>
		/// Storage state if the media is present and being disk-checked.
		/// </summary>
		public const string MEDIA_CHECKING = "checking";

		/// <summary>
		/// Storage state if the media is in the process of being ejected.
		/// </summary>
		public const string MEDIA_MOUNTED = "mounted";

		/// <summary>
		/// Storage state if the media is present and mounted at its mount point with read-only access.
		/// </summary>
		public const string MEDIA_MOUNTED_READ_ONLY = "mounted_ro";

		/// <summary>
		/// Storage state if the media is present but is blank or is using an unsupported filesystem.
		/// </summary>
		public const string MEDIA_NOFS = "nofs";

		/// <summary>
		/// Storage state if the media is not present.
		/// </summary>
		public const string MEDIA_REMOVED = "removed";

		/// <summary>
		/// Storage state if the media is present not mounted, and shared via USB mass storage.
		/// </summary>
		public const string MEDIA_SHARED = "shared";

		/// <summary>
		/// Unknown storage state, such as when a path isn't backed by known storage media.
		/// </summary>
		public const string MEDIA_UNKNOWN = "unknown";

		/// <summary>
		/// Storage state if the media is present but cannot be mounted. Typically this happens if the file system on the media is corrupted.
		/// </summary>
		public const string MEDIA_UNMOUNTABLE = "unmountable";

		/// <summary>
		/// Storage state if the media is present but not mounted.
		/// </summary>
		public const string MEDIA_UNMOUNTED = "unmounted";

		#endregion

		#region fields

		/// <summary>
		/// Standard directory in which to place any audio files that should be in the list of alarms that the user can select (not as regular music).
		/// </summary>
		/// <value>The alarms directory.</value>
		public static string DirectoryAlarms
		{
			get { return GetStringProperty("DIRECTORY_ALARMS"); }
		}

		/// <summary>
		/// The traditional location for pictures and videos when mounting the device as a camera.
		/// </summary>
		/// <value>The DCIM directory.</value>
		public static string DirectoryDCIM
		{
			get { return GetStringProperty("DIRECTORY_DCIM"); }
		}

		/// <summary>
		/// Standard directory in which to place documents that have been created by the user.
		/// </summary>
		/// <value>The documents directory.</value>
		public static string DirectoryDocuments
		{
			get { return GetStringProperty("DIRECTORY_DOCUMENTS"); }
		}

		/// <summary>
		/// Standard directory in which to place files that have been downloaded by the user. 
		/// </summary>
		/// <value>The downloads directory.</value>
		public static string DirectoryDownloads
		{
			get { return GetStringProperty("DIRECTORY_DOWNLOADS"); }
		}

		/// <summary>
		/// Standard directory in which to place movies that are available to the user.
		/// </summary>
		/// <value>The movies directory.</value>
		public static string DirectoryMovies
		{
			get { return GetStringProperty("DIRECTORY_MOVIES"); }
		}

		/// <summary>
		/// Standard directory in which to place any audio files that should be in the regular list of music for the user.
		/// </summary>
		/// <value>The music directory.</value>
		public static string DirectoryMusic
		{
			get { return GetStringProperty("DIRECTORY_MUSIC"); }
		}

		/// <summary>
		/// Standard directory in which to place any audio files that should be in the list of notifications that the user can select (not as regular music).
		/// </summary>
		/// <value>The notifications directory.</value>
		public static string DirectoryNotifications
		{
			get { return GetStringProperty("DIRECTORY_NOTIFICATIONS"); }
		}

		/// <summary>
		/// Standard directory in which to place pictures that are available to the user.
		/// </summary>
		/// <value>The pictures directory.</value>
		public static string DirectoryPictures
		{
			get { return GetStringProperty("DIRECTORY_PICTURES"); }
		}

		/// <summary>
		/// Standard directory in which to place any audio files that should be in the list of podcasts that the user can select (not as regular music). 
		/// </summary>
		/// <value>The podcasts directory.</value>
		public static string DirectoryPodcasts
		{
			get { return GetStringProperty("DIRECTORY_PODCASTS"); }
		}

		/// <summary>
		/// Standard directory in which to place any audio files that should be in the list of ringtones that the user can select (not as regular music).
		/// </summary>
		/// <value>The ringtones directory.</value>
		public static string DirectoryRingtones
		{
			get { return GetStringProperty("DIRECTORY_RINGTONES"); }
		}

		#endregion

		#region methods

		/// <summary>
		/// Return the user data directory.
		/// </summary>
		public static string DataDirectoryPath
		{
			get { return GetFileDirectory("getDataDirectory"); }
		}

		/// <summary>
		/// Return the download/cache content directory.
		/// </summary>
		public static string DownloadCacheDirectoryPath
		{
			get { return GetFileDirectory("getDownloadCacheDirectory"); }
		}

		/// <summary>
		/// Return the primary shared/external storage directory path.
		/// </summary>
		/// <value>The external storage directory path.</value>
		public static string ExternalStorageDirectoryPath
		{
			get { return GetFileDirectory("getExternalStorageDirectory"); }
		}

		/// <summary>
		/// Return root of the "system" partition holding the core Android OS.
		/// </summary>
		/// <value>The root directory path.</value>
		public static string RootDirectoryPath
		{
			get { return GetFileDirectory("getRootDirectory"); }
		}

		/// <summary>
		/// Returns the current state of the shared/external storage media at the given path.
		/// </summary>
		/// <value>The state of the external storage.</value>
		public static string ExternalStorageState
		{
			get { return EnvClassCallStatic<string>("getExternalStorageState"); }
		}

		/// <summary>
		/// Returns whether the primary shared/external storage media is emulated.
		/// </summary>
		/// <value><c>true</c> if external storage is emulated; otherwise, <c>false</c>.</value>
		public static bool IsExternalStorageEmulated
		{
			get { return EnvClassCallStatic<bool>("isExternalStorageEmulated"); }
		}

		/// <summary>
		/// Returns whether the primary shared/external storage media is physically removable.
		/// </summary>
		/// <value><c>true</c> if external storage is removable; otherwise, <c>false</c>.</value>
		public static bool IsExternalStorageRemovable
		{
			get { return EnvClassCallStatic<bool>("isExternalStorageRemovable"); }
		}

		/// <summary>
		/// Get a top-level shared/external storage directory for placing files of a particular type.
		/// </summary>
		/// <returns>The external storage public directory path.</returns>
		/// <param name="type">Type.</param>
		public static string GetExternalStoragePublicDirectoryPath(string type)
		{
			return GetFileDirectory("getExternalStoragePublicDirectory", type);
		}

		#endregion

		#region helpers

		static string GetFileDirectory(string methodName, params object[] args)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}

			try
			{
				using (var env = new AndroidJavaClass(C.AndroidOsEnvironment))
				{
					return env.CallStaticAJO(methodName, args).GetAbsolutePath();
				}
			}
			catch (Exception e)
			{
				Debug.LogWarning(string.Format("Failed to get directory {0} on {1}, Error: {2}", methodName, C.AndroidOsEnvironment, e.Message));
				return null;
			}
		}

		static T EnvClassCallStatic<T>(string methodName, params object[] args)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return default(T);
			}

			try
			{
				return C.AndroidOsEnvironment.AJCCallStaticOnce<T>(methodName, args);
			}
			catch (Exception e)
			{
				Debug.LogWarning(string.Format("Failed to invoke {0} on {1}, Error: {2}", methodName, C.AndroidOsEnvironment, e.Message));
				return default(T);
			}
		}

		static string GetStringProperty(string propertyName)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}

			try
			{
				using (var env = new AndroidJavaClass(C.AndroidOsEnvironment))
				{
					return env.GetStaticStr(propertyName);
				}
			}
			catch (Exception e)
			{
				Debug.LogWarning("Could not get the property: " + propertyName +
				                 ". Check the device API level if the property is present, reason: " + e.Message);
				return string.Empty;
			}
		}

		#endregion
	}
}
#endif
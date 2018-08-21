// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGSharedPrefs.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.Collections.Generic;
	using Internal;
	using UnityEngine;

	/// <summary>
	/// Class to interact with Android SharedPreferences:
	/// https://developer.android.com/reference/android/content/SharedPreferences.html
	/// Unlike the Android API all operations are done instantly commiting in the end
	/// </summary>
	public static class AGSharedPrefs
	{
		public const int MODE_PRIVATE = 0;
		public const int MODE_WORLD_READABLE = 1;
		public const int MODE_WORLD_WRITEABLE = 2;

		#region put_API

		/// <summary>
		/// Set a boolean value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="value">The new value for the preference.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static bool SetBool(string preferenceFileKey, string key, bool value, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}

			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			var editor = PutValue(preferenceFileKey, key, value, mode);
			return Commit(editor);
		}

		/// <summary>
		/// Set a float value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="value">The new value for the preference.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static bool SetFloat(string preferenceFileKey, string key, float value, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}


			var editor = PutValue(preferenceFileKey, key, value, mode);
			return Commit(editor);
		}

		/// <summary>
		/// Set a int value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="value">The new value for the preference.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static bool SetInt(string preferenceFileKey, string key, int value, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}


			try
			{
				var editor = PutValue(preferenceFileKey, key, value, mode);
				return Commit(editor);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				return false;
			}
		}

		/// <summary>
		/// Set a long value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="value">The new value for the preference.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static bool SetLong(string preferenceFileKey, string key, long value, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}

			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			var editor = PutValue(preferenceFileKey, key, value, mode);
			return Commit(editor);
		}

		/// <summary>
		/// Set a string value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="value">The new value for the preference.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static bool SetString(string preferenceFileKey, string key, string value, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}


			var editor = PutValue(preferenceFileKey, key, value, mode);
			return Commit(editor);
		}

		/// <summary>
		/// Removes value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to remove.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static bool Remove(string preferenceFileKey, string key, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}

			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			using (var editor = GetEditorInternal(preferenceFileKey, mode))
			{
				editor.CallAJO("remove", key);
				return Commit(editor);
			}
		}

		/// <summary>
		///  Remove all values from the preferences.
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static bool Clear(string preferenceFileKey, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}

			using (var editor = GetEditorInternal(preferenceFileKey, mode))
			{
				editor.CallAJO("clear");
				return Commit(editor);
			}
		}

		#endregion

		#region get_API

		/// <summary>
		/// Get all preferences
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Dictionary with all the preferences</returns>
		public static Dictionary<string, object> GetAll(string preferenceFileKey, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return new Dictionary<string, object>();
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}


			try
			{
				using (var prefs = GetSharedPrefs(preferenceFileKey, mode))
				{
					var allPrefsMapAjo = prefs.CallAJO("getAll");
					return allPrefsMapAjo.FromJavaMap();
				}
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return null;
			}
		}

		/// <summary>
		/// Get a boolean value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="defaultValue">Value to return if this preference does not exist.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static bool GetBool(string preferenceFileKey, string key, bool defaultValue, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}


			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		/// <summary>
		/// Get a float value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="defaultValue">Value to return if this preference does not exist.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static float GetFloat(string preferenceFileKey, string key, float defaultValue, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}


			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		/// <summary>
		/// Get an int value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="defaultValue">Value to return if this preference does not exist.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static int GetInt(string preferenceFileKey, string key, int defaultValue, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}

			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		/// <summary>
		/// Get a long value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="defaultValue">Value to return if this preference does not exist.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static long GetLong(string preferenceFileKey, string key, long defaultValue, int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}


			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		/// <summary>
		/// Get a string value
		/// </summary>
		/// <param name="preferenceFileKey">Desired preferences file. If a preferences file by this name does not exist, it will be created.</param>
		/// <param name="key">The name of the preference to modify.</param>
		/// <param name="defaultValue">Value to return if this preference does not exist.</param>
		/// <param name="mode">Operating mode. Use 0 or MODE_PRIVATE for the default operation.</param>
		/// <returns>Returns true if the new values were successfully written to persistent storage.</returns>
		public static string GetString(string preferenceFileKey, string key, string defaultValue,
			int mode = MODE_PRIVATE)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(preferenceFileKey))
			{
				throw new ArgumentException("preferenceFileKey");
			}

			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("key");
			}
			return GetValue(preferenceFileKey, key, defaultValue, mode);
		}

		#endregion

		static AndroidJavaObject GetSharedPrefs(string preferenceFileKey, int mode = MODE_PRIVATE)
		{
			return AGUtils.Activity.CallAJO("getSharedPreferences", preferenceFileKey, mode);
		}

		static AndroidJavaObject GetPrefs(int mode = MODE_PRIVATE)
		{
			return AGUtils.Activity.CallAJO("getPreferences", mode);
		}

		static AndroidJavaObject GetEditor(AndroidJavaObject prefs)
		{
			return prefs.CallAJO("edit");
		}

		static AndroidJavaObject GetEditorInternal(string perferenceFileKey, int mode = MODE_PRIVATE)
		{
			return GetEditor(GetSharedPrefs(perferenceFileKey, mode));
		}

		static bool Commit(AndroidJavaObject editor)
		{
			return editor.CallBool("commit");
		}

		static AndroidJavaObject PutValue<T>(string preferenceFileKey, string key, T value, int mode = MODE_PRIVATE)
		{
			var editor = GetEditorInternal(preferenceFileKey, mode);
			if (typeof(T) == typeof(bool))
			{
				editor.CallAJO("putBoolean", key, value);
			}
			else if (typeof(T) == typeof(float))
			{
				editor.CallAJO("putFloat", key, value);
			}
			else if (typeof(T) == typeof(int))
			{
				editor.CallAJO("putInt", key, value);
			}
			else if (typeof(T) == typeof(long))
			{
				editor.CallAJO("putLong", key, value);
			}
			else if (typeof(T) == typeof(string))
			{
				editor.CallAJO("putString", key, value);
			}
			else
			{
				Debug.LogError(typeof(T) + " is not supported");
			}
			return editor;
		}

		static T GetValue<T>(string preferenceFileKey, string key, T defaultValue, int mode = MODE_PRIVATE)
		{
			try
			{
				using (var prefs = GetSharedPrefs(preferenceFileKey, mode))
				{
					T result = defaultValue;

					if (typeof(T) == typeof(bool))
					{
						result = prefs.Call<T>("getBoolean", key, defaultValue);
					}
					else if (typeof(T) == typeof(float))
					{
						result = prefs.Call<T>("getFloat", key, defaultValue);
					}
					else if (typeof(T) == typeof(int))
					{
						result = prefs.Call<T>("getInt", key, defaultValue);
					}
					else if (typeof(T) == typeof(long))
					{
						result = prefs.Call<T>("getLong", key, defaultValue);
					}
					else if (typeof(T) == typeof(string))
					{
						result = prefs.Call<T>("getString", key, defaultValue);
					}
					else
					{
						Debug.LogError(typeof(T) + " is not supported");
					}

					return result;
				}
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return defaultValue;
			}
		}
	}
}

#endif
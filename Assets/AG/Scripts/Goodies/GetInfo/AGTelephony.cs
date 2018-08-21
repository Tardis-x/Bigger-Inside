// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGTelephony.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using UnityEngine;

	/// <summary>
	/// Telephony.
	///
	/// Required permissions:
	///     <uses-permission android:name="android.permission.READ_PHONE_STATE" />
	/// </summary>
	public static class AGTelephony
	{
		/// <summary>
		/// Gets the telephony device identifier.
		/// </summary>
		/// <value>The telephony device identifier.</value>
		public static string TelephonyDeviceId
		{
			get { return CallTelephonyMethod<string>("getDeviceId"); }
		}

		/// <summary>
		/// Gets the telephony sim serial number.
		/// </summary>
		/// <value>The telephony sim serial number.</value>
		public static string TelephonySimSerialNumber
		{
			get { return CallTelephonyMethod<string>("getSimSerialNumber"); }
		}

		/// <summary>
		/// Returns the ISO country code equivalent of the current registered operator's MCC (Mobile Country Code).
		/// </summary>
		/// <value>The ISO country code equivalent of the current registered operator's MCC (Mobile Country Code)..</value>
		public static string NetworkCountryIso
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return string.Empty;
				}

				return AGSystemService.TelephonyService.CallStr("getNetworkCountryIso");
			}
		}

		static T CallTelephonyMethod<T>(string methodName, params object[] args)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return default(T);
			}

			try
			{
				return AGSystemService.TelephonyService.Call<T>(methodName, args);
			}
			catch (Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("Could not call method : " + methodName +
					                 ". Check the device API level if the property is present, reason: " + e.Message);
				}
				return default(T);
			}
		}
	}
}
#endif
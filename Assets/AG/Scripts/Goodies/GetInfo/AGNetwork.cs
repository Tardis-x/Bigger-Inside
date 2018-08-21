// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGNetwork.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Retrieve information about device network connectivity.
	///
	/// Used permissions:
	/// <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	/// <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
	/// </summary>
	public static class AGNetwork
	{
		public class WifiInfo
		{
			/// <summary>
			/// the BSSID, in the form of a six-byte MAC address: XX:XX:XX:XX:XX:XX
			/// </summary>
			/// <value>the BSSID, in the form of a six-byte MAC address: XX:XX:XX:XX:XX:XX</value>
			public string BSSID { get; set; }

			/// <summary>
			/// Returns the service set identifier (SSID) of the current 802.11 network.
			/// If the SSID can be decoded as UTF-8, it will be returned surrounded by double quotation marks.
			/// Otherwise, it is returned as a string of hex digits.
			/// The SSID may be <unknown ssid> if there is no network currently connected.
			/// </summary>
			/// <value>The SSI.</value>
			public string SSID { get; set; }

			/// <summary>
			/// Mac Address.
			/// </summary>
			/// <value>Mac Address.</value>
			public string MacAddress { get; set; }

			/// <summary>
			/// Returns the current link speed in Mbps.
			/// </summary>
			/// <value>The current link speed in Mbps.</value>
			public int LinkSpeed { get; set; }

			/// <summary>
			/// Returns the service set identifier (SSID) of the current 802.11 network. If the SSID can be decoded as UTF-8, it will be returned surrounded by double quotation marks. Otherwise, it is returned as a string of hex digits. The SSID may be <unknown ssid> if there is no network currently connected.
			/// </summary>
			/// <value>Returns the service set identifier (SSID) of the current 802.11 network.</value>
			public int IpAddress { get; set; }

			/// <summary>
			/// Each configured network has a unique small integer ID, used to identify the network when performing operations on the supplicant.
			///  This property returns the ID for the currently connected network.
			/// </summary>
			/// <value>The network identifier.</value>
			public int NetworkId { get; set; }

			/// <summary>
			/// Returns the received signal strength indicator of the current 802.11 network, in dBm.
			/// </summary>
			/// <value>The rssi signal strength.</value>
			public int Rssi { get; set; }

			/// <summary>
			/// Returns a <see cref="System.String"/> that represents the current <see cref="AGNetwork"/>.
			/// </summary>
			/// <returns>A <see cref="System.String"/> that represents the current <see cref="AGNetwork"/>.</returns>
			public override string ToString()
			{
				return string.Format("[WifiInfo: BSSID={0}, SSID={1}, MacAddress={2}, LinkSpeed={3} Mbps, IpAddress={4}, NetworkId={5}, Rssi={6}]",
					BSSID, SSID, MacAddress, LinkSpeed, IpAddress, NetworkId, Rssi);
			}
		}

		const int TYPE_MOBILE = 0x00000000;
		const int TYPE_WIFI = 0x00000001;

		/// <summary>
		/// Required permission:
		/// <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
		/// Check if device is connected to the internet
		/// </summary>
		/// <returns>Whether device is connected to the internet</returns>
		public static bool IsInternetAvailable()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			AndroidJavaObject networkInfo;
			try
			{
				networkInfo = AGSystemService.ConnectivityService.CallAJO("getActiveNetworkInfo");
			}
			catch ( /* Null */ Exception)
			{
				return false;
			}
			return networkInfo.Call<bool>("isConnected");
		}

		/// <summary>
		/// Gets the Mac Address for the ethernet if available, <code>null</code> otherwise.
		/// </summary>
		[PublicAPI]
		public static string EthernetMacAddress
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return null;
				}
				
				return C.UnityHelperUtilsClass.AJCCallStaticOnce<string>("getMacAddress");
			}
		}
		
		/// <summary>
		/// Required permission: <uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
		/// </summary>
		/// <returns></returns>
		public static bool IsWifiEnabled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			try
			{
				return AGSystemService.WifiService.Call<bool>("isWifiEnabled");
			}
			catch ( /* Null */ Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("Failed to check if wi-fi is enabled. Error: " + e.Message);
				}
				return false;
			}
		}

		/// <summary>
		/// Determines if wifi is connected.
		/// </summary>
		/// <returns><c>true</c> if wifi is connected; otherwise, <c>false</c>.</returns>
		public static bool IsWifiConnected()
		{
			return IsNetworkConnected(TYPE_WIFI);
		}

		/// <summary>
		/// Determines if mobile data internet is connected.
		/// </summary>
		/// <returns><c>true</c> if mobile data internet is connected; otherwise, <c>false</c>.</returns>
		public static bool IsMobileConnected()
		{
			return IsNetworkConnected(TYPE_MOBILE);
		}

		static bool IsNetworkConnected(int networkType)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			AndroidJavaObject networkInfo;
			try
			{
				networkInfo = AGSystemService.ConnectivityService.CallAJO("getNetworkInfo", networkType);
				if (networkInfo.IsJavaNull())
				{
					return false;
				}
			}
			catch ( /* Null */ Exception)
			{
				return false;
			}
			return networkInfo.Call<bool>("isConnected");
		}

		/// <summary>
		/// Return dynamic information about the current Wi-Fi connection, if any is active.
		/// </summary>
		/// <returns>   the Wi-Fi information, contained in WifiInfo.</returns>
		public static WifiInfo GetWifiConnectionInfo()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}

			using (var wifiInfoAJO = GetWifiInfoAJO())
			{
				if (wifiInfoAJO == null)
				{
					return null;
				}

				var result = new WifiInfo
				{
					BSSID = wifiInfoAJO.CallStr("getBSSID"),
					SSID = wifiInfoAJO.CallStr("getSSID"),
					MacAddress = wifiInfoAJO.CallStr("getMacAddress"),
					LinkSpeed = wifiInfoAJO.CallInt("getLinkSpeed"),
					NetworkId = wifiInfoAJO.CallInt("getNetworkId"),
					IpAddress = wifiInfoAJO.CallInt("getIpAddress"),
					Rssi = wifiInfoAJO.CallInt("getRssi")
				};

				return result;
			}
		}

		/// <summary>
		/// Gets the wifi signal level out of 100.
		/// </summary>
		/// <returns>The wifi signal level out of 100.</returns>
		public static int GetWifiSignalLevel()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0;
			}

			var wifiInfo = GetWifiConnectionInfo();
			return wifiInfo == null ? 0 : AGSystemService.WifiService.CallStatic<int>("calculateSignalLevel", wifiInfo.Rssi, 100);
		}

		static AndroidJavaObject GetWifiInfoAJO()
		{
			try
			{
				return AGSystemService.WifiService.CallAJO("getConnectionInfo");
			}
			catch ( /* Null */ Exception e)
			{
				if (Debug.isDebugBuild)
				{
					Debug.LogWarning("Failed to get wifi info. Error: " + e.Message);
				}
				return null;
			}
		}
	}
}
#endif
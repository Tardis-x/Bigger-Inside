// 
// DOCUMENTATION FOR THIS CLASS: https://github.com/TarasOsiris/iOS-Goodies-Docs/wiki/IGDevice.cs
//



#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Runtime.InteropServices;
	using Internal;

	/// <summary>
	///     Class to get device properties. Properties are obtained from UIDevice class
	///     See: https://developer.apple.com/reference/uikit/uidevice for more documentation.
	/// </summary>
	public static class IGDevice
	{
		/// <summary>
		///     User interface type idiom.
		/// </summary>
		public enum UserInterfaceTypeIdiom
		{
			Unspecified = -1,

			/// <summary>
			///     iPhone and iPod touch style UI
			/// </summary>
			Phone = 0,

			/// <summary>
			///     iPad style UI
			/// </summary>
			Pad = 1,

			/// <summary>
			///     Apple TV style UI
			/// </summary>
			AppleTV = 2,

			/// <summary>
			///     CarPlay style UI
			/// </summary>
			CarPlay = 3
		}

		/// <summary>
		///     A Boolean value indicating whether multitasking is supported on the current device.
		/// </summary>
		/// <value><c>true</c> if multitasking is supported; otherwise, <c>false</c>.</value>
		public static bool IsMultitaskingSupported
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return false;
				}

				return _uiDeviceIsMultitaskingSupported();
			}
		}

		/// <summary>
		///     The name identifying the device.
		/// </summary>
		/// <value>The name identifying the device.</value>
		public static string Name
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return string.Empty;
				}

				return _uiDeviceGetName();
			}
		}

		/// <summary>
		///     The name of the operating system running on the device represented by the receiver.
		/// </summary>
		/// <value>The name of the operating system running on the device represented by the receiver.</value>
		public static string SystemName
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return string.Empty;
				}

				return _uiDeviceGetSystemName();
			}
		}

		/// <summary>
		///     The current version of the operating system.
		/// </summary>
		/// <value>The current version of the operating system.</value>
		public static string SystemVersion
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return string.Empty;
				}

				return _uiDeviceGetSystemVersion();
			}
		}

		/// <summary>
		///     The model of the device.
		/// </summary>
		/// <value>The model of the device.</value>
		public static string Model
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return string.Empty;
				}

				return _uiDeviceGetModel();
			}
		}

		/// <summary>
		///     The model of the device as a localized string.
		/// </summary>
		/// <value>The model of the device as a localized string.</value>
		public static string LocalizedModel
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return string.Empty;
				}

				return _uiDeviceGetLocalizedModel();
			}
		}

		/// <summary>
		///     The style of interface to use on the current device.
		/// </summary>
		/// <value>The style of interface to use on the current device.</value>
		public static UserInterfaceTypeIdiom UserInterfaceIdiom
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return UserInterfaceTypeIdiom.Unspecified;
				}
				// TODO to enum
				return (UserInterfaceTypeIdiom) _uiDeviceGetUserInterfaceIdiom();
			}
		}

		/// <summary>
		///     An alphanumeric string that uniquely identifies a device to the app’s vendor.
		/// </summary>
		/// <value>An alphanumeric string that uniquely identifies a device to the app’s vendor.</value>
		public static string UUID
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return string.Empty;
				}

				return _uiDeviceGetUUID();
			}
		}

		[DllImport("__Internal")]
		static extern bool _uiDeviceIsMultitaskingSupported();

		[DllImport("__Internal")]
		static extern string _uiDeviceGetName();

		[DllImport("__Internal")]
		static extern string _uiDeviceGetSystemName();

		[DllImport("__Internal")]
		static extern string _uiDeviceGetSystemVersion();

		[DllImport("__Internal")]
		static extern string _uiDeviceGetModel();

		[DllImport("__Internal")]
		static extern string _uiDeviceGetLocalizedModel();

		[DllImport("__Internal")]
		static extern int _uiDeviceGetUserInterfaceIdiom();

		[DllImport("__Internal")]
		static extern string _uiDeviceGetUUID();

		[DllImport("__Internal")]
		static extern float _uiDeviceGetBatteryLevel();

		[DllImport("__Internal")]
		static extern bool _uiDeviceIsBatteryMonitoringEnabled();

		[DllImport("__Internal")]
		static extern int _uiDeviceGetBatteryState();

		[DllImport("__Internal")]
		static extern void _uiDeviceSetBatteryMonitoringEnabled(bool enabled);

		[DllImport("__Internal")]
		static extern void _uiDeviceSetProximityMonitoringEnabled(bool enabled);

		[DllImport("__Internal")]
		static extern bool _uiDeviceIsProximityMonitoringEnabled();

		[DllImport("__Internal")]
		static extern bool _uiDeviceProximityState();

		#region battery

		/// <summary>
		///     Battery state.
		/// </summary>
		public enum BatteryState
		{
			/// <summary>
			///     The unknown.
			/// </summary>
			Unknown = 0,

			/// <summary>
			///     On battery, discharging.
			/// </summary>
			Unplugged = 1,

			/// <summary>
			///     Plugged in, less than 100%
			/// </summary>
			Charging = 2,

			/// <summary>
			///     Plugged in, at 100%
			/// </summary>
			Full = 3
		}

		/// <summary>
		///     The battery charge level for the device.
		/// </summary>
		/// <value>The battery charge level for the device.</value>
		public static float BatteryLevel
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return 0f;
				}

				return _uiDeviceGetBatteryLevel();
			}
		}

		/// <summary>
		///     A boolean value indicating whether battery monitoring is enabled (true) or not (false).
		/// </summary>
		/// <value><c>true</c> if battery monitoring is enabled ; otherwise, <c>false</c>.</value>
		public static bool IsBatteryMonitoringEnabled
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return false;
				}

				return _uiDeviceIsBatteryMonitoringEnabled();
			}
			set
			{
				if (IGUtils.IsIosCheck())
				{
					return;
				}

				_uiDeviceSetBatteryMonitoringEnabled(value);
			}
		}

		/// <summary>
		///     The battery state for the device.
		/// </summary>
		/// <value>The battery state for the device.</value>
		public static BatteryState DeviceBatteryState
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return BatteryState.Unknown;
				}

				return (BatteryState) _uiDeviceGetBatteryState();
			}
		}

		#endregion

		#region proximity_sensor

		/// <summary>
		///     Gets or sets a value indicating whether proximity monitoring is enabled.
		/// </summary>
		/// <value><c>true</c> if proximity monitoring is enabled; otherwise, <c>false</c>.</value>
		public static bool IsProximityMonitoringEnabled
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return false;
				}

				return _uiDeviceIsProximityMonitoringEnabled();
			}
			set
			{
				if (IGUtils.IsIosCheck())
				{
					return;
				}

				_uiDeviceSetProximityMonitoringEnabled(value);
			}
		}

		/// <summary>
		///     A Boolean value indicating whether the proximity sensor is close to the user (true) or not (false).
		/// </summary>
		/// <value><c>true</c> if proximity sensor is close to the user; otherwise, <c>false</c>.</value>
		public static bool ProximityState
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return false;
				}

				return _uiDeviceProximityState();
			}
		}

		#endregion
	}
}
#endif
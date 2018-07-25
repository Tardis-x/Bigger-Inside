namespace DeadMosquito.IosGoodies.Example
{
	using System.Text;
	using UnityEngine;
	using UnityEngine.UI;

	public class IGInfoExample : MonoBehaviour
	{
		public Text infoText;
#if UNITY_IOS

		IGDevice.BatteryState _batteryState;
		float _batteryLevel;

		bool _proximityState;


		// Setup
		void Awake()
		{
			// Enable battery monitoring so we ca nget battery level 
			IGDevice.IsBatteryMonitoringEnabled = true;
			// Enable proximity sensor monitoring
			IGDevice.IsProximityMonitoringEnabled = true;

			UpdateData();
		}

		// monitor changes
		void Update()
		{
			UpdateData();
		}

		void UpdateData()
		{
			_batteryState = IGDevice.DeviceBatteryState;
			_batteryLevel = IGDevice.BatteryLevel;
			_proximityState = IGDevice.ProximityState;
		}

		void OnEnable()
		{
			var builder = new StringBuilder();
			// Device info
			builder.AppendLine("Multitasking supported? : " + IGDevice.IsMultitaskingSupported);
			builder.AppendLine("Device name? : " + IGDevice.Name);
			builder.AppendLine("System name? : " + IGDevice.SystemName);
			builder.AppendLine("System version? : " + IGDevice.SystemVersion);
			builder.AppendLine("Model? : " + IGDevice.Model);
			builder.AppendLine("Localized Model? : " + IGDevice.LocalizedModel);
			builder.AppendLine("User Interface Idiom? : " + IGDevice.UserInterfaceIdiom);
			builder.AppendLine("UUID? : " + IGDevice.UUID);
			builder.AppendLine("\n>>> Batterry");
			builder.AppendLine("Is battery monitoring enabled? : " + IGDevice.IsBatteryMonitoringEnabled);
			builder.AppendLine("Battery level? : " + _batteryLevel);
			builder.AppendLine("Battery state? : " + _batteryState);

			builder.AppendLine("\n>>> Proximity sensor");
			builder.AppendLine("Is proximity monitoring enabled? : " + IGDevice.IsProximityMonitoringEnabled);
			builder.AppendLine("Proximity state? : " + _proximityState);

			infoText.text = builder.ToString();
		}
#endif
	}
}
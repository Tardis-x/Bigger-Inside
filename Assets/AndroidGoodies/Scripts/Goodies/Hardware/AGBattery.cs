// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGBattery.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using UnityEngine;

	public static class AGBattery
	{
		const string BatteryManagerEXTRA_LEVEL = "level";
		const string BatteryManagerEXTRA_SCALE = "scale";

		/// <summary>
		/// Gets the battery charge level from 1-100.
		/// </summary>
		/// <returns>The battery charge level from 1-100.</returns>
		public static float GetBatteryChargeLevel()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return 0f;
			}

			using (var batteryIntentFilter = new AndroidJavaObject("android.content.IntentFilter", AndroidIntent.ACTION_BATTERY_CHANGED))
			{
				using (var batteryIntent = AGUtils.Activity.CallAJO("registerReceiver", null, batteryIntentFilter))
				{
					int level = batteryIntent.Call<int>("getIntExtra", BatteryManagerEXTRA_LEVEL, -1);
					int scale = batteryIntent.Call<int>("getIntExtra", BatteryManagerEXTRA_SCALE, -1);

					// Error checking that probably isn't needed but I added just in case.
					if (level == -1 || scale == -1)
					{
						return 50.0f;
					}

					return ((float) level / (float) scale) * 100.0f;
				}
			}
		}
	}
}
#endif
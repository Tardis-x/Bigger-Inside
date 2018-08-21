// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGVibrator.cs
//


#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;

	public static class AGVibrator
	{
		/// <summary>
		/// Check whether the hardware has a vibrator.
		/// </summary>
		/// <returns><c>true</c> if the hardware has a vibrator; otherwise, <c>false</c>.</returns>
		public static bool HasVibrator()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			try
			{
				return AGSystemService.VibratorService.Call<bool>("hasVibrator");
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Vibrate constantly for the specified period of time.
		/// 
		/// You must specify <uses-permission android:name="android.permission.VIBRATE"/> permission in order for this method to work.
		/// </summary>
		/// <param name="durationInMillisec">Vibration duration in millisec.</param>
		public static void Vibrate(long durationInMillisec)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			AGSystemService.VibratorService.Call("vibrate", durationInMillisec);
		}

		const int REPEAT = -1;
		// Do not repeat for now

		/// <summary>
		/// Vibrate with a given pattern.
		/// </summary>
		/// <param name="pattern">
		/// Pass in an array of ints that are the durations for which to turn on or off the vibrator in milliseconds. 
		/// The first value indicates the number of milliseconds to wait before turning the vibrator on. 
		/// The next value indicates the number of milliseconds for which to keep the vibrator on before turning it off. 
		/// Subsequent values alternate between durations in milliseconds to turn the vibrator off or to turn the vibrator on.
		/// </param>
		public static void VibratePattern(long[] pattern)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			AGSystemService.VibratorService.Call("vibrate", pattern, REPEAT);
		}
	}
}

#endif
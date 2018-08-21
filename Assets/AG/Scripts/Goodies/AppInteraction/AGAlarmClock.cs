// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGAlarmClock.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System.Diagnostics.CodeAnalysis;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Android class to set/show alarms and timer.
	/// </summary>
	public static class AGAlarmClock
	{
		/// <summary>
		/// Alarm days of week.
		/// </summary>
		[SuppressMessage("ReSharper", "UnusedMember.Global")]
		public enum AlarmDays
		{
			Sunday = 0x00000001,
			Monday = 0x00000002,
			Tuesday = 0x00000003,
			Wednesday = 0x00000004,
			Thursday = 0x00000005,
			Friday = 0x00000006,
			Saturday = 0x00000007
		}

		const string ACTION_SET_ALARM = "android.intent.action.SET_ALARM";
		const string ACTION_SHOW_ALARMS = "android.intent.action.SHOW_ALARMS";
		const string ACTION_SET_TIMER = "android.intent.action.SET_TIMER";

		const string EXTRA_HOUR = "android.intent.extra.alarm.HOUR";
		const string EXTRA_MINUTES = "android.intent.extra.alarm.MINUTES";
		const string EXTRA_MESSAGE = "android.intent.extra.alarm.MESSAGE";
		const string EXTRA_DAYS = "android.intent.extra.alarm.DAYS";
		const string EXTRA_VIBRATE = "android.intent.extra.alarm.VIBRATE";
		const string EXTRA_RINGTONE = "android.intent.extra.alarm.RINGTONE";
		const string EXTRA_SKIP_UI = "android.intent.extra.alarm.SKIP_UI";

		const string EXTRA_LENGTH = "android.intent.extra.alarm.LENGTH";

		/// <summary>
		/// Indicates whether any app to show alarms is installed
		/// </summary>
		/// <returns><c>true</c>, if any app to show alarms is installed, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool CanShowListOfAlarms()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			using (var intent = new AndroidIntent(ACTION_SHOW_ALARMS))
			{
				return intent.ResolveActivity();
			}
		}

		/// <summary>
		/// Indicates whether any app to set alarms is installed
		/// </summary>
		/// <returns><c>true</c>, if any app to set alarms is installed, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool CanSetAlarm()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			using (var intent = new AndroidIntent(ACTION_SET_ALARM))
			{
				return intent.ResolveActivity();
			}
		}

		/// <summary>
		/// Indicates whether any app to set timer is installed
		/// </summary>
		/// <returns><c>true</c>, if any app to timer alarms is installed, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool CanSetTimer()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			using (var intent = new AndroidIntent(ACTION_SET_TIMER))
			{
				return intent.ResolveActivity();
			}
		}

		/// <summary>
		/// Shows all alarms.
		/// </summary>
		[PublicAPI]
		public static void ShowAllAlarms()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			using (var intent = new AndroidIntent(ACTION_SHOW_ALARMS))
			{
				AGUtils.StartActivity(intent.AJO);
			}
		}

		/// <summary>
		/// Sets the alarm.
		/// </summary>
		/// <param name="hour">The hour of the alarm being set.</param>
		/// <param name="minute">he minutes of the alarm being set.</param>
		/// <param name="message">A custom message for the alarm.</param>
		/// <param name="days">Weekdays for repeating alarm. If not set the alarm will be once</param>
		/// <param name="vibrate">Whether or not to activate the device vibrator for this alarm.</param>
		/// <param name="skipUI">Whether or not to display an activity for setting this alarm.</param>
		[PublicAPI]
		public static void SetAlarm(int hour, int minute, string message, AlarmDays[] days = null,
			bool vibrate = true, bool skipUI = false)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			using (var intent = new AndroidIntent(ACTION_SET_ALARM))
			{
				intent.PutExtra(EXTRA_HOUR, hour);
				intent.PutExtra(EXTRA_MINUTES, minute);
				intent.PutExtra(EXTRA_MESSAGE, message);
				if (days != null)
				{
					intent.PutExtra(EXTRA_DAYS, CreateDaysArrayList(days));
				}
				intent.PutExtra(EXTRA_VIBRATE, vibrate);
				intent.PutExtra(EXTRA_SKIP_UI, skipUI);

				AGUtils.StartActivity(intent.AJO);
			}
		}

		static AndroidJavaObject CreateDaysArrayList(AlarmDays[] days)
		{
			var list = new AndroidJavaObject("java.util.ArrayList");
			foreach (var day in days)
			{
				list.Call<bool>("add", new AndroidJavaObject("java.lang.Integer", (int) day));
			}

			return list;
		}

		/// <summary>
		/// Indicates whether user has the timer app installed
		/// </summary>
		/// <returns><c>true</c>, if user has any timer app installed, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool UserHasTimerApp()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			using (var intent = new AndroidIntent(ACTION_SET_TIMER))
			{
				return intent.ResolveActivity();
			}
		}

		/// <summary>
		/// Opens or starts the countdown timer application
		/// </summary>
		/// <param name="lengthInSeconds">The length of the timer in seconds.</param>
		/// <param name="message">A custom message to identify the timer.</param>
		/// <param name="skipUi">A boolean specifying whether the responding app should skip its UI when setting the timer. If true, the app should bypass any confirmation UI and simply start the specified timer.</param>
		[PublicAPI]
		public static void SetTimer(int lengthInSeconds, string message, bool skipUi = false)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			using (var intent = new AndroidIntent(ACTION_SET_TIMER))
			{
				intent.PutExtra(EXTRA_LENGTH, lengthInSeconds);
				intent.PutExtra(EXTRA_MESSAGE, message);
				intent.PutExtra(EXTRA_SKIP_UI, skipUi);
				AGUtils.StartActivity(intent.AJO);
			}
		}
	}
}
#endif
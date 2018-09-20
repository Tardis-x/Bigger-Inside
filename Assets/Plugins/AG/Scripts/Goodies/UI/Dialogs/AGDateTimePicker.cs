//
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGDateTimePicker.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using UnityEngine;

	public static class AGDateTimePicker
	{
		public delegate void OnDatePicked(int year, int month, int day);

		public delegate void OnTimePicked(int hourOfDay, int minute);

		const string DatePickerClass = "android.app.DatePickerDialog";
		const string TimePickerClass = "android.app.TimePickerDialog";

		/// <summary>
		/// Shows the default Android date picker.
		/// </summary>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		/// <param name="day">Day.</param>
		/// <param name="onDatePicked">Date was picked callback.</param>
		/// <param name="onCancel">Dialog was cancelled callback.</param>
		/// <param name="theme">Dialog theme</param>
		public static void ShowDatePicker(int year, int month, int day, OnDatePicked onDatePicked, Action onCancel, AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			AGUtils.RunOnUiThread(() =>
			{
				var listener = new OnDateSetListenerPoxy(onDatePicked);
				int themeResource = AndroidDialogUtils.GetDialogTheme(theme);

				//  Month! (0-11 for compatibility with MONTH)
				var dialog = AndroidDialogUtils.IsDefault(themeResource)
					? new AndroidJavaObject(DatePickerClass, AGUtils.Activity, listener, year, month - 1, day)
					: new AndroidJavaObject(DatePickerClass, AGUtils.Activity, themeResource, listener, year, month - 1, day);

				dialog.Call("setOnCancelListener", new DialogOnCancelListenerPoxy(onCancel));
				AndroidDialogUtils.ShowWithImmersiveModeWorkaround(dialog);
			});
		}

		/// <summary>
		/// Shows the default Android time picker.
		/// </summary>
		/// <param name="hourOfDay">Hour of day.</param>
		/// <param name="minute">Minute. Not Zero-base as on Android!</param>
		/// <param name="onTimePicked">Time was picked callback.</param>
		/// <param name="onCancel">Dialog was cancelled callback.</param>
		/// <param name="theme">Dialog theme</param>
		/// <param name="is24HourFormat">If the picker is in 24 hour format</param>
		public static void ShowTimePicker(int hourOfDay, int minute, OnTimePicked onTimePicked, Action onCancel, AGDialogTheme theme = AGDialogTheme.Default, bool is24HourFormat = true)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			AGUtils.RunOnUiThread(() =>
			{
				var listener = new OnTimeSetListenerPoxy(onTimePicked);
				int themeResource = AndroidDialogUtils.GetDialogTheme(theme);

				var dialog = AndroidDialogUtils.IsDefault(themeResource)
					? new AndroidJavaObject(TimePickerClass, AGUtils.Activity, listener, hourOfDay, minute, is24HourFormat)
					: new AndroidJavaObject(TimePickerClass, AGUtils.Activity, themeResource, listener, hourOfDay, minute, is24HourFormat);
				dialog.Call("setOnCancelListener", new DialogOnCancelListenerPoxy(onCancel));
				AndroidDialogUtils.ShowWithImmersiveModeWorkaround(dialog);
			});
		}

		class OnDateSetListenerPoxy : AndroidJavaProxy
		{
			readonly OnDatePicked _onDatePicked;

			public OnDateSetListenerPoxy(OnDatePicked onDatePicked)
				: base("android.app.DatePickerDialog$OnDateSetListener")
			{
				_onDatePicked = onDatePicked;
			}

			// ReSharper disable once InconsistentNaming
			// ReSharper disable once UnusedMember.Local
			// ReSharper disable once UnusedParameter.Local
			void onDateSet(AndroidJavaObject view, int year, int month, int dayOfMonth)
			{
				//  Month! (0-11 for compatibility with MONTH)
				GoodiesSceneHelper.Queue(() => _onDatePicked(year, month + 1, dayOfMonth));
			}
		}

		class OnTimeSetListenerPoxy : AndroidJavaProxy
		{
			readonly OnTimePicked _onTimePicked;

			public OnTimeSetListenerPoxy(OnTimePicked onTimePicked)
				: base("android.app.TimePickerDialog$OnTimeSetListener")
			{
				_onTimePicked = onTimePicked;
			}

			// ReSharper disable once InconsistentNaming
			// ReSharper disable once UnusedMember.Local
			// ReSharper disable once UnusedParameter.Local
			void onTimeSet(AndroidJavaObject view, int hourOfDay, int minute)
			{
				GoodiesSceneHelper.Queue(() => _onTimePicked(hourOfDay, minute));
			}
		}
	}
}
#endif
// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGProgressDialog.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using UnityEngine;

	public class AGProgressDialog
	{
		enum Style
		{
			Horizontal = 0x00000001,
			Spinner = 0x00000000
		}

		const string ProgressDialogClass = "android.app.ProgressDialog";

		AndroidJavaObject _dialog;

		AGProgressDialog(Style style, string title, string message, AGDialogTheme theme, int maxValue = 100)
		{
			AGUtils.RunOnUiThread(
				() =>
				{
					var d = CreateDialog(theme);
					d.Call("setProgressStyle", (int) style);
					d.Call("setCancelable", false);
					d.Call("setTitle", title);
					d.Call("setMessage", message);
					d.Call("setCancelable", false);
					if (style == Style.Spinner)
					{
						d.Call("setIndeterminate", true);
					}
					else
					{
						d.Call("setIndeterminate", false);
						d.Call("setProgress", 0);
						d.Call("setMax", maxValue);
					}
					_dialog = d;
				});
		}

		static AndroidJavaObject CreateDialog(AGDialogTheme theme)
		{
			int themeResource = AndroidDialogUtils.GetDialogTheme(theme);
			return AndroidDialogUtils.IsDefault(themeResource) ? new AndroidJavaObject(ProgressDialogClass, AGUtils.Activity) : new AndroidJavaObject(ProgressDialogClass, AGUtils.Activity, themeResource);
		}

		#region factory_methods

		/// <summary>
		/// Creates the spinner dialog. 
		/// Call <see cref="Show"/> to show the spinner dialog.
		/// Call <see cref="Dismiss"/> to hide the spinner dialog.
		/// </summary>
		/// <returns>The spinner dialog.</returns>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="theme">Theme of the dialog.</param>
		public static AGProgressDialog CreateSpinnerDialog(string title, string message,
			AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}

			return new AGProgressDialog(Style.Spinner, title, message, theme);
		}

		/// <summary>
		/// Creates the horizontal loading dialog
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="message">Dialog message</param>
		/// <param name="maxValue">Maximum value of the progress bar</param>
		/// <param name="theme">Theme of the dialog.</param>
		/// <returns></returns>
		public static AGProgressDialog CreateHorizontalDialog(string title, string message, int maxValue,
			AGDialogTheme theme = AGDialogTheme.Default)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}

			return new AGProgressDialog(Style.Horizontal, title, message, theme, maxValue);
		}

		#endregion

		/// <summary>
		/// Shows the dialog on screen.
		/// </summary>
		public void Show()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			AGUtils.RunOnUiThread(() => _dialog.Call("show"));
		}


		/// <summary>
		/// Dismisses the dialog.
		/// </summary>
		public void Dismiss()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			AGUtils.RunOnUiThread(() =>
			{
				_dialog.Call("dismiss");
				_dialog.Dispose();
			});
		}

		#region horizontal_progress

		/// <summary>
		/// Sets the progress of the dialog. Call this only on horizontal progress bar dialog
		/// </summary>
		/// <param name="value">Progress value.</param>
		public void SetProgress(int value)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			AGUtils.RunOnUiThread(() => _dialog.Call("setProgress", value));
		}

		#endregion
	}
}
#endif
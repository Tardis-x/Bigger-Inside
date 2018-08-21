// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGDialer.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using Internal;
	using UnityEngine;

	/// <summary>
	/// Android class to place phone calls.
	/// </summary>
	public static class AGDialer
	{
		/// <summary>
		/// Indicates whether the device has the app installed which can place phone calls
		/// </summary>
		/// <returns><c>true</c>, if user has any phone app installed, <c>false</c> otherwise.</returns>
		public static bool UserHasPhoneApp()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			using (var i = new AndroidIntent(AndroidIntent.ACTION_DIAL))
			{
				return i.ResolveActivity();
			}
		}

		/// <summary>
		/// Opens the dialer with the number provided.
		/// </summary>
		/// <param name="phoneNumber">Phone number.</param>
		public static void OpenDialer(string phoneNumber)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			using (var i = new AndroidIntent(AndroidIntent.ACTION_DIAL))
			{
				i.SetData(ParsePhoneNumber(phoneNumber));
				AGUtils.StartActivity(i.AJO);
			}
		}

		/// <summary>
		/// Places the phone call immediately.
		/// 
		/// You need <uses-permission android:name="android.permission.CALL_PHONE" /> to use this method!
		/// </summary>
		/// <param name="phoneNumber">Phone number.</param>
		public static void PlacePhoneCall(string phoneNumber)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			using (var i = new AndroidIntent(AndroidIntent.ACTION_CALL))
			{
				i.SetData(ParsePhoneNumber(phoneNumber));
				AGUtils.StartActivity(i.AJO);
			}
		}

		static AndroidJavaObject ParsePhoneNumber(string phoneNumber)
		{
			return AndroidUri.Parse("tel:" + phoneNumber);
		}
	}
}
#endif
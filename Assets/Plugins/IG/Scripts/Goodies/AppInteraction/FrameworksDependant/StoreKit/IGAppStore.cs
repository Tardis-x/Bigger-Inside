#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Runtime.InteropServices;
	using JetBrains.Annotations;

	/// <summary>
	/// Class for AppStore interaction
	/// </summary>
	[PublicAPI]
	public static class IGAppStore
	{
		/// <summary>
		/// Tells StoreKit to ask the user to rate or review your app, if appropriate.
		/// 
		/// Although you should call this method when it makes sense in the user experience flow of your app, the actual display of a rating/review request view is governed by App Store policy. Because this method may or may not present an alert, it's not appropriate to call it in response to a button tap or other user action.
		/// 
		/// Requests review for AppStore. For more detail see https://developer.apple.com/documentation/storekit/skstorereviewcontroller/2851536-requestreview
		/// </summary>
		[PublicAPI]
		public static void RequestReview()
		{
			_goodiesRequestReview();
		}
		
		[DllImport("__Internal")]
		static extern void _goodiesRequestReview();
	}
}
#endif
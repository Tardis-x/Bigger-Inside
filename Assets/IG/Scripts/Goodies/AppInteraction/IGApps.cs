// 
// DOCUMENTATION FOR THIS CLASS: https://github.com/TarasOsiris/iOS-Goodies-Docs/wiki/IGApps.cs
//


#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Runtime.InteropServices;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine.iOS;

	[PublicAPI]
	public static class IGApps
	{
		/// <summary>
		///     Opens the YouTube video to view.
		///     If the YouTube video cannot be viewed on the device, iOS displays an appropriate warning message to the user.
		/// </summary>
		/// <param name="videoId">Id of the YouTube video</param>
		[PublicAPI]
		public static void OpenYoutubeVideo(string videoId)
		{
			Check.Argument.IsStrNotNullOrEmpty(videoId, "videoId");

			if (IGUtils.IsIosCheck())
			{
				return;
			}

			IGUtils._openUrl(Device.systemVersion.StartsWith("11") ? string.Format("youtube://{0}", videoId) : string.Format("http://www.youtube.com/watch?v={0}", videoId));
		}

		/// <summary>
		///     Starts the face time video call.
		/// </summary>
		/// <param name="userId">User identifier.</param>
		[PublicAPI]
		public static void StartFaceTimeVideoCall(string userId)
		{
			Check.Argument.IsStrNotNullOrEmpty(userId, "userId");

			if (IGUtils.IsIosCheck())
			{
				return;
			}

			IGUtils._openUrl(string.Format("facetime:{0}", userId));
		}

		/// <summary>
		///     Starts the face time audio call.
		/// </summary>
		/// <param name="userId">User identifier.</param>
		[PublicAPI]
		public static void StartFaceTimeAudioCall(string userId)
		{
			Check.Argument.IsStrNotNullOrEmpty(userId, "userId");

			if (IGUtils.IsIosCheck())
			{
				return;
			}

			IGUtils._openUrl(string.Format("facetime-audio:{0}", userId));
		}

		/// <summary>
		/// Shows the dialer dialogue that prompts the user to make a call.
		/// </summary>
		/// <param name="phoneNumber">Phone number to call.</param>
		[PublicAPI]
		public static void OpenDialer(string phoneNumber)
		{
			Check.Argument.IsStrNotNullOrEmpty(phoneNumber, "phoneNumber");

			if (IGUtils.IsIosCheck())
			{
				return;
			}

			IGUtils._openUrl(string.Format("tel:{0}", phoneNumber));
		}

		/// <summary>
		/// Opens the screen dedicated to you application settings where user can enable permissions, etc.
		/// </summary>
		[PublicAPI]
		public static void OpenAppSettings()
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}
			
			_goodiesOpenAppSettings();
		}

		/// <summary>
		/// Opens existing app AppStore page.
		/// </summary>
		/// <param name="appId">Id of the app. To find the id of the existing app copy it from iTunes link.</param>
		[PublicAPI]
		public static void OpenAppOnAppStore(string appId)
		{
			Check.Argument.IsStrNotNullOrEmpty(appId, "appId");
			
			if (IGUtils.IsIosCheck())
			{
				return;
			}
			
			var iTunesUrl = string.Format("itms://itunes.apple.com/us/app/apple-store/id{0}?mt=8", appId);
			IGUtils._openUrl(iTunesUrl);
		}
		
		[DllImport("__Internal")]
		static extern void _goodiesOpenAppSettings();
	}
}
#endif
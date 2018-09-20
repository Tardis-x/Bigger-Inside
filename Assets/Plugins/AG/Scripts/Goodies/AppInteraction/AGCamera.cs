// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGCamera.cs
//


using JetBrains.Annotations;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;

	/// <summary>
	/// Methods to interact with device camera.
	/// </summary>
	[PublicAPI]
	public static class AGCamera
	{
		

		#region take_photo

				[PublicAPI]
		public static void TakePhoto(Action<ImagePickResult> onSuccess, Action<string> onError,
			ImageResultSize maxSize = ImageResultSize.Original, bool shouldGenerateThumbnails = true)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (onSuccess == null)
			{
				throw new ArgumentNullException("onSuccess", "Success callback cannot be null");
			}

			_onSuccessAction = onSuccess;
			_onCancelAction = onError;

			AGUtils.RunOnUiThread(() => AGActivityUtils.TakePhoto(maxSize, shouldGenerateThumbnails));
		}

		static Action<ImagePickResult> _onSuccessAction;
		static Action<string> _onCancelAction;

		// Invoked by UnityPlayer.SendMessage

		internal static void OnSuccessTrigger(string imageCallbackJson)
		{
			if (_onSuccessAction != null)
			{
				var image = ImagePickResult.FromJson(imageCallbackJson);
				_onSuccessAction(image);
				_onCancelAction = null;
			}
		}

		// Invoked by UnityPlayer.SendMessage

		internal static void OnErrorTrigger(string errorMessage)
		{
			if (_onCancelAction != null)
			{
				_onCancelAction(errorMessage);
				_onCancelAction = null;
			}
		}

		#endregion

		
	}
}
#endif
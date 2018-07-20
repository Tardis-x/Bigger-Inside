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
		/// <summary>
		/// Check if device has camera
		/// </summary>
		/// <returns><c>true</c>, if device has camera, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool DeviceHasCamera()
		{
			return AGDeviceInfo.SystemFeatures.HasSystemFeature(AGDeviceInfo.SystemFeatures.FEATURE_CAMERA);
		}

		/// <summary>
		/// Check if device has frontal camera
		/// </summary>
		/// <returns><c>true</c>, if device has frontal camera, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool DeviceHasFrontalCamera()
		{
			return AGDeviceInfo.SystemFeatures.HasSystemFeature(AGDeviceInfo.SystemFeatures.FEATURE_CAMERA_FRONT);
		}

		/// <summary>
		/// Check if device has camera with autofocus
		/// </summary>
		/// <returns><c>true</c>, if device has camera with autofocus, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool DeviceHasCameraWithAutoFocus()
		{
			return DeviceHasCamera() && AGDeviceInfo.SystemFeatures.HasSystemFeature(AGDeviceInfo.SystemFeatures.FEATURE_CAMERA_AUTOFOCUS);
		}

		/// <summary>
		/// Check if device has camera with flashlight
		/// </summary>
		/// <returns><c>true</c>, if device has camera with flashlight, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool DeviceHasCameraWithFlashlight()
		{
			return DeviceHasCamera() && AGDeviceInfo.SystemFeatures.HasSystemFeature(AGDeviceInfo.SystemFeatures.FEATURE_CAMERA_FLASH);
		}
		
		
		/// <summary>
		/// Check if device has camera app installed that can handle taking photo
		/// </summary>
		/// <returns><c>true</c>, if device has camera app installed that can handle taking photo, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool DeviceHasCameraAppInstalled()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}
			
			return new AndroidIntent("android.media.action.IMAGE_CAPTURE").ResolveActivity();
		}

		#region take_photo

		/// <summary>
		/// Required permissions:
		///     <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
		/// 
		/// Launches the camera app to take a photo and returns resulting Texture2D in callback. The photo is also saved to the device gallery.
		/// 
		/// IMPORTANT! : You don't need any permissions to use this method. It works using Android intent.
		/// </summary>
		/// <param name="onSuccess">On success callback. Image is received as callback parameter</param>
		/// <param name="onError">On error/cancel callback.</param>
		/// <param name="maxSize">Max image size. If provided image will be downscaled.</param>
		/// <param name="shouldGenerateThumbnails">Whether thumbnail images will be generated. Used to show small previews of image.</param>
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

		#region take_video

		static Action<VideoPickResult> _onVideoSuccessAction;
		static Action<string> _onVideoCancelAction;

		/// <summary>
		/// Take the video file using the device camera
		/// </summary>
		/// <param name="onSuccess">Video file was successfully picked by the user. Result is received in a callback.</param>
		/// <param name="onError">Picking video file failed.</param>
		[PublicAPI]
		public static void RecordVideo(Action<VideoPickResult> onSuccess, Action<string> onError,
			bool generatePreviewImages = true)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(onSuccess, "onSuccess");
			Check.Argument.IsNotNull(onError, "onError");
			_onVideoSuccessAction = onSuccess;
			_onVideoCancelAction = onError;

			AGActivityUtils.PickVideoCamera(generatePreviewImages);
		}

		[PublicAPI]
		public static void OnVideoSuccessTrigger(string json)
		{
			if (_onVideoSuccessAction != null)
			{
				var result = VideoPickResult.FromJson(json);
				_onVideoSuccessAction(result);
				_onVideoSuccessAction = null;
			}
		}

		[PublicAPI]
		public static void OnVideoErrorTrigger(string message)
		{
			if (_onVideoCancelAction != null)
			{
				_onVideoCancelAction(message);
				_onVideoCancelAction = null;
			}
		}

		#endregion
	}
}
#endif
// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGGallery.cs
//



#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	/// Methods to interact with device gallery.
	/// </summary>
	[PublicAPI]
	public static class AGGallery
	{
		static Action<ImagePickResult> _onSuccessAction;
		static Action<string> _onCancelAction;

		/// <summary>
		/// Picks the image from gallery.
		/// </summary>
		/// <param name="onSuccess">On success callback. Image is received as callback parameter</param>
		/// <param name="onError">On error callback.</param>
		/// <param name="maxSize">Max image size. If provided image will be downscaled.</param>
		/// <param name="shouldGenerateThumbnails">Whether thumbnail images will be generated. Used to show small previews of image.</param>
		[PublicAPI]
		public static void PickImageFromGallery([NotNull] Action<ImagePickResult> onSuccess, Action<string> onError,
			ImageResultSize maxSize = ImageResultSize.Original, bool shouldGenerateThumbnails = true)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(onSuccess, "onSuccess");

			_onSuccessAction = onSuccess;
			_onCancelAction = onError;

			AGActivityUtils.PickPhotoFromGallery(maxSize, shouldGenerateThumbnails);
		}

		// Invoked by UnityPlayer.SendMessage
		/// <summary>
		/// Saves the image to android gallery.
		/// </summary>
		/// <returns>The image to save to the gallery.</returns>
		/// <param name="texture2D">Texture2D to save.</param>
		/// <param name="title">Title.</param>
		/// <param name="folder">Inner folder in Pictures directory. Must be a valid folder name</param>
		/// <param name="imageFormat">Image format.</param>
		[PublicAPI]
		public static void SaveImageToGallery([NotNull] Texture2D texture2D, [NotNull] string title, string folder = null,
			ImageFormat imageFormat = ImageFormat.PNG)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			AGFileUtils.SaveImageToGallery(texture2D, title, folder, imageFormat);
		}

		/// <summary>
		/// Call this method after you have saved the image for it to appear in gallery applications
		/// </summary>
		/// <param name="filePath">File path to scan</param>
		[PublicAPI]
		public static void RefreshFile([NotNull] string filePath)
		{
			Check.Argument.IsStrNotNullOrEmpty(filePath, "path");
			
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}
			
			AndroidPersistanceUtilsInternal.RefreshGallery(filePath);
		}

		internal static void OnSuccessTrigger(string imageCallbackJson)
		{
			if (_onSuccessAction == null)
			{
				return;
			}

			var image = ImagePickResult.FromJson(imageCallbackJson);
			_onSuccessAction(image);
		}

		// Invoked by UnityPlayer.SendMessage

		internal static void OnErrorTrigger(string errorMessage)
		{
			if (_onCancelAction == null)
			{
				return;
			}

			_onCancelAction(errorMessage);
			_onCancelAction = null;
		}
	}
}

#endif
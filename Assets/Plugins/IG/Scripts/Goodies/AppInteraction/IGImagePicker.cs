//
// DOCUMENTATION FOR THIS CLASS: https://github.com/TarasOsiris/iOS-Goodies-Docs/wiki/IGImagePicker.cs
//


#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Runtime.InteropServices;
	using AOT;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	/// <summary>
	///     Class allows you to pick images from gallery and camera and receive Texture2D in a callback
	/// </summary>
	[PublicAPI]
	public static class IGImagePicker
	{
		[MonoPInvokeCallback(typeof(ImageResultDelegate))]
		static void ImageResultCallback(IntPtr callbackPtr, IntPtr byteArrPtr, int arrayLength)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("Picked img ptr: " + byteArrPtr.ToInt32() + ", array length: " + arrayLength);
			}

			var buffer = new byte[arrayLength];

			if (Debug.isDebugBuild)
			{
				Debug.Log("ImageResultCallback");
			}

			Marshal.Copy(byteArrPtr, buffer, 0, arrayLength);
			var tex = new Texture2D(2, 2);
			tex.LoadImage(buffer);

			if (callbackPtr != IntPtr.Zero)
			{
				var action = callbackPtr.Cast<Action<Texture2D>>();
				action(tex);
			}
		}

		[PublicAPI]
		enum GallerySourceType
		{
			PhotoLibrary = 0,
			PhotosAlbum = 1
		}

		internal delegate void ImageResultDelegate(IntPtr callbackPtr, IntPtr byteArrPtr, int arrayLength);

		#region API

		/// <summary>
		///     Camera type to take picture.
		/// </summary>
		[PublicAPI]
		public enum CameraType
		{
			/// <summary>
			///     The rear camera.
			/// </summary>
			[PublicAPI]
			Rear = 0,
			
			/// <summary>
			///     The front camera.
			/// </summary>
			[PublicAPI]
			Front = 1
		}

		/// <summary>
		///     Camera flash mode.
		/// </summary>
		[PublicAPI]
		public enum CameraFlashMode
		{
			/// <summary>
			///     Flash off.
			/// </summary>
			[PublicAPI]
			Off = -1,

			/// <summary>
			///     Flash auto.
			/// </summary>
			[PublicAPI]
			Auto = 0,

			/// <summary>
			///     Flash on.
			/// </summary>
			[PublicAPI]
			On = 1
		}

		/// <summary>
		///     Picks the image from camera.
		/// </summary>
		/// <param name="callback">Image picked callback. Passes <see cref="Texture2D" /> as a parameter. Must not be null</param>
		/// <param name="onCancel">On cancel callback.</param>
		/// <param name="compressionQuality">Compression quality. Must be between 0 to 1</param>
		/// <param name="allowEditing">If set to <c>true</c> allow editing.</param>
		/// <param name="cameraType">Camera type. Front or Rear camera to use?</param>
		/// <param name="flashMode">Flash mode to take picture.</param>
		[PublicAPI]
		public static void PickImageFromCamera(Action<Texture2D> callback, Action onCancel,
			float compressionQuality = 1f,
			bool allowEditing = true,
			CameraType cameraType = CameraType.Front,
			CameraFlashMode flashMode = CameraFlashMode.Auto)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(callback, "callback");
			compressionQuality = Mathf.Clamp01(compressionQuality);

			_pickImageFromCamera(
				ImageResultCallback, callback.GetPointer(),
				IGUtils.ActionVoidCallback, onCancel.GetPointer(),
				compressionQuality, allowEditing, cameraType == CameraType.Rear,
				(int) flashMode);
		}

		/// <summary>
		///     Picks the image from photo library.
		/// </summary>
		/// <param name="callback">Image picked callback. Passes <see cref="Texture2D" /> as a parameter. Must not be null</param>
		/// <param name="onCancel">On cancel callback.</param>
		/// <param name="compressionQuality">Compression quality. Must be between 0 to 1</param>
		/// <param name="allowEditing">If set to <c>true</c> allow editing.</param>
		///  <param name="iPadScreenPosition">Position of the arrow on iPad screen. Positioning zero is at top left screen point. </param>
		[PublicAPI]
		public static void PickImageFromPhotoLibrary(Action<Texture2D> callback, Action onCancel,
			float compressionQuality = 1f,
			bool allowEditing = true,
			Vector2 iPadScreenPosition = default(Vector2))
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(callback, "callback");
			compressionQuality = Mathf.Clamp01(compressionQuality);

			var posX = IGUtils.ClampXScreenPos(iPadScreenPosition);
			var posY = IGUtils.ClampYScreenPos(iPadScreenPosition);

			_pickImageFromGallery(
				ImageResultCallback, callback.GetPointer(),
				IGUtils.ActionVoidCallback, onCancel.GetPointer(),
				(int) GallerySourceType.PhotoLibrary, compressionQuality, allowEditing,
				posX, posY);
		}

		/// <summary>
		///     Picks the image from photos album.
		/// </summary>
		/// <param name="callback">Image picked callback. Passes <see cref="Texture2D" /> as a parameter. Must not be null</param>
		/// <param name="onCancel">On cancel callback.</param>
		/// <param name="compressionQuality">Compression quality. Must be between 0 to 1</param>
		/// <param name="allowEditing">If set to <c>true</c> allow editing.</param>
		///  <param name="iPadScreenPosition">Position of the arrow on iPad screen. Positioning zero is at top left screen point. </param>
		[PublicAPI]
		public static void PickImageFromPhotosAlbum(Action<Texture2D> callback, Action onCancel,
			float compressionQuality = 1f,
			bool allowEditing = true,
			Vector2 iPadScreenPosition = default(Vector2))
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(callback, "callback");
			compressionQuality = Mathf.Clamp01(compressionQuality);

			var posX = IGUtils.ClampXScreenPos(iPadScreenPosition);
			var posY = IGUtils.ClampYScreenPos(iPadScreenPosition);

			_pickImageFromGallery(
				ImageResultCallback, callback.GetPointer(),
				IGUtils.ActionVoidCallback, onCancel.GetPointer(),
				(int) GallerySourceType.PhotosAlbum, compressionQuality, allowEditing,
				posX, posY);
		}

		/// <summary>
		///     Adds the specified image to the user’s Camera Roll album.
		/// </summary>
		/// <param name="texture2D">The image to write to the Camera Roll album.</param>
		[PublicAPI]
		public static void SaveImageToGallery(Texture2D texture2D)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(texture2D, "texture2D");

			var imageBuffer = texture2D.EncodeToPNG();
			var handle = GCHandle.Alloc(imageBuffer, GCHandleType.Pinned);
			_saveImageToGallery(handle.AddrOfPinnedObject(), imageBuffer.Length);
			handle.Free();
		}

		#endregion

		#region external

		[DllImport("__Internal")]
		static extern void _pickImageFromCamera(
			ImageResultDelegate callback, IntPtr callbackPtr,
			IGUtils.ActionVoidCallbackDelegate cancelCallback, IntPtr cancelPtr,
			float compressionQuality,
			bool allowEditing,
			bool isRearCamera,
			int flashMode);

		[DllImport("__Internal")]
		static extern void _pickImageFromGallery(
			ImageResultDelegate callback, IntPtr callbackPtr,
			IGUtils.ActionVoidCallbackDelegate cancelCallback, IntPtr cancelPtr,
			int source,
			float compressionQuality,
			bool allowEditing,
			int iPadX, int iPadY);

		[DllImport("__Internal")]
		static extern void _saveImageToGallery(IntPtr bufferPtr, int bufferLength);

		#endregion
	}
}
#endif
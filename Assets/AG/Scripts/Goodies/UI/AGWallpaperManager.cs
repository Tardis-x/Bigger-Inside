#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;
	using System.IO;

	public static class AGWallpaperManager
	{
		const string ACTION_LIVE_WALLPAPER_CHOOSER = "android.service.wallpaper.LIVE_WALLPAPER_CHOOSER";
		static AndroidJavaObject WallpaperManager
		{ 
			get
			{
				return C.AndroidAppWallpaperManager.AJCCallStaticOnceAJO("getInstance", AGUtils.Activity);
			}
		}

		/// <summary>
		/// Remove any currently set system wallpaper, reverting to the system's built-in wallpaper. On success, the intent ACTION_WALLPAPER_CHANGED is broadcast.
		/// </summary>
		public static void Clear()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			WallpaperManager.Call("clear");
		}

		/// <summary>
		/// Returns whether wallpapers are supported for the calling user.
		/// If this function returns <code>false</code>, any attempts to changing the wallpaper will have no effect,
		/// and any attempt to obtain of the wallpaper will return <code>null</code>.
		/// </summary>
		/// <returns>Whether wallpapers are supported for the calling user.</returns>
		public static bool IsWallpaperSupported()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.M)
			{
				return WallpaperManager.CallBool("isWallpaperSupported");
			}

			return true;
		}

		/// <summary>
		/// Returns whether the calling package is allowed to set the wallpaper for the calling user.
		/// If this function returns <code>false</code>, any attempts to change the wallpaper will have no effect. 
		/// Always returns true for device owner and profile owner.
		/// </summary>
		/// <returns>Whether the calling package is allowed to set the wallpaper.</returns>
		public static bool IsSetWallpaperAllowed()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return false;
			}

			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.N)
			{
				return WallpaperManager.CallBool("isSetWallpaperAllowed");
			}

			return true;
		}
		
		/// <summary>
		/// Sets provided texture as wallpaper
		/// </summary>
		/// <param name="wallpaperTexture">Image to set as wallpeper.</param>
		public static void SetWallpaper([NotNull] Texture2D wallpaperTexture)
		{
			if (wallpaperTexture == null)
			{
				throw new ArgumentNullException("wallpaperTexture");
			}
			
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			var wallpaperPath = AndroidPersistanceUtilsInternal.SaveWallpaperImageToExternalStorage(wallpaperTexture);
			SetWallpaperFromFilePath(wallpaperPath);
		}

		/// <summary>
		/// Sets provided image on the provided filepath as wallpaper. File MUST exist.
		/// </summary>
		/// <param name="imagePath">File path to the image file</param>
		public static void SetWallpaper([NotNull] string imagePath)
		{
			if (imagePath == null)
			{
				throw new ArgumentNullException("imagePath");
			}

			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			CheckIfFileExists(imagePath);
			SetWallpaperFromFilePath(imagePath);
		}

		static void SetWallpaperFromFilePath(string wallpaperPath)
		{
			var bitmapAjo = AGUtils.DecodeBitmap(wallpaperPath);
			WallpaperManager.Call("setBitmap", bitmapAjo);
		}

		/// <summary>
		/// <remarks>
		/// WARNING: This method work on my devices but always crashes on emulators and I can't find a way to fix it.
		/// It may be something with emulator but use this method with care and test careffuly before using it.
		/// </remarks>
		/// 
		/// Sets provided texture as wallpaper allowing user to crop beforehand
		/// </summary>
		/// <param name="wallpaperTexture">Image to set as wallpeper.</param>
		public static void ShowCropAndSetWallpaperChooser([NotNull] Texture2D wallpaperTexture)
		{
			if (wallpaperTexture == null)
			{
				throw new ArgumentNullException("wallpaperTexture");
			}

			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (AGDeviceInfo.SDK_INT < AGDeviceInfo.VersionCodes.KITKAT)
			{
				return;
			}

			var uri = AndroidPersistanceUtilsInternal.SaveWallpaperImageToExternalStorageUri(wallpaperTexture);
			StartCropAndSetWallpaperActivity(uri);
		}

		/// <summary>
		/// 
		/// <remarks>
		/// WARNING: This method work on my devices but always crashes on emulators and I can't find a way to fix it.
		/// It may be something with emulator but use this method with care and test careffuly before using it.
		/// </remarks>
		/// 
		/// Sets provided image on the provided filepath as wallpaper allowing user to crop beforehand. File MUST exist.
		/// </summary>
		/// <param name="imagePath">File path to the image file</param>
		public static void ShowCropAndSetWallpaperChooser([NotNull] string imagePath)
		{
			if (imagePath == null)
			{
				throw new ArgumentNullException("imagePath");
			}

			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (AGDeviceInfo.SDK_INT < AGDeviceInfo.VersionCodes.KITKAT)
			{
				return;
			}

			CheckIfFileExists(imagePath);
			var uri = AndroidPersistanceUtilsInternal.GetUriFromFilePath(imagePath);
			StartCropAndSetWallpaperActivity(uri);
		}

		static void StartCropAndSetWallpaperActivity(AndroidJavaObject uri)
		{
			var intent = WallpaperManager.CallAJO("getCropAndSetWallpaperIntent", uri);
			AGUtils.StartActivity(intent);
		}

		/// <summary>
		/// Launch an activity for the user to pick the current global live wallpaper.
		/// </summary>
		public static void ShowLiveWallpaperChooser()
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}
			
			var intent = new AndroidIntent(ACTION_LIVE_WALLPAPER_CHOOSER);
			AGUtils.StartActivity(intent.AJO);
		}

		static void CheckIfFileExists(string imagePath)
		{
			if (!File.Exists(imagePath))
			{
				Debug.LogError("File doesn't exist: " + imagePath);
			}
		}
	}
}
#endif
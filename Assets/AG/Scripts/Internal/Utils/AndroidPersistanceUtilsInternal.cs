
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using System.IO;
	using UnityEngine;

	public static class AndroidPersistanceUtilsInternal
	{
		const string FileProviderClass = "android.support.v4.content.FileProvider";

		const string GoodiesFileFolder = "android-goodies";
		const string GoodiesShareImageFileName = "android-goodies-share-image.png";
		const string GoodiesWallpaperImageFileName = "android-goodies-wallpaper-image.png";

		public static AndroidJavaObject SaveShareImageToExternalStorage(Texture2D tex2D)
		{
			var saveFilePath = SaveImageToPictures(tex2D, GoodiesShareImageFileName, GoodiesFileFolder);
			return GetUriFromFilePath(saveFilePath);
		}

		public static string SaveWallpaperImageToExternalStorage(Texture2D tex2D)
		{
			return SaveImageToPictures(tex2D, GoodiesWallpaperImageFileName, GoodiesFileFolder);
		}

		public static AndroidJavaObject SaveWallpaperImageToExternalStorageUri(Texture2D tex2D)
		{
			var saveFilePath = SaveImageToPictures(tex2D, GoodiesWallpaperImageFileName, GoodiesFileFolder);
			return GetUriFromFilePath(saveFilePath);
		}

		public static AndroidJavaObject GetUriFromFilePath(string saveFilePath)
		{
			AndroidJavaObject uri;
			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.N)
			{
				// Reference: http://stackoverflow.com/questions/38200282/android-os-fileuriexposedexception-file-storage-emulated-0-test-txt-exposed
				using (var c = new AndroidJavaClass(FileProviderClass))
				{
					var provider = AGDeviceInfo.GetApplicationPackage() + ".multipicker.fileprovider";
					uri = c.CallStaticAJO("getUriForFile", AGUtils.Activity, provider, AGUtils.NewJavaFile(saveFilePath));
				}
			}
			else
			{
				uri = AndroidUri.FromFile(saveFilePath);
			}
			return uri;
		}

		public static string SaveImageToPictures(Texture2D tex2D, string fileName, string directory = null,
			ImageFormat format = ImageFormat.PNG)
		{
			byte[] encoded = tex2D.Encode(format);
			fileName += format == ImageFormat.PNG ? ".png" : ".jpeg";

			var picsDirectory = string.IsNullOrEmpty(directory)
				? AGEnvironment.DirectoryPictures
				: Path.Combine(AGEnvironment.DirectoryPictures, directory);

			var savedFilePath = SaveFileToExternalStorage(encoded, fileName, picsDirectory);
			RefreshGallery(savedFilePath);
			return savedFilePath;
		}

		public static string SaveFileToExternalStorage(byte[] buffer, string fileName, string directory = null)
		{
			var pathToSave = AGEnvironment.ExternalStorageDirectoryPath;
			if (!string.IsNullOrEmpty(directory))
			{
				pathToSave = Path.Combine(pathToSave, directory);
				Directory.CreateDirectory(pathToSave);
			}

			var filePath = Path.Combine(pathToSave, fileName);

			try
			{
				var file = File.Open(filePath, FileMode.OpenOrCreate);
				var binary = new BinaryWriter(file);
				binary.Write(buffer);
				file.Close();
			}
			catch (Exception e)
			{
				Debug.LogError("Android Goodies failed to save file " + fileName + " to external storage");
				Debug.LogException(e);
			}

			return filePath;
		}

		public static void RefreshGallery(string filePath)
		{
			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.KITKAT)
			{
				ScanFile(filePath, null);
			}
			else
			{
				var uri = AndroidUri.FromFile(filePath);
				var intent = new AndroidIntent(AndroidIntent.ACTION_MEDIA_MOUNTED, uri);
				AGUtils.SendBroadcast(intent.AJO);
			}
		}

		public static string InsertImage(Texture2D texture2D, string title, string description)
		{
			using (var mediaClass = new AndroidJavaClass(C.AndroidProviderMediaStoreImagesMedia))
			{
				using (var cr = AGUtils.ContentResolver)
				{
					var image = AGUtils.Texture2DToAndroidBitmap(texture2D);
					var imageUrl = mediaClass.CallStaticStr("insertImage", cr, image, title, description);
					return imageUrl;
				}
			}
		}

		public static void ScanFile(string filePath, Action<string, AndroidJavaObject> onScanCompleted)
		{
			var listener = onScanCompleted == null ? null : new OnScanCompletedListener(onScanCompleted);
			using (var c = new AndroidJavaClass(C.AndroidMediaMediaScannerConnection))
			{
				c.CallStatic("scanFile", AGUtils.Activity, new[] {filePath}, null, listener);
			}
		}
	}
}

#endif
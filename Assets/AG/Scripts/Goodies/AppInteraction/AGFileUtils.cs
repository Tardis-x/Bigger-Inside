// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGFileUtils.cs
//


#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	public static class AGFileUtils
	{
		/// <summary>
		/// Returns absolute path to application-specific directory on the primary shared/external storage device where the application can place cache files it owns.
		/// </summary>
		[PublicAPI]
		public static string ExternalCacheDirectory
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return null;
				}
				
				return AGUtils.ExternalCacheDirectory.GetAbsolutePath();
			}
		}
		
		/// <summary>
		/// Returns the absolute path to the application specific cache directory on the filesystem.
		/// </summary>
		[PublicAPI]
		public static string CacheDirectory
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return null;
				}
				
				return AGUtils.CacheDirectory.GetAbsolutePath();
			}
		}
		
		/// <summary>
		/// Returns the absolute path to the application specific cache directory on the filesystem designed for storing cached code.
		/// </summary>
		[PublicAPI]
		public static string CodeCacheDirectory
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return null;
				}
				
				return AGUtils.CodeCacheDirectory.GetAbsolutePath();
			}
		}
		
		/// <summary>
		/// Returns the absolute path to the directory on the filesystem where all private files belonging to this app are stored.
		/// </summary>
		[PublicAPI]
		public static string DataDir
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return null;
				}
				
				return AGUtils.DataDir.GetAbsolutePath();
			}
		}
		
		/// <summary>
		/// Return the primary shared/external storage directory where this application's OBB files (if there are any) can be found.
		/// </summary>
		[PublicAPI]
		public static string ObbDir
		{
			get
			{
				if (AGUtils.IsNotAndroidCheck())
				{
					return null;
				}
				
				return AGUtils.ObbDir.GetAbsolutePath();
			}
		}
		
		/// <summary>
		/// Saves the image to android gallery.
		/// </summary>
		/// <returns>The image to save to the gallery.</returns>
		/// <param name="texture2D">Texture2D to save.</param>
		/// <param name="title">Title.</param>
		/// <param name="folder">Inner folder in Pictures directory. Must be a valid folder name.
		/// If not specified the image will be save to default pictures folder</param>
		/// <param name="imageFormat">Image format.</param>
		/// <returns>Path to the saved file</returns>
		[PublicAPI]
		public static string SaveImageToGallery([NotNull] Texture2D texture2D, string title, string folder = null,
			ImageFormat imageFormat = ImageFormat.PNG)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}

			if (texture2D == null)
			{
				throw new ArgumentNullException("texture2D", "Image to save cannot be null");
			}

			return AndroidPersistanceUtilsInternal.SaveImageToPictures(texture2D, title, folder, imageFormat);
		}

		/// <summary>
		/// Loads image by URI to Texture2D
		/// </summary>
		/// <returns>Loaded image as Texture2D.</returns>
		/// <param name="imageUri">Android Image URI.</param>
		[PublicAPI]
		public static Texture2D ImageUriToTexture2D(string imageUri)
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return null;
			}

			Check.Argument.IsStrNotNullOrEmpty(imageUri, "imageUri");

			return AGUtils.TextureFromUriInternal(imageUri);
		}
	}
}

#endif
// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGShare.cs
//


#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.IO;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	public static class AGShare
	{
		[PublicAPI]
		public static void ShareTextWithImage(string subject, string body, Texture2D image, bool withChooser = true,
			string chooserTitle = "Share via...")
		{
			if (AGUtils.IsNotAndroidCheck())
			{
				return;
			}

			if (image == null)
			{
				throw new ArgumentNullException("image", "Image must not be null");
			}

			var intent = new AndroidIntent()
				.SetAction(AndroidIntent.ACTION_SEND)
				.SetType(AndroidIntent.MIMETypeImageJpeg)
				.PutExtra(AndroidIntent.EXTRA_SUBJECT, subject)
				.PutExtra(AndroidIntent.EXTRA_TEXT, body);

			var imageUri = AndroidPersistanceUtilsInternal.SaveShareImageToExternalStorage(image);
			intent.PutExtra(AndroidIntent.EXTRA_STREAM, imageUri);

			if (withChooser)
			{
				AGUtils.StartActivityWithChooser(intent.AJO, chooserTitle);
			}
			else
			{
				AGUtils.StartActivity(intent.AJO);
			}
		}
	}
}

#endif
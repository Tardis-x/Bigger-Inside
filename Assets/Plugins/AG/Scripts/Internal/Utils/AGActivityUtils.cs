
#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies.Internal
{
	using System;
	using UnityEngine;

	public static class AGActivityUtils
	{
		public const string JarPackageName = "com.deadmosquitogames.";
		public const string AndroidGoodiesActivityClassSignature = JarPackageName + "AndroidGoodiesActivity";
		public const string PhotoPickerUtilsClassSignature = JarPackageName + "PhotoPickerUtils";

		#region intent_extras

		const string EXTRAS_PERMISSIONS = "EXTRAS_PERMISSIONS";
		const string EXTRAS_PICKER_TYPE = "EXTRAS_PICKER_TYPE";
		const string EXTRAS_MAX_SIZE = "EXTRAS_MAX_SIZE";
		const string EXTRAS_GENERATE_THUMBNAILS = "EXTRAS_GENERATE_THUMBNAILS";
		const string EXTRAS_GENERATE_PREVIEW_IMAGES = "EXTRAS_GENERATE_PREVIEW_IMAGES";
		const string EXTRAS_MIME_TYPE = "EXTRAS_MIME_TYPE";

		#endregion

		const int REQ_CODE_PICK_IMAGE = 3111;
		const int REQ_CODE_TAKE_PHOTO = 4222;
		const int REQ_CODE_PICK_CONTACT = 8666;
		const int REQ_CODE_PICK_AUDIO = 9777;
		const int REQ_CODE_PICK_VIDEO_DEVICE = 5333;
		const int REQ_CODE_PICK_VIDEO_CAMERA = 6444;
		const int REQ_CODE_PICK_FILE = 7555;
		const int REQ_CODE_PERMISSIONS = 444;

		public static AndroidJavaClass AndroidGoodiesActivityClass
		{
			get
			{
				if (_androidGoodiesActivityClass == null)
				{
					_androidGoodiesActivityClass = new AndroidJavaClass(AndroidGoodiesActivityClassSignature);
				}

				return _androidGoodiesActivityClass;
			}
		}

		static AndroidJavaClass _androidGoodiesActivityClass;

		public static void PickContact()
		{
			StartAndroidGoodiesActivity(REQ_CODE_PICK_CONTACT,
				intent => { });
		}

		public static void PickPhotoFromGallery(ImageResultSize maxSize, bool shouldGenerateThumbnails)
		{
			PickImage(REQ_CODE_PICK_IMAGE, maxSize, shouldGenerateThumbnails);
		}

		public static void TakePhoto(ImageResultSize maxSize, bool shouldGenerateThumbnails)
		{
			PickImage(REQ_CODE_TAKE_PHOTO, maxSize, shouldGenerateThumbnails);
		}

		static void PickImage(int requestCode, ImageResultSize maxSize, bool shouldGenerateThumbnails)
		{
			StartAndroidGoodiesActivity(requestCode,
				intent =>
				{
					intent.PutExtra(EXTRAS_MAX_SIZE, (int) maxSize);
					intent.PutExtra(EXTRAS_GENERATE_THUMBNAILS, shouldGenerateThumbnails);
				});
		}

		public static void PickAudio()
		{
			StartAndroidGoodiesActivity(REQ_CODE_PICK_AUDIO,
				_ => { });
		}

		public static void PickVideoDevice(bool generatePreviewImages)
		{
			StartAndroidGoodiesActivity(REQ_CODE_PICK_VIDEO_DEVICE,
				intent => intent.PutExtra(EXTRAS_GENERATE_PREVIEW_IMAGES, generatePreviewImages));
		}

		public static void PickVideoCamera(bool generatePreviewImages)
		{
			StartAndroidGoodiesActivity(REQ_CODE_PICK_VIDEO_CAMERA,
				intent => intent.PutExtra(EXTRAS_GENERATE_PREVIEW_IMAGES, generatePreviewImages));
		}

		public static void PickFile(string mimeType)
		{
			StartAndroidGoodiesActivity(REQ_CODE_PICK_FILE,
				intent => intent.PutExtra(EXTRAS_MIME_TYPE, mimeType));
		}

		public static void RequestPermissions(string[] permissions)
		{
			StartAndroidGoodiesActivity(REQ_CODE_PERMISSIONS,
				intent => intent.PutExtra(EXTRAS_PERMISSIONS, permissions));
		}

		public static void StartAndroidGoodiesActivity(int pickerType, Action<AndroidIntent> configurator)
		{
			using (var clazz = AGUtils.ClassForName(AndroidGoodiesActivityClassSignature))
			{
				using (var intent = new AndroidIntent(AGUtils.Activity, clazz))
				{
					intent.PutExtra(EXTRAS_PICKER_TYPE, pickerType);
					configurator(intent);
					AGUtils.StartActivity(intent.AJO);
				}
			}
		}
	}
}

#endif
namespace DeadMosquito.IosGoodies.Example
{
	using JetBrains.Annotations;
	using UnityEngine;
	using UnityEngine.UI;

	public class IGOpenAppsExample : MonoBehaviour
	{
		public Image image;
		public Texture2D testImage;

#if UNITY_IOS
		[UsedImplicitly]
		public void OpenAppStoreApp()
		{
			const string facebookAppId = "284882215";
			IGApps.OpenAppOnAppStore(facebookAppId);
		}

		[UsedImplicitly]
		public void OnOpenYouTubeVideo()
		{
			const string videoId = "rZ2csdtP440";
			IGApps.OpenYoutubeVideo(videoId);
		}

		[UsedImplicitly]
		public void OnFaceTimeVideoCall()
		{
			IGApps.StartFaceTimeVideoCall("user@example.com");
		}

		[UsedImplicitly]
		public void OnFaceTimeAudioCall()
		{
			IGApps.StartFaceTimeAudioCall("user@example.com");
		}

		[UsedImplicitly]
		public void OnOpenDialer()
		{
			IGApps.OpenDialer("123456789");
		}

		[UsedImplicitly]
		public void OnOpenAppSettings()
		{
			IGApps.OpenAppSettings();
		}

		#region image_pickers

		[UsedImplicitly]
		public void PickImageFromCamera()
		{
			const bool allowEditing = true;
			const float compressionQuality = 0.8f;
			const IGImagePicker.CameraType cameraType = IGImagePicker.CameraType.Front;
			const IGImagePicker.CameraFlashMode flashMode = IGImagePicker.CameraFlashMode.On;

			IGImagePicker.PickImageFromCamera(tex =>
				{
					Debug.Log("Successfully picked image from camera");
					image.sprite = SpriteFromTex2D(tex);
					// IMPORTANT! Call this method to clean memory if you are picking and discarding images
					Resources.UnloadUnusedAssets();
				},
				() => Debug.Log("Picking image from camera cancelled"),
				compressionQuality,
				allowEditing, cameraType, flashMode);
		}

		[UsedImplicitly]
		public void PickImageFromPhotoLibrary()
		{
			const bool allowEditing = false;
			const float compressionQuality = 0.5f;
			var screenPosition = new Vector2(Screen.width, Screen.height); // On iPads ONLY you can choose screen position of popover

			IGImagePicker.PickImageFromPhotoLibrary(tex =>
				{
					Debug.Log("Successfully picked image from photo library");
					image.sprite = SpriteFromTex2D(tex);
					// IMPORTANT! Call this method to clean memory if you are picking and discarding images
					Resources.UnloadUnusedAssets();
				},
				() => Debug.Log("Picking image from photo library cancelled"),
				compressionQuality,
				allowEditing, screenPosition);
		}

		[UsedImplicitly]
		public void PickImageFromPhotosAlbum()
		{
			const bool allowEditing = true;
			const float compressionQuality = 0.1f;
			var screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

			IGImagePicker.PickImageFromPhotosAlbum(tex =>
				{
					Debug.Log("Successfully picked image from photos album");
					image.sprite = SpriteFromTex2D(tex);
					// IMPORTANT! Call this method to clean memory if you are picking and discarding images
					Resources.UnloadUnusedAssets();
				},
				() => Debug.Log("Picking image from photos album cancelled"),
				compressionQuality,
				allowEditing, screenPosition);
		}

		[UsedImplicitly]
		public void SaveImageToPhotoLibrary()
		{
			IGImagePicker.SaveImageToGallery(testImage);
		}

		[UsedImplicitly]
		public void PickContact()
		{
			IGContactPicker.PickContact(Debug.Log, () => { Debug.Log("Picking contact was cancelled");});
		}

		#endregion

		static Sprite SpriteFromTex2D(Texture2D texture)
		{
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		}
#endif
	}
}
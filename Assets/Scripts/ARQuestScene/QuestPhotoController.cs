using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeadMosquito.AndroidGoodies;
using UnityEngine.UI;

public class QuestPhotoController : MonoBehaviour 
{
	public Image photoTextureHolder;
	public void OnTakePictureButtonClick()
	{
		Debug.Log("Take a picture button Clicked.");
		if (AGCamera.DeviceHasCamera())
		{
			Debug.Log("Phone has camera.");
			var imageResultSize = ImageResultSize.Max2048; 
			AGCamera.TakePhoto(
				selectedImage =>
				{
					Debug.Log("Taking Photo.");
					// Load received image into Texture2D
					var imageTexture2D = selectedImage.LoadTexture2D();
					string msg = string.Format("{0} was taken from camera with size {1}x{2}",
						selectedImage.DisplayName, imageTexture2D.width, imageTexture2D.height);
					AGUIMisc.ShowToast(msg);
					photoTextureHolder.sprite = SpriteFromTex2D(imageTexture2D);
					Debug.Log("Image taken.");
					var path = AGFileUtils.SaveImageToGallery(imageTexture2D, selectedImage.DisplayName, "ARQuest",
						ImageFormat.JPEG);
					AGGallery.RefreshFile(path);
					// Clean up
					Resources.UnloadUnusedAssets();
				},
				error => AGUIMisc.ShowToast("Cancelled taking photo from camera: " + error), imageResultSize, false);
		}
	}

	public void OnSharePictureButtonClick()
	{
		Debug.Log("Share Button Clicked.");
		Texture2D tex = photoTextureHolder.mainTexture as Texture2D;
		string subject = "DevFest Photo";
		string body = "My photo with speaker!";
		AGShare.ShareTextWithImage(subject, body, tex, true, "Share via facebook..." );
	}
	Sprite SpriteFromTex2D(Texture2D texture)
	{
		return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
	}
}

using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using DeadMosquito.AndroidGoodies;
using DeadMosquito.AndroidGoodies.Internal;
using Firebase.Auth;
using Firebase.Storage;
using UnityEngine.UI;

public class QuestPhotoController : MonoBehaviour 
{
	public Image photoTextureHolder;
	private Texture2D tex;
	private string userID;
	private FirebaseStorage storage;
	private string picturePath;
	private int imageSize = 0;
	void OnTakePictureButtonClick()
	{
		Debug.Log("Take a picture button Clicked.");
		if (AGCamera.DeviceHasCamera())
		{
			Debug.Log("Phone has camera.");
			AGCamera.TakePhoto(selectedImage =>
				{
					Debug.Log("Taking Photo.");
					// Load received image into Texture2D
					var imageTexture2D = selectedImage.LoadTexture2D();
					string msg = string.Format("{0} was taken from camera with size {1}x{2}",
						selectedImage.DisplayName, imageTexture2D.width, imageTexture2D.height);
					AGUIMisc.ShowToast(msg);
					picturePath = AndroidUri.FromFile(selectedImage.OriginalPath).JavaToString();
					photoTextureHolder.sprite = SpriteFromTex2D(imageTexture2D);
					//imageSize = selectedImage.Size;
					AGUIMisc.ShowToast("Photo is ready for sharing and uploading.");
					StartCoroutine(ShowSpinnerForPhoto());
//					string galleryPath = AGFileUtils.SaveImageToGallery(imageTexture2D, selectedImage.DisplayName, "ARQuest", ImageFormat.JPEG);
//					AGGallery.RefreshFile(galleryPath);
					// Clean up
					Resources.UnloadUnusedAssets();
				},
				error => AGUIMisc.ShowToast("Cancelled taking photo from camera: " + error), ImageResultSize.Max1024, false);
		}
	}
	public void OnSharePictureButtonClick()
	{
		Debug.Log("Share Button Clicked.");
		tex = photoTextureHolder.mainTexture as Texture2D;
		string subject = "DevFest Photo";
		string body = "My photo with speaker!";
		AGShare.ShareTextWithImage(subject, body, tex, true, "Share via facebook..." );
	}
	public void OnUploadButtonClick()
	{
		userID = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
		userID = userID.Replace(" ", string.Empty);
		//Creating reference to a picture in the database
		storage = FirebaseStorage.DefaultInstance;
		StorageReference storageRef = storage.GetReferenceFromUrl("gs://hoverboard-v2-dev.appspot.com");
		string pictureNameInStorage = "/Pictures/" + userID + ".jpeg";
		StorageReference pictureReference = storageRef.Child(pictureNameInStorage);
		//Uploading image
		var task = pictureReference.PutFileAsync(picturePath);
		//Is called periodically during the upload
//			Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.",
//				state.BytesTransferred, state.TotalByteCount));
//			AGUIMisc.ShowToast(String.Format("Progress: {0} of {1} bytes transferred.",
//				state.BytesTransferred, state.TotalByteCount));
		var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait...", "Uploading...");
		spinner.Show();
		task.ContinueWith(resultTask =>
		{
			spinner.Dismiss();
//			progressBar.Dismiss();
			if (!resultTask.IsFaulted && !resultTask.IsCanceled) 
			{
				Debug.Log("Upload finished.");
				AGUIMisc.ShowToast("Upload finished.");
			}
			else
			{
				AGUIMisc.ShowToast(resultTask.Exception.Message);
			}
		});
	}
	private IEnumerator ShowSpinnerForPhoto()
	{
		// Create spinner dialog
		var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait...", "Saving photo...");
		spinner.Show();
		// Spin for some time (do important work)
		yield return new WaitForSeconds(2);
		// Dismiss spinner after all the background work is done
		spinner.Dismiss();
	}
	Sprite SpriteFromTex2D(Texture2D texture)
	{
		return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
	}
}

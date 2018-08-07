using System.Collections;
using System.IO;
using UnityEngine;
using DeadMosquito.AndroidGoodies;
using DeadMosquito.AndroidGoodies.Internal;
using DeadMosquito.IosGoodies;
using Firebase.Auth;
using Firebase.Storage;
using ua.org.gdg.devfest;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class QuestPhotoController : MonoBehaviour
{
	public Image photoTextureHolder;
	Texture2D tex;
	string userID;
	FirebaseStorage storage;
	public string picturePath;
	StorageReference pictureReference;
	QuestManager _questManager;
	public string imageUrl;
	byte[] imageBytes;
	string pictureNameInStorage;
	SigninSampleScript signIn;
	public Text _cameraText;
	string _photoComment;
	[SerializeField] Camera _mainCamera;
	[SerializeField] Camera _arCamera;
	[SerializeField] Text _scanStatusText;

	void Awake()
	{
		Debug.Log("QuestPhotoController.Awake");
		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();
		_photoComment = "";
		userID = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
		userID = userID.Replace(" ", string.Empty);
		//Creating reference to a picture in the database
		storage = FirebaseStorage.DefaultInstance;
	}

	void QuestManagerReferenceInitialization()
	{
		GameObject questManagerTemp = GameObject.Find("QuestManager");

		if (questManagerTemp != null)
		{
			_questManager = questManagerTemp.GetComponent<QuestManager>();

			if (_questManager == null)
			{
				Debug.LogError("Could not locate QuestManager component on " + questManagerTemp.name);
			}
		}
		else
		{
			Debug.LogError("Could not locate quest manager object in current scene!");
		}
	}

	public void OnPhotoActionButtonClick(string s)
	{
		_photoComment = s;
#if UNITY_ANDROID
		string[] photoActions = {"Take a photo", "Pick from Gallery"};
		AGAlertDialog.ShowChooserDialog("Pick an action", photoActions,
			colorIndex =>
			{
				switch (colorIndex)
				{
					case 0:
					{
						OnAndroidTakePictureButtonClick();
						break;
					}
					case 1:
					{
						OnAndroidPickPictureButtonClick();
						break;
					}
				}
			}, AGDialogTheme.Dark);

#elif UNITY_IOS
    string[] actionSheetOptions = {"Take a photo", "Pick from Library"};
    
        IGActionSheet.ShowActionSheet("Pick an action:", "Cancel", () =>
            {
                _cameraText.text = "Operation was canceled.";
                StartCoroutine(TextClearing());
            }, 
            actionSheetOptions, index =>
            {
                switch (index)
                {
                    case 0:
                    {
                        OnIosTakePhotoButtonClick();
                        break;
                    }
                    case 1:
                    {
                        OnIosPickPhotoButtonClick();
                        break;
                    }
                }
            });
#endif
	}
#if UNITY_ANDROID
	void OnAndroidTakePictureButtonClick()
	{
		var imageResultSize = ImageResultSize.Max1024;
		AGCamera.TakePhoto(OnAndroidImagePickSuccess,
			error => AGUIMisc.ShowToast("Cancelled taking photo from camera: " + error), imageResultSize, false);
	}

	void OnAndroidPickPictureButtonClick()
	{
		var imageResultSize = ImageResultSize.Max512;
		AGGallery.PickImageFromGallery(OnAndroidImagePickSuccess,
			errorMessage => AGUIMisc.ShowToast("Cancelled picking image from gallery: " + errorMessage),
			imageResultSize, false);
	}

	void OnAndroidImagePickSuccess(ImagePickResult selectedImage)
	{
		// Load received image into Texture2D
		var imageTexture2D = selectedImage.LoadTexture2D();
//                    string msg = string.Format("{0} was taken from camera with size {1}x{2}",
//                        selectedImage.DisplayName, imageTexture2D.width, imageTexture2D.height);
//                    AGUIMisc.ShowToast(msg);
		picturePath = AndroidUri.FromFile(selectedImage.OriginalPath).JavaToString();
		photoTextureHolder.sprite = SpriteFromTex2D(imageTexture2D);
		string galleryPath =
			AGFileUtils.SaveImageToGallery(imageTexture2D, selectedImage.DisplayName, "ARQuest", ImageFormat.JPEG);
		AGGallery.RefreshFile(galleryPath);
		OnAndroidUploadButtonClick();
		// Clean up
		Resources.UnloadUnusedAssets();
	}
#elif UNITY_IOS
    void OnIosTexturePickSuccess(Texture2D texture)
    {
        Debug.Log("Successfully picked image from camera");
        tex = texture;
        Debug.Log("Texture saved.");
        photoTextureHolder.sprite = SpriteFromTex2D(texture);
        Debug.Log("Texture placed on image holder.");
//                IGImagePicker.SaveImageToGallery(texture);
//                Debug.Log("Image saved to gallery.");
        //Creating local file from texture
        picturePath = Application.persistentDataPath + "/photo.jpeg";
        if (File.Exists(picturePath))
        {
            File.Delete(picturePath);
            Debug.Log("File was deleted.");
        }
        File.WriteAllBytes(picturePath, texture.EncodeToJPG());
        Debug.Log("File was created.");
        if (File.Exists(picturePath))
        {
            Debug.Log("It is there, man...: " + picturePath);
            //Upload
            OnIosUploadButtonClick();
            _cameraText.text = "Picture was picked and uploaded successfully.";
            StartCoroutine(TextClearing());
        }
    }

    void OnIosTakePhotoButtonClick()
    {
        Debug.Log("Take a picture button Clicked.");
        const bool allowEditing = false;
        const float compressionQuality = 0.1f;
        const IGImagePicker.CameraType cameraType = IGImagePicker.CameraType.Rear;
        const IGImagePicker.CameraFlashMode flashMode = IGImagePicker.CameraFlashMode.Off;
        IGImagePicker.PickImageFromCamera(OnIosTexturePickSuccess, 
            () =>
            {
                _cameraText.text = "Picking image from camera was canceled";
                StartCoroutine(TextClearing());
            }, 
            compressionQuality,
            allowEditing, cameraType, flashMode);
    }

    private void OnIosPickPhotoButtonClick()
    {
        const bool allowEditing = false;
        const float compressionQuality = 0.1f;
        var screenPosition =
 new Vector2(Screen.width / 2f, Screen.height / 2f); // On iPads ONLY you can choose screen position of popover
        IGImagePicker.PickImageFromPhotosAlbum(OnIosTexturePickSuccess,
            () =>
            {
                _cameraText.text = "Picking image from photos album was canceled";
                StartCoroutine(TextClearing());
            },
            compressionQuality,
            allowEditing, screenPosition);
    }
#endif
	public void OnSharePictureButtonClick()
	{
		Debug.Log("Share Button Clicked.");
#if UNITY_ANDROID
		tex = photoTextureHolder.mainTexture as Texture2D;
		string subject = "DevFest Photo";
		string body = "My photo with speaker!";
		AGShare.ShareTextWithImage(subject, body, tex);
#elif UNITY_IOS
        var screenPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

        IGShare.Share(
            activityType =>
            {
                if (string.IsNullOrEmpty(activityType))
                {
                    Debug.Log("Posting was canceled or unknown result");
                }
                else
                {
                    Debug.Log("DONE sharing, activity: " + activityType);
                }
            },
            error => Debug.LogError("Error happened when sharing activity: " + error),
            "Google DevFest!", tex, screenPosition);
#endif
	}


#if UNITY_ANDROID
	void OnAndroidUploadButtonClick()
	{
		StorageReference storageRef = storage.GetReferenceFromUrl("gs://hoverboard-v2-dev.appspot.com");
		pictureNameInStorage = "/Pictures/" + userID + _photoComment + ".jpeg";
		pictureReference = storageRef.Child(pictureNameInStorage);
		//Uploading image
		var task = pictureReference.PutFileAsync(picturePath);
		var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait...", "Uploading...", AGDialogTheme.Dark);
		spinner.Show();
		task.ContinueWith(resultTask =>
		{
			spinner.Dismiss();
			//Saving picture progress for quest
			imageUrl = "gs://hoverboard-v2-dev.appspot.com" + pictureNameInStorage;
			_questManager.questProgress.photoData.imgUrl = imageUrl;
			_questManager.CheckInPhoto(this);
			if (!resultTask.IsFaulted && !resultTask.IsCanceled)
			{
				Debug.Log("Upload finished.");
				AGUIMisc.ShowToast("Picture was successfully taken and uploaded.");
			}
			else
			{
				AGUIMisc.ShowToast(resultTask.Exception.Message);
			}
		});
	}
#elif UNITY_IOS
    void OnIosUploadButtonClick()
    {
        picturePath = "file://" + picturePath;
        userID = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        userID = userID.Replace(" ", string.Empty);
        //Creating reference to a picture in the database
        storage = FirebaseStorage.DefaultInstance;
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hoverboard-v2-dev.appspot.com");
        pictureNameInStorage = "/Pictures/" + userID + _photoComment + ".jpeg";
        pictureReference = storageRef.Child(pictureNameInStorage);
        //Uploading image
        var pictureMetadata = new MetadataChange();
        pictureMetadata.ContentType = "image/jpeg";
        //Saving picture progress for quest
        imageUrl = "gs://hoverboard-v2-dev.appspot.com" + pictureNameInStorage;
        _questManager.questProgress.photoData.imgUrl = imageUrl;
        _questManager.CheckInPhoto(this);
        var task = pictureReference.PutFileAsync(picturePath, pictureMetadata);
        task.ContinueWith(resultTask =>
        {
            if (!resultTask.IsFaulted && !resultTask.IsCanceled)
            {
                Debug.Log("Upload finished.");
            }
            else
            {
                Debug.Log(resultTask.Exception.Message);
            }
        });
    }
#endif

	public void OnScanButtonClicked()
	{
		Debug.Log("QuestPhotoController.OnScanButtonClicked");

		_mainCamera.gameObject.SetActive(false);

		_arCamera.gameObject.SetActive(true);
	}

	public void OnImageScanned(string scannedMarker)
	{
		Debug.Log("QuestPhotoController.OnImageScanned");

		if (!_questManager.questProgress.photoData.state)
		{
			if (scannedMarker == "Photo")
			{
				_scanStatusText.color = Color.green;
				_scanStatusText.text = "Congratulations! Step completed!\nLoading next step...";
				StartCoroutine(CameraSwitchDelay());
			}
		}
	}

	IEnumerator CameraSwitchDelay()
	{
		yield return new WaitForSeconds(3);
		_scanStatusText.text = "";
		_mainCamera.gameObject.SetActive(true);
		_arCamera.gameObject.SetActive(false);
		_questManager.CompletePhoto();
	}

	IEnumerator TextClearing()
	{
		yield return new WaitForSeconds(3);
		_cameraText.text = string.Empty;
	}

	Sprite SpriteFromTex2D(Texture2D texture)
	{
		return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
	}
}
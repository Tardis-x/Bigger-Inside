using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using DeadMosquito.AndroidGoodies;
using DeadMosquito.AndroidGoodies.Internal;
using DeadMosquito.IosGoodies;
using Firebase.Auth;
using Firebase.Storage;
using ua.org.gdg.devfest;
using Unity.Collections;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class QuestPhotoController : MonoBehaviour
{
    public Image photoTextureHolder;
    private Texture2D tex;
    private string userID;
    private FirebaseStorage storage;
    public string picturePath;
    private int imageSize = 0;
    private StorageReference pictureReference;
    QuestManager _questManager;
    public string imageUrl;
    byte[] imageBytes;
    string pictureNameInStorage;
    SigninSampleScript signIn;

    void Awake()
    {
        Debug.Log("QuestPhotoController.Awake");
        // obtain reference to object that represents quest manager
        QuestManagerReferenceInitialization();
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

    public void OnPhotoActionButtonClick()
    {
#if UNITY_ANDROID
//        AGAlertDialog.ShowMessageDialog("Photo with speaker",
//            "Please select a photo with speaker.",
//            "Take a photo", OnTakePictureButtonClick,
//            "Pick from Gallery", OnPickPictureButtonClick,
//            "Cancel", () => AGUIMisc.ShowToast("Operation was cancelled"));
        string[] photoActions = { "Take a photo", "Pick from Gallery"};
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
            },AGDialogTheme.Dark);
        
#elif UNITY_IOS
        Debug.Log("Take a picture button Clicked.");
        const bool allowEditing = false;
        const float compressionQuality = 0.1f;
        const IGImagePicker.CameraType cameraType = IGImagePicker.CameraType.Rear;
        const IGImagePicker.CameraFlashMode flashMode = IGImagePicker.CameraFlashMode.Off;
        IGImagePicker.PickImageFromCamera(texture =>
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
                    OnUploadButtonClick();
                }
            }, 
            () => Debug.Log("Picking image from camera cancelled"), 
            compressionQuality,
        allowEditing, cameraType, flashMode);
        
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
        string galleryPath = AGFileUtils.SaveImageToGallery(imageTexture2D, selectedImage.DisplayName, "ARQuest", ImageFormat.JPEG);
        AGGallery.RefreshFile(galleryPath);
        OnUploadButtonClick();
        // Clean up
        Resources.UnloadUnusedAssets();
    }
#elif UNITY_IOS
    
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
        var screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

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

    void OnUploadButtonClick()
    {
#if UNITY_ANDROID
        userID = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        userID = userID.Replace(" ", string.Empty);
        //Creating reference to a picture in the database
        storage = FirebaseStorage.DefaultInstance;
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hoverboard-v2-dev.appspot.com");
        pictureNameInStorage = "/Pictures/" + userID + ".jpeg";
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
#elif UNITY_IOS
        picturePath = "file://" + picturePath;
        userID = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        userID = userID.Replace(" ", string.Empty);
        //Creating reference to a picture in the database
        storage = FirebaseStorage.DefaultInstance;
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hoverboard-v2-dev.appspot.com");
        pictureNameInStorage = "/Pictures/" + userID + ".jpeg";
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

#endif
    }
    Sprite SpriteFromTex2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
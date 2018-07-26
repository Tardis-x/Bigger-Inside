using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using DeadMosquito.AndroidGoodies;
using DeadMosquito.AndroidGoodies.Internal;
using DeadMosquito.IosGoodies;
using Firebase.Auth;
using Firebase.Storage;
using Unity.Collections;
using UnityEngine.UI;

public class QuestPhotoController : MonoBehaviour
{
    public Image photoTextureHolder;
    private Texture2D tex;
    private string userID = "1";
    private FirebaseStorage storage;
    public string picturePath;
    private int imageSize = 0;
    private StorageReference pictureReference;
    QuestManager _questManager;
    public string imageUrl;
    byte[] imageBytes;
    string pictureNameInStorage;

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

    void OnTakePictureButtonClick()
    {
#if UNITY_ANDROID
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
                error => AGUIMisc.ShowToast("Cancelled taking photo from camera: " + error), ImageResultSize.Max1024,
                false);
        }
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
                    Debug.Log("It is there, man...");
                    //Upload
                    OnUploadButtonClick();
                }
            }, 
            () => Debug.Log("Picking image from camera cancelled"), 
            compressionQuality,
        allowEditing, cameraType, flashMode);
        
#endif
    }
    
   public void OnSharePictureButtonClick()
    {
        Debug.Log("Share Button Clicked.");
#if UNITY_ANDROID
        tex = photoTextureHolder.mainTexture as Texture2D;
        string subject = "DevFest Photo";
        string body = "My photo with speaker!";
        AGShare.ShareTextWithImage(subject, body, tex, true, "Share via facebook...");
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

    public void OnUploadButtonClick()
    {
#if UNITY_ANDROID
        userID = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        userID = userID.Replace(" ", string.Empty);
        //Creating reference to a picture in the database
        storage = FirebaseStorage.DefaultInstance;
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hoverboard-v2-dev.appspot.com");
        string pictureNameInStorage = "/Pictures/" + userID + ".jpeg";
        pictureReference = storageRef.Child(pictureNameInStorage);
        //Uploading image
        var task = pictureReference.PutFileAsync(picturePath);
        var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait...", "Uploading...");
        spinner.Show();
        task.ContinueWith(resultTask =>
            {
                spinner.Dismiss();
                imageUrl = "gs://hoverboard-v2-dev.appspot.com" + pictureNameInStorage;
                _questManager.questProgress.photoData.imgUrl = imageUrl;
                _questManager.CheckInPhoto(this);
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
#elif UNITY_IOS
        picturePath = "file://" + picturePath;
        Debug.Log("Picture path changed successfully.");
        try
        {
            userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            Debug.Log("UserID string was initialized.");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        userID = userID.Replace(" ", string.Empty);
        Debug.Log("String was changed.");
        //Creating reference to a picture in the database
        storage = FirebaseStorage.DefaultInstance;
        Debug.Log("Firebase storage was initialized.");
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hoverboard-v2-dev.appspot.com");
        Debug.Log("Storage reference was set.");
        pictureNameInStorage = "/Pictures/" + userID + ".jpeg";
        Debug.Log("Path in storage was set.");
        pictureReference = storageRef.Child(pictureNameInStorage);
        Debug.Log("Child reference was set.");
        //Uploading image
        var pictureMetadata = new MetadataChange();
        pictureMetadata.ContentType = "image/jpeg";
        Debug.Log("Metadata was initialized.");
        var task = pictureReference.PutFileAsync(picturePath, pictureMetadata);
        Debug.Log("Image started uploading.");
        task.ContinueWith(resultTask =>
        {
            imageUrl = "gs://hoverboard-v2-dev.appspot.com" + pictureNameInStorage;
            Debug.Log("Image URL was set.");
            _questManager.questProgress.photoData.imgUrl = imageUrl;
            _questManager.CheckInPhoto(this);
            Debug.Log("Image URL was changed in FireBase.");
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
#if UNITY_ANDROID
    private IEnumerator ShowSpinnerForPhoto()
    {

        // Create spinner dialog
        var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait...", "Saving photo...");
        spinner.Show();
        yield return new WaitForSeconds(2);
        // Dismiss spinner after all the background work is done
        spinner.Dismiss();

    }
#elif UNITY_IOS
    private IEnumerator UploadFIleAfterThreeSeconds()
    {
        yield return new WaitForSeconds(10);
        
    }
#endif
    Sprite SpriteFromTex2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
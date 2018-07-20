using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DeadMosquito.AndroidGoodies;

public class QuestUI : MonoBehaviour
{
    [SerializeField]
    GameObject _menuPanel;

    [SerializeField]
    GameObject _photoPanel;
    [SerializeField]
    GameObject _vrGamePanel;
    [SerializeField]
    GameObject _riddlesPanel;

    GameObject _activePanel;

    QuestManager _questManager;
    
    public Image photoTextureHolder;

    void Awake()
    {
        Debug.Log("QuestUI.Awake()");
        
        // obtain reference to object that represents quest manager
        QuestManagerReferenceInitialization();
        
        // show quest menu screen
        _menuPanel.SetActive(true);

        _activePanel = _menuPanel;
        
        // hide quest screens with tasks
        _photoPanel.SetActive(false);
        _vrGamePanel.SetActive(false);
        _riddlesPanel.SetActive(false);
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

    public void OnMainMenuButtonClicked()
    {
        Debug.Log("QuestUI.OnMainMenuButtonClicked");
        
        SceneManager.LoadScene("MenuScene");
    }

    public void OnPhotoButtonClicked()
    {
        Debug.Log("QuestUI.OnPhotoButtonClicked");
        
        _menuPanel.SetActive(false);
        _activePanel = _photoPanel;
        _photoPanel.SetActive(true);
    }
    
    public void OnVrGameButtonClicked()
    {
        Debug.Log("QuestUI.OnVrGameButtonClicked");
        
        _menuPanel.SetActive(false);
        _activePanel = _vrGamePanel;
        _vrGamePanel.SetActive(true);
    }
    
    public void OnRiddlesButtonClicked()
    {
        Debug.Log("QuestUI.OnRiddlesButtonClicked");
        
        _menuPanel.SetActive(false);
        _activePanel = _riddlesPanel;
        _riddlesPanel.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        Debug.Log("QuestUI.OnBackButtonClicked");
        
        _activePanel.SetActive(false);
        _activePanel = _menuPanel;
        _menuPanel.SetActive(true);
    }
    
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

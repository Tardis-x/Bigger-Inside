using System;
using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class MenuUIManager : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private GameObject _signInPanel;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _notificationsPanel;
    [SerializeField] private DescriptionPanelScript _descriptionPanel;
    [SerializeField] private SchedulePanelScript _schedulePanel;
    [SerializeField] private ScenesManager _scenesManager;
    [SerializeField] private UserPopUp _userPopUp;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      Application.targetFrameRate = 60;
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void ShowSignInPanel()
    {
      _userPopUp.InMenu = false;
      _signInPanel.SetActive(true);
      _menuPanel.SetActive(false);
    }

    public void ShowMenu()
    {
      _userPopUp.InMenu = true;
      if (_userPopUp.Active) _userPopUp.HideSignout();
      if (_scenesManager.SceneToGo != String.Empty) return;
      _signInPanel.SetActive(false);
      _descriptionPanel.SetActive(false);
      _notificationsPanel.SetActive(false);
      _schedulePanel.DisablePanel();
      _menuPanel.SetActive(true);
    }

    public void OnBackToMenuButtonClick()
    {
      _userPopUp.InMenu = true;
      _scenesManager.ResetSceneToGo();
      _signInPanel.SetActive(false);
      _descriptionPanel.SetActive(false);
      _notificationsPanel.SetActive(false);
      _schedulePanel.DisablePanel();
      _menuPanel.SetActive(true);
    }

    public void ShowSchedule()
    {
      _userPopUp.InMenu = false;
      _menuPanel.SetActive(false);
      _schedulePanel.gameObject.SetActive(true);
      _schedulePanel.EnablePanel(2);
      _schedulePanel.EnablePanel(1);
    }
    
    public void ShowNotifications()
    {
      _userPopUp.InMenu = false;
      _notificationsPanel.SetActive(true);
      _menuPanel.SetActive(false);
    }

    public void ShowSpeechDescription(GameObject speech)
    {
      _descriptionPanel.SetActive(true);
      _descriptionPanel.SetData(speech.GetComponent<SpeechItemScript>().GetDescription());
    }

    public void OnLogout()
    {
      if (_userPopUp.Active) _userPopUp.HideSignout();
    }

    public void OpenTermsAndServicesURL()
    {
      Application.OpenURL("https://devfest.gdg.org.ua/terms-and-services");
    }
  }
}
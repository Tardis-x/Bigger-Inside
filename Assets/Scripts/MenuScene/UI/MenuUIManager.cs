using System;
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
    [SerializeField] private DescriptionPanelScript _descriptionPanel;
    [SerializeField] private SchedulePanelScript _schedulePanel;
    [SerializeField] private ScenesManager _scenesManager;
    [SerializeField] private UserPopUp _userPopUp;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public int CurrentDay = 1;

    public void ShowSignInPanel()
    {
      _userPopUp.InMenu = false;
      _signInPanel.SetActive(true);
      _menuPanel.SetActive(false);
      _schedulePanel.DisablePanel();
    }

    public void ShowMenu()
    {
      _userPopUp.InMenu = true;
      if (_userPopUp.Active) _userPopUp.HideSignout();
      if (_scenesManager.SceneToGo != String.Empty) return;
      _signInPanel.SetActive(false);
      _descriptionPanel.SetActive(false);
      _schedulePanel.DisablePanel();
      _menuPanel.SetActive(true);
    }

    public void OnBackToMenuButtonClick()
    {
      _userPopUp.InMenu = true;
      _scenesManager.ResetSceneToGo();
      _signInPanel.SetActive(false);
      _descriptionPanel.SetActive(false);
      _schedulePanel.DisablePanel();
      _menuPanel.SetActive(true);
    }

    public void ShowSchedule()
    {
      _userPopUp.InMenu = false;
      _menuPanel.SetActive(false);
      _schedulePanel.EnablePanel(CurrentDay);
    }

    public void ShowSpeechDescription(GameObject speech)
    {
      _descriptionPanel.SetActive(true);
      _descriptionPanel.SetData(speech.GetComponent<SpeechItemScript>().GetDescription());
    }

    public void SetCurrentDay(int day)
    {
      CurrentDay = day;
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
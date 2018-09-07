using UnityEngine;
using UnityEngine.SceneManagement;

namespace ua.org.gdg.devfest
{
  public class ScenesPanelScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("UI")] 
    [SerializeField] private GameObject _openScenesMenuButton;
    [SerializeField] private GameObject _closeScenesMenuButton;
    [SerializeField] private GameObject _saveSpeakerSceneButton;
    [SerializeField] private GameObject _towerDefenceSceneButton;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OpenScenesMenu()
    {
      ShowOpenMenuButton(false);
      ShowMenu(true);
    }

    public void CloseScenesMenu()
    {
      ShowMenu(false);
      ShowOpenMenuButton(true);
    }

    public void OpenSaveSpeakerScene()
    {
      SceneManager.LoadScene(Scenes.SCENE_SSFSQ);
    }

    public void OpenTowerDefenceScene()
    {
      SceneManager.LoadScene(Scenes.SCENE_TD);
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void ShowOpenMenuButton(bool value)
    {
      _openScenesMenuButton.SetActive(value);
    }

    private void ShowMenu(bool value)
    {
      _closeScenesMenuButton.SetActive(value);
      _saveSpeakerSceneButton.SetActive(value);
      _towerDefenceSceneButton.SetActive(value);
    }
  }
}
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ua.org.gdg.devfest
{
  public class ScenesManager : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private SignInManager _signInManager;
    [SerializeField] private GameEvent _showSignIn;
    [SerializeField] private GameEvent _showLoading;
    [SerializeField] private GameEvent _dismissLoading;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private ProgressDialogSpinner _progressDialogSpinner;

    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnSignIn()
    {
      Debug.Log("OnSignIn");
      if (SceneToGo != string.Empty) GoToScene(SceneToGo);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    [NonSerialized] public string SceneToGo = string.Empty;

    public void GoToARMap()
    {
      ResetSceneToGo();
      GoToScene(Scenes.SCENE_AR_MAP);
    }

    public void GoToARQuest()
    {
      if (_signInManager.UserSignedIn)
      {
        ResetSceneToGo();
        GoToScene(Scenes.SCENE_AR_QUEST);
      }
      else
      {
        SceneToGo = Scenes.SCENE_AR_QUEST;
        _showSignIn.Raise();
      }
    }

    public void ResetSceneToGo()
    {
      SceneToGo = string.Empty;
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private void GoToScene(string sceneName)
    {
      Debug.Log("Going to scene: " + sceneName);
      ShowLoading();
      SceneManager.LoadSceneAsync(sceneName);
    }

    private void ShowLoading()
    {
      _showLoading.Raise();
    }
  }
}
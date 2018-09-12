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
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private ProgressDialogSpinner _progressDialogSpinner;
    

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void OnDestroy()
    {
      DismissLoadingDialog();
    }
    
    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------
   
    public void OnSignIn()
    {
      Debug.Log("OnSignIn");
      if(SceneToGo != string.Empty) GoToScene(SceneToGo);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    [NonSerialized] public string SceneToGo = string.Empty;
    
    public void GoToARMap()
    {
      if (_signInManager.UserSignedIn)
      {
        ResetSceneToGo();
        GoToScene(Scenes.SCENE_AR_MAP);
      }
      else
      {
        SceneToGo = Scenes.SCENE_AR_MAP;
        _showSignIn.Raise();
      }
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
      Debug.Log("Showing Loading Dialog");
      if(_progressDialogSpinner != null) DismissLoadingDialog();
      
      _progressDialogSpinner = new ProgressDialogSpinner("Loading", "Please wait...");
      _progressDialogSpinner.Show();
    }

    private void DismissLoadingDialog()
    {
      Debug.Log("Dismissing loading dialog");
      if (_progressDialogSpinner == null) return;
      
      _progressDialogSpinner.Dismiss();
      _progressDialogSpinner = null;
    }
  }
}
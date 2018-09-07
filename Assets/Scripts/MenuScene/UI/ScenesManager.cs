using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ua.org.gdg.devfest
{
  public class ScenesManager : MonoBehaviour
  {
    private const string AR_MAP_SCENE = "ArMapScene";
    private const string QUEST_SCENE = "ArQuestScene";
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private SignInManager _signInManager;
    [SerializeField] private GameEvent _showSignIn;
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    [NonSerialized] public string SceneToGo = String.Empty;
    
    public void GoToARMap()
    {
      if (_signInManager.UserSignedIn)
      {
        ResetSceneToGo();
        SceneManager.LoadScene(AR_MAP_SCENE);
      }
      else
      {
        SceneToGo = AR_MAP_SCENE;
        _showSignIn.Raise();
      }
      
    }

    public void GoToARQuest()
    {
      if (_signInManager.UserSignedIn)
      {
        ResetSceneToGo();
        SceneManager.LoadScene(QUEST_SCENE);
      }
      else
      {
        SceneToGo = QUEST_SCENE;
        _showSignIn.Raise();
      }
    }

    public void OnSignIn()
    {
      if(SceneToGo != String.Empty) SceneManager.LoadScene(SceneToGo);
    }

    public void ResetSceneToGo()
    {
      SceneToGo = String.Empty;
    }
  }
}
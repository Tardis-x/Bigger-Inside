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

    [SerializeField] private SigninManager _signinManager;
    [SerializeField] private GameEvent _showSignIn;
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void GoToARMap()
    {
      if(_signinManager.UserSignedIn) SceneManager.LoadScene(AR_MAP_SCENE);
      else _showSignIn.Raise();
      
    }

    public void GoToARQuest()
    {
      if(_signinManager.UserSignedIn) SceneManager.LoadScene(QUEST_SCENE);
      else _showSignIn.Raise();
    }
  }
}
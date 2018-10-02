using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class PlayVButtonScript : VirtualButtonOnClick
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    [Header("Events")] 
    [SerializeField] private GameEvent _onCountdownStart;
    [SerializeField] private GameEvent _showTutorial;
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public override void OnClick()
    {
      var tutorShowState = PlayerPrefsHandler.GetTutorStateSSFSQ();

      if (tutorShowState)
      {
        _onCountdownStart.Raise();
      }
      else
      {
        _showTutorial.Raise();
      }
    }
  }
}
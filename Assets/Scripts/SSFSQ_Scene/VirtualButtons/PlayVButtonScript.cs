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
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public override void OnClick()
    {
        _onCountdownStart.Raise();
    }
  }
}

using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class PlayVButtonScript : VirtualButtonOnClick
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public override void OnClick()
    {
      if (!GameManager.Instance.GameActive)
      {
        GameManager.Instance.NewGame();
        UIManager.Instance.ButtonsToPlayMode();
      }
    }
  }
}

namespace ua.org.gdg.devfest
{
  public class PlayVButtonScript : VirtualButtonOnClick
  {
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public override void OnClick()
    {
      if (!GameManager.Instance.GameActive)
      {
        AnimationManager.Instance.ResetAnimations();
        UIManager.Instance.StartGetReadyCountdown(GameManager.Instance.NewGame);
      }
    }
  }
}

namespace ua.org.gdg.devfest
{
  public class AnswerVButtonScript : VirtualButtonOnClick
  {
    public override void OnClick()
    {
      if (GameManager.Instance.GameActive)
      {
        GameManager.Instance.Answer();
        UIManager.Instance.ToAnswerMode();
      }
    }
  }
}
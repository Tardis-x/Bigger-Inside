using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class AnswerVButtonScript : VirtualButtonOnClick
  {
    [Header("Events")] 
    [SerializeField] private GameEvent _onAnswer;
    
    public override void OnClick()
    {
      if (GameManager.Instance.GameActive)
      {
        _onAnswer.Raise();
        UIManager.Instance.ToAnswerMode();
      }
    }
  }
}
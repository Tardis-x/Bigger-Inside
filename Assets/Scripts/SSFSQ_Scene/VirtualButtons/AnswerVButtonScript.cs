using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class AnswerVButtonScript : VirtualButtonOnClick
  {
    [Header("Events")] 
    [SerializeField] private GameEvent _onAnswer;
    
    public override void OnClick()
    {
        _onAnswer.Raise();
    }
  }
}
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class StateBehaviourWithEvents : StateMachineBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Events")] 
    [SerializeField] private GameEvent _onStateEnter;
    [SerializeField] private GameEvent _onStateExit;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      if (_onStateEnter != null) _onStateEnter.Raise();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
      if (_onStateExit != null) _onStateExit.Raise();
    }
  }
}
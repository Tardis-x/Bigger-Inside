using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SpeakerTurnRightStateBehaviour : StateMachineBehaviour
  {
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      AnimationManager.Instance.SpeakerAnimation.TurnRight();
    }
  }
}
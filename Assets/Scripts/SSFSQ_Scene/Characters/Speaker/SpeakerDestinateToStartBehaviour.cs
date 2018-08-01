using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SpeakerDestinateToStartBehaviour : StateMachineBehaviour
  {
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      AnimationManager.Instance.SpeakerAnimation.DestinateToStart();
      animator.SetBool("Turning", true);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
      animator.SetBool("Turning", false);
    }
  }
}
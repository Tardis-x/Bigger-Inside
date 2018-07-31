using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class LookAtTheCrowdStateBehaviour : StateMachineBehaviour
  {
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      AnimationManager.Instance.SpeakerAnimation.LookAtTheCrowd();
      animator.SetBool("FacingTheCrowd", true);
    }
  }
}
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SpeakerHitBehaviour : StateMachineBehaviour
  {
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      AnimationManager.Instance.CrowdControl.CurrentCharacter.GetHit();
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      animator.gameObject.GetComponent<SpeakerAnimationScript>().LookAtTheCrowd();
    }
  }
}
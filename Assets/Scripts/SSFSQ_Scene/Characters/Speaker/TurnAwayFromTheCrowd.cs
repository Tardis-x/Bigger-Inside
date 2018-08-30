using UnityEngine;

public class TurnAwayFromTheCrowd : StateMachineBehaviour
{
  // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    animator.SetBool("FacingTheCrowd", false);
  }

  private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
  {
    animator.SetBool("StartPosition", false);
    animator.SetBool("EndPosition", false);
  }
}
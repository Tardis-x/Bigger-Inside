using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class TomatoThrowBehaviour : StateMachineBehaviour
  {
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      AnimationManager.Instance.TurnTomatoesOn();
    }
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      AnimationManager.Instance.TurnTomatoesOff();
    }
  }
}
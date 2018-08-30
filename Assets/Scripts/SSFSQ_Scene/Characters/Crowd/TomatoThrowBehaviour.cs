using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class TomatoThrowBehaviour : StateMachineBehaviour
  {
    [Header("Events")]
    [SerializeField] private GameEvent _onStartTomatoesThrowing;
    [SerializeField] private GameEvent _onStopTomatoesThrowing;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      _onStartTomatoesThrowing.Raise();
    }
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      _onStopTomatoesThrowing.Raise();
    }
  }
}
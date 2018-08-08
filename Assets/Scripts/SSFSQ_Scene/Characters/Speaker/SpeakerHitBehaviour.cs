using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SpeakerHitBehaviour : StateMachineBehaviour
  {
    [Header("Events")]
    [SerializeField] private GameEvent _onSpeakerHit;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      _onSpeakerHit.Raise();
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      animator.gameObject.GetComponent<SpeakerAnimationScript>().LookAtTheCrowd();
    }
  }
}
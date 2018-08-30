using UnityEngine;

namespace ua.org.gdg.devfest
{
    public class StateBehaviourWithEvents : StateMachineBehaviour
    {
        [Header("Events")] 
        [SerializeField] private GameEvent _onStateEnter;
        [SerializeField] private GameEvent _onStateExit;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
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
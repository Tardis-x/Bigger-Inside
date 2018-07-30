using UnityEngine;
using UnityEngine.Animations;

namespace ua.org.gdg.devfest
{
	public class SpeakerWalkStateBehaviour : StateMachineBehaviour
	{
		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			AnimationManager.Instance.SpeakerAnimation.MoveForward();
		}
	}
}
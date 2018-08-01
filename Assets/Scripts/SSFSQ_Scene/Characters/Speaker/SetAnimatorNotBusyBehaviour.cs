using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class SetAnimatorNotBusyBehaviour : StateMachineBehaviour
	{
		// OnStateMachineExit is called when exiting a statemachine via its Exit Node
		override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) 
		{
			AnimationManager.Instance.SpeakerAnimation.SetBusy(false);
			animator.SetBool("Hit", false);
			animator.SetBool("Answer", false);
		}
	}
}
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class SetAnimatorNotBusyBehaviour : StateMachineBehaviour
	{
		// OnStateMachineExit is called when entering a statemachine via its Exit Node
		override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash) 
		{
			AnimationManager.Instance.SpeakerAnimation.SetBusy(false);
			animator.SetBool("Hit", false);
			animator.SetBool("Answer", false);
			animator.SetBool("Yell", false);
		}

		public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
		{
			animator.SetBool("FacingTheCrowd", false);
		}
	}
}
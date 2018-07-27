using ua.org.gdg.devfest;
using UnityEngine;

public class SpeakerWalkStateBehaviour : StateMachineBehaviour 
{
	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		SpeakerAnimationScript.Instance.MoveFroward();
	}
}

using UnityEngine;

namespace ua.org.gdg.devfest
{

	public class SpeakerDieStateBehaviour : StateMachineBehaviour
	{
		[Header("Events")] 
		[SerializeField] private GameEvent _onSpeakerDied;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			_onSpeakerDied.Raise();
		}
	}
}
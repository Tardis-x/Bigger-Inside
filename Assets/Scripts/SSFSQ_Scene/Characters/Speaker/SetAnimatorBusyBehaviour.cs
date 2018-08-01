﻿using UnityEngine;

namespace ua.org.gdg.devfest
{

	public class SetAnimatorBusyBehaviour : StateMachineBehaviour
	{
		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			AnimationManager.Instance.SpeakerAnimation.SetBusy(true);
		}
	}
}
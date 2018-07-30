using System.Collections;
using System.Collections.Generic;
using ua.org.gdg.devfest;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class AnimationManager : Singleton<AnimationManager>
	{
		//-----------------------------------------------
		// Editor
		//-----------------------------------------------

		[SerializeField] public CrowdControlScript CrowdControl;
		[SerializeField] public SpeakerAnimationScript SpeakerAnimation;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ResetAnimations()
		{
			Instance.CrowdControl.StopThrowing();
			Instance.SpeakerAnimation.StopBeingScared();
		}
	}
}
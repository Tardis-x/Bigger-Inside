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
		[SerializeField] public BoxingGloveScript BoxingGlove;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ResetAnimations()
		{
			Instance.CrowdControl.StopThrowing();
			Instance.CrowdControl.StopBeingScared();
			Instance.SpeakerAnimation.StopBeingScared();
			Instance.SpeakerAnimation.StopBeingDead();
		}
	}
}
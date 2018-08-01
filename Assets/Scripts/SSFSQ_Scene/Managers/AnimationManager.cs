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
		[SerializeField] private ParticleSystem _tomatoes;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			_tomatoes.Stop();
		}

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

		public void TurnTomatoesOn()
		{
			_tomatoes.Play();
		}

		public void TurnTomatoesOff()
		{
			_tomatoes.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
	}
}
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
		[SerializeField] private GameObject _sneaker;
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
			Instance.ShowSneaker(false);
		}

		public void TurnTomatoesOn()
		{
			_tomatoes.Play();
		}

		public void TurnTomatoesOff()
		{
			_tomatoes.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}

		public void ShowSneaker(bool value)
		{
			_sneaker.SetActive(value);
		}

		public void SetComponents(CrowdControlScript crowdControl, SpeakerAnimationScript speakerAnimation, 
			BoxingGloveScript boxingGlove, ParticleSystem tomatoes)
		{
			CrowdControl = crowdControl;
			SpeakerAnimation = speakerAnimation;
			BoxingGlove = boxingGlove;
			_tomatoes = tomatoes;
		}
	}
}
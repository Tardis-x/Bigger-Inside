using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class AnimationManager : Singleton<AnimationManager>
	{
		private CrowdControlScript _crowdControl;
		private SpeakerAnimationScript _speakerAnimation;
		private BoxingGloveScript _boxingGlove;
		private ParticleSystem _tomatoes;
		
		//-----------------------------------------------
		// Editor
		//-----------------------------------------------

		[SerializeField] private GameObject _sneaker;
		
		//---------------------------------------------------------------------
		// Property
		//---------------------------------------------------------------------

		public CrowdControlScript CrowdControl
		{
			get { return _crowdControl; }
		}

		public SpeakerAnimationScript SpeakerAnimation
		{
			get { return _speakerAnimation; }
		}

		public BoxingGloveScript BoxingGlove
		{
			get { return _boxingGlove; }
		}
		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------
		
		public void OnEnvironmentInstantiated()
		{
			var environmentInstance = GameManager.Instance.EnvironmentInstance;
			
			_crowdControl = environmentInstance.CrowdControl;
			_speakerAnimation = environmentInstance.SpeakerAnimation;
			_boxingGlove = environmentInstance.BoxingGlove;
			_tomatoes = environmentInstance.Tomatoes;
			
			_tomatoes.Stop();
		}

		public void OnGameOver()
		{
			ShowSneaker(true);
		}

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ResetAnimations()
		{
			Instance._crowdControl.StopThrowing();
			Instance._crowdControl.StopBeingScared();
			Instance._speakerAnimation.StopBeingScared();
			Instance._speakerAnimation.StopBeingDead();
			Instance.ShowSneaker(false);
		}

		public void TurnTomatoesOn()
		{
			if (_tomatoes != null)
			{
				_tomatoes.Play();
			}
		}

		public void TurnTomatoesOff()
		{
			if (_tomatoes != null)
			{
				_tomatoes.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
		}

		public void ShowSneaker(bool value)
		{
			_sneaker.SetActive(value);
		}
	}
}
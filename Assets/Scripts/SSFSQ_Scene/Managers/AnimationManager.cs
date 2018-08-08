using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class AnimationManager : Singleton<AnimationManager>
	{
		private CrowdControlScript _crowdControl;
		private SpeakerAnimationScript _speakerAnimation;
		
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
		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------
		
		public void OnEnvironmentInstantiated()
		{
			var environmentInstance = GameManager.Instance.EnvironmentInstance;
			
			_crowdControl = environmentInstance.CrowdControl;
			_speakerAnimation = environmentInstance.SpeakerAnimation;
		}

		public void OnGameOver()
		{
			ShowSneaker(true);
		}

		public void OnCountdownStart()
		{
			Debug.Log("AnimationManager: OnCountDownStart");
			ShowSneaker(false);
		}

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ShowSneaker(bool value)
		{
			_sneaker.SetActive(value);
		}
	}
}
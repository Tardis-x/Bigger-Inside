using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class AnimationManager : Singleton<AnimationManager>
	{
		//-----------------------------------------------
		// Editor
		//-----------------------------------------------

		[SerializeField] private GameObject _sneaker;
		
		//---------------------------------------------------------------------
		// Property
		//---------------------------------------------------------------------

		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------

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
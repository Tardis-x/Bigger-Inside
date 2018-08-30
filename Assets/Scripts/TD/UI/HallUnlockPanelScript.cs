using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class HallUnlockPanelScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Text _hallUnlockedMessage;
		[SerializeField] private GameEvent _pauseGameEvent;
		[SerializeField] private GameEvent _resumeGameEvent;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------
		
		public void OKButtonOnClick()
		{
			gameObject.SetActive(false);
			_resumeGameEvent.Raise();
		}

		public void OnHallUnlocked(int hall)
		{
			_pauseGameEvent.Raise();
			_hallUnlockedMessage.text = "SPEECH IN HALL #" + hall + " FINISHED";
		}
	}
}
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class TDGameOverPanelScript : MonoBehaviour
	{
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

		[SerializeField] private Text _scoreText;
		[SerializeField] private GameEvent _restart;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ShowPanel(int score)
		{
			_scoreText.text = score.ToString();
			gameObject.SetActive(true);
		}

		public void HidePanel()
		{
			gameObject.SetActive(false);
		}
		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------

		public void OnReplayButtonClick()
		{
			_restart.Raise();
		}
	}
}
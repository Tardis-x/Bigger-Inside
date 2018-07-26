
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class GameOverPanelScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Text _scoreText;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ShowPanel()
		{
			gameObject.SetActive(true);
		}

		public void HidePanel()
		{
			gameObject.SetActive(false);
		}

		public void SetScore(int score)
		{
			_scoreText.text = "YOUR SCORE: " + score;
		}
	}
}
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class PlayButton : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private TutorialPanelScript _tutorialPanel;
		[SerializeField] private GameEvent _gameStart;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		public void OnClick()
		{
			var tutorShowState = PlayerPrefsHandler.GetTutorState();

//			if (tutorShowState)
//			{
//				_gameStart.Raise();
//			}
//			else
//			{
				gameObject.SetActive(false);
				_tutorialPanel.ShowPanel(true);
//			}
		}
	}
}
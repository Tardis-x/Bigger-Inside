using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class PlayButton : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Canvas _canvas;
		[SerializeField] private TutorialPanelScript _tutorialPanel;
		[SerializeField] private GameEvent _gameStart;
		[SerializeField] private InstanceGameEvent _prepareSpawnPoints;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		public void OnClick()
		{
			var tutorShowState = PlayerPrefsHandler.GetTutorState();

			if (tutorShowState)
			{
				_prepareSpawnPoints.Raise(_canvas.gameObject);
				_gameStart.Raise();
			}
			else
			{
				gameObject.SetActive(false);
				_tutorialPanel.ShowPanel(true);
			}
		}
	}
}
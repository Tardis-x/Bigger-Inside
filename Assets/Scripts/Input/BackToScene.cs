using UnityEngine;
using UnityEngine.SceneManagement;

namespace ua.org.gdg.devfest
{
	public class BackToScene : MonoBehaviour
	{
		public string SceneToGoBackTo = "Menu";

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape) && !PanelManager.Instance.SchedulePanel.Active
			    && !PanelManager.Instance.SpeechDescriptionPanel.Active) SceneManager.LoadScene(SceneToGoBackTo);
		}
	}
}
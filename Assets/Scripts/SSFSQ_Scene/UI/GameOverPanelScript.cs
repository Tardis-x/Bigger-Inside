
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class GameOverPanelScript : MonoBehaviour
	{
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
	}
}
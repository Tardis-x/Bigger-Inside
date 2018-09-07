using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class SceneNavigationButton : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private string _sceneToGoTo = "MenuScene";
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------
		
		private Button _menuButton;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------
		
		void Awake()
		{
			_menuButton = GetComponent<Button>();
			_menuButton.onClick.AddListener(GoToScene);
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		void GoToScene()
		{
			SceneManager.LoadScene(_sceneToGoTo);
		}
	}
}

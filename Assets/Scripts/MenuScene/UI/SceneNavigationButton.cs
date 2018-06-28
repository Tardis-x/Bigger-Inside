using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class SceneNavigationButton : MonoBehaviour
{
	private UnityEngine.UI.Button _menuButton;
	public string SceneToGoTo = "Menu";
	
	// Use this for initialization
	void Awake ()
	{
		_menuButton = GetComponent<UnityEngine.UI.Button>();
		_menuButton.onClick.AddListener(GoToScene);
	}

	void GoToScene()
	{
		SceneManager.LoadScene(SceneToGoTo);
	}
}

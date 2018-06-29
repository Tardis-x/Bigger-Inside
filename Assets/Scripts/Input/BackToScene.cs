using System.Collections;
using System.Collections.Generic;
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
			if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(SceneToGoBackTo);
		}
	}
}
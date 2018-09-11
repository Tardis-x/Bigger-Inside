using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

namespace ua.org.gdg.devfest
{
	public class Manager : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private GameObject _planeFinder;

		[Space] 
		[Header("UI")] 
		[SerializeField] private PanelManager _firebasePanel;
		[SerializeField] private ObjectClick _objectClick;

		[Space]
		[Header("Cameras")]
		[SerializeField] private Camera _arCamera;
		[SerializeField] private Camera _mainCamera;
		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------

		public void OnTrackingLost()
		{
			EnableObjectClick(false);			
		}

		public void OnTrackingFound()
		{
			Invoke("EnableObjectClick", 0.5f);
		}
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------
		
		private void Start()
		{
			StartCoroutine(EnableARCamera());
			EnableObjectClick(true);
		}
		
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape) && !PanelManager.Instance.SchedulePanelNew.Active
			    && !PanelManager.Instance.SpeechDescriptionPanelNew.Active) GoToMenu();
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void GoToMenu()
		{
			SceneManager.LoadScene(Scenes.SCENE_MENU);
		}
		
		public void OnContentPlaced(GameObject anchor)
		{
			Invoke("EnableObjectClick", 0.5f);
		}

		public void OnInteractiveHitTest(HitTestResult hitTestResult)
		{
			if (_firebasePanel.IsPanelActive()) return;
			
			_planeFinder.GetComponent<ContentPositioningBehaviour>().PositionContentAtPlaneAnchor(hitTestResult);
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private IEnumerator EnableARCamera()
		{
			yield return new WaitForEndOfFrame();
			
			_arCamera.gameObject.SetActive(true);
			_mainCamera.gameObject.SetActive(false);
		}
		
		private void EnableObjectClick()
		{
			_objectClick.IsInteractable = true;
		}

		private void EnableObjectClick(bool value)
		{
			_objectClick.IsInteractable = value;
		}
	}
}
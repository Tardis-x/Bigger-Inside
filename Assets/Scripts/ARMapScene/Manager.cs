using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace ua.org.gdg.devfest
{
	public class Manager : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[Header("Prefabs")]
		[SerializeField] private GameObject _environment;

		[Space]
		[Header("Targets")]
		[SerializeField] private GameObject _imageTarget;
		[SerializeField] private GameObject _planeFinder;

		[Space] 
		[Header("UI")] 
		[SerializeField] private PanelManager _firebasePanel;
		[SerializeField] private ObjectClick _objectClick;
		[SerializeField] private Text _hint;

		[Space]
		[Header("Cameras")]
		[SerializeField] private Camera _arCamera;
		[SerializeField] private Camera _mainCamera;
		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------

		public void OnTrackingLost()
		{
			var arCoreSupport = ARCoreHelper.CheckArCoreSupport();
								
			_hint.gameObject.SetActive(arCoreSupport);
			_planeFinder.SetActive(arCoreSupport);
			EnableObjectClick(false);			
		}

		public void OnTrackingFound()
		{
			_planeFinder.SetActive(false);
			ShowHint(false);
			Invoke("EnableObjectClick", 0.5f);
		}
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Awake()
		{
			LoadResources(ARCoreHelper.CheckArCoreSupport());
		}

		private void Start()
		{
			var arCoreSupport = ARCoreHelper.CheckArCoreSupport();
			StartCoroutine(EnableARCamera());
			PrepareScene(arCoreSupport);
		}

		public void OnContentPlaced(GameObject anchor)
		{
			_planeFinder.SetActive(false);
			ShowHint(false);
			Invoke("EnableObjectClick", 0.5f);
		}

		public void OnAutomaticHitTest(HitTestResult hitTestResult)
		{
			if (hitTestResult == null)
			{
				_hint.gameObject.SetActive(true);
				return;
			}
						
			_hint.gameObject.SetActive(false);
		}

		public void OnInteractiveHitTest(HitTestResult hitTestResult)
		{
			if (_firebasePanel.IsPanelActive()) return;
			
			_planeFinder.GetComponent<ContentPositioningBehaviour>().PositionContentAtPlaneAnchor(hitTestResult);
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private void LoadResources(bool arCoreSupport)
		{
			if (arCoreSupport)
			{
				_planeFinder.GetComponent<ContentPositioningBehaviour>().AnchorStage =
					Resources.Load<AnchorBehaviour>("ARMap/GroundPlaneStage");
			}
			else
			{
				_environment = Resources.Load<GameObject>("ARMap/Environment");
			}
		}
		
		private void PrepareScene(bool arCoreSupport)
		{
			_planeFinder.gameObject.SetActive(arCoreSupport);
			_hint.gameObject.SetActive(arCoreSupport);
			_imageTarget.SetActive(!arCoreSupport);
      
			if (!arCoreSupport)
			{
				Instantiate(_environment, _imageTarget.transform);  
				EnableObjectClick(true);
			}
		}

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

		private void ShowHint(bool value)
		{
			_hint.gameObject.SetActive(value);
		}
	}
}
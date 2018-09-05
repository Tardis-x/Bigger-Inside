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
		[SerializeField] private GameObject _descriptionPanel;
		[SerializeField] private GameObject _schedulePanel;
		[SerializeField] private ObjectClick _objectClick;
		[SerializeField] private Text _hint;
		
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

		private void Start()
		{
			var arCoreSupport = ARCoreHelper.CheckArCoreSupport();
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
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------
		
		private void PrepareScene(bool arCoreSupport)
		{
			_planeFinder.gameObject.SetActive(arCoreSupport);
			_hint.gameObject.SetActive(arCoreSupport);
			_imageTarget.SetActive(!arCoreSupport);
      
			if (!arCoreSupport)
			{
				Instantiate(_environment, _imageTarget.transform);  
				EnableObjectClick();
			}
		}

		private void ShowFirebaseUI(bool value)
		{
			_descriptionPanel.SetActive(value);
			_schedulePanel.SetActive(value);
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
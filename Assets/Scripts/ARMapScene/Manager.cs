using UnityEngine;

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
		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------

		public void OnTrackingLost()
		{
			ShowFirebaseUI(false);
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
			ShowFirebaseUI(false);
			Invoke("EnableObjectClick", 0.5f);
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------
		
		private void PrepareScene(bool arCoreSupport)
		{
			_planeFinder.gameObject.SetActive(arCoreSupport);
      
			_imageTarget.SetActive(!arCoreSupport);
      
			if (!arCoreSupport)
			{
				Instantiate(_environment, _imageTarget.transform);        
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
	}
}
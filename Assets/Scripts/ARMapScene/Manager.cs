using UnityEngine;
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
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			var arCoreSupport = ARCoreHelper.CheckArCoreSupport();
			PrepareScene(arCoreSupport);
		}

		public void OnContentPlaced(GameObject environment)
		{
			_planeFinder.SetActive(false);
			_planeFinder.GetComponent<PlaneFinderBehaviour>().OnInteractiveHitTest.RemoveAllListeners();
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
	}
}
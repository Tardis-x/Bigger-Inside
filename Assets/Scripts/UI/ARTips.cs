using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class ARTips : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private GameObject _vuforiaTip;
		[SerializeField] private GameObject _arTip;
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private bool _arCoreSupport;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------
		
		private void Start()
		{
			_arCoreSupport = ARCoreHelper.CheckArCoreSupport();
			ShowTipPanel(true);
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void OnTrackableLost()
		{
			ShowTipPanel(true);
		}

		public void OnTrackableFound()
		{
			_arTip.SetActive(false);
			_vuforiaTip.SetActive(false);
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private void ShowTipPanel(bool value)
		{
			if (_arCoreSupport)
			{
				_arTip.SetActive(value);
			}
			else
			{
				_vuforiaTip.SetActive(value);
			}
		}
	}
}
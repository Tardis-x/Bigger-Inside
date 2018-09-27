using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class ProgressBarScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private GameObject _progressBar;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------
		
		public void ShowLoading()
		{
			_progressBar.gameObject.SetActive(true); 
		}
    
		public void DismissLoading()
		{
			_progressBar.gameObject.SetActive(false); 
		}
	}
}
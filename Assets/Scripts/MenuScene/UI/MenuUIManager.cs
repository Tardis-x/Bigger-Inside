using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class MenuUIManager : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private GameObject _signInPanel;
		[SerializeField] private GameObject _menuPanel;
		[SerializeField] private DescriptionPanelScript _descriptionPanel;
		[SerializeField] private UserPopUp _userPopUp;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ShowSignInPanel()
		{
			_signInPanel.SetActive(true);
			_menuPanel.SetActive(false);
		}

		public void ShowMenu()
		{
			_signInPanel.SetActive(false);
			_menuPanel.SetActive(true);
		}
		
		public void ShowSpeechDescription(GameObject speech)
		{
			_descriptionPanel.SetActive(true);
			_descriptionPanel.SetData(speech.GetComponent<SpeechItemScript>().GetDescription());
		}
	}
}
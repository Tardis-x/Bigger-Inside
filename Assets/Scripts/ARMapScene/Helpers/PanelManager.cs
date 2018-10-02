using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class PanelManager : Singleton<PanelManager>
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] public SchedulePanelScript SchedulePanelNew;
		[SerializeField] public DescriptionPanelScript SpeechDescriptionPanelNew;
		[SerializeField] public InfoCoinNamePanel InfoCoinNamePanel;

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------
		
		public string CurrentHall;
		
		public void ShowSpeechDescription(GameObject speech)
		{
			ClearAll();
			SpeechDescriptionPanelNew.SetActive(true);
			SpeechDescriptionPanelNew.SetData(speech.GetComponent<SpeechItemScript>().GetDescription());
		}

		public bool IsPanelActive()
		{
			return SchedulePanelNew.gameObject.activeSelf ||
			       SpeechDescriptionPanelNew.gameObject.activeSelf;
		}

		public void ShowCoinNamePanel(string coinName)
		{
			ClearAll();
			InfoCoinNamePanel.OpenPanel(coinName);
		}

		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------
		
		private void ClearAll()
		{
			InfoCoinNamePanel.gameObject.SetActive(false);
		}
	}
}

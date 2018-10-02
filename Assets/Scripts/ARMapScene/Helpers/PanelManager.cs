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
		[SerializeField] public InfoCoinSchedulePanel InfoCoinSchedulePanel;

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------
		
		public string CurrentHall;

		public void ShowSchedulePanel(string hall)
		{
			ClearAll();
			CurrentHall = hall;
			SchedulePanelNew.EnablePanel(2, hall);
			SchedulePanelNew.EnablePanel(1, hall);
		}
		
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

		// Coins Panels
		
		public void ShowCoinNamePanel(string coinName)
		{
			ClearAll();
			InfoCoinNamePanel.OpenPanel(coinName);
		}

		public void ShowCoinSchedulePanel(string hallName)
		{
			ClearAll();
			InfoCoinSchedulePanel.OpenPanel(hallName);
		}

		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------
		
		private void ClearAll()
		{
			InfoCoinNamePanel.gameObject.SetActive(false);
			InfoCoinSchedulePanel.gameObject.SetActive(false);
		}
	}
}

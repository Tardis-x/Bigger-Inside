using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class PanelManager : Singleton<PanelManager>
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[Header("Panels")]
		[SerializeField] public SchedulePanelScript SchedulePanelNew;
		[SerializeField] public DescriptionPanelScript SpeechDescriptionPanelNew;
		
		[SerializeField] private InfoCoinNamePanel _infoCoinNamePanel;
		[SerializeField] private InfoCoinSchedulePanel _infoCoinSchedulePanel;
		[SerializeField] private InfoCoinGroupPanel _infoCoinGroupPanel;

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------
		
		public string CurrentHall;

		public void ShowSchedulePanel(string hall)
		{
			CurrentHall = hall;
			SchedulePanelNew.EnablePanel(2, hall);
			SchedulePanelNew.EnablePanel(1, hall);
		}
		
		public void ShowSpeechDescription(GameObject speech)
		{
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
			_infoCoinNamePanel.OpenPanel(coinName);
		}

		public void ShowCoinSchedulePanel(string hallName)
		{
			ClearAll();
			_infoCoinSchedulePanel.OpenPanel(hallName);
		}

		public void ShowInfoCoinGroupPanel(InfoCoinGroup infoCoinGroup)
		{
			ClearAll();
			_infoCoinGroupPanel.OpenPanel(infoCoinGroup);
		}

		public void ClearAll()
		{
			_infoCoinNamePanel.gameObject.SetActive(false);
			_infoCoinSchedulePanel.gameObject.SetActive(false);
			_infoCoinGroupPanel.gameObject.SetActive(false);
		}
	}
}

using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class PanelManager : Singleton<PanelManager>
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		public ScrollableListScript SchedulePanel;
		public DescriptionPanelScript SpeechDescriptionPanel;

		public SchedulePanelScript SchedulePanelNew;
		public DescriptionPanelScript SpeechDescriptionPanelNew;
		
		public void ShowSpeechDescription(GameObject speech)
		{
			SpeechDescriptionPanelNew.SetActive(true);
			SpeechDescriptionPanelNew.SetData(speech.GetComponent<SpeechItemScript>().GetDescription());
		}
	}
}

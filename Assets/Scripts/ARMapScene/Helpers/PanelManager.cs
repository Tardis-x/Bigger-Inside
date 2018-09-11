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

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------
		
		public string CurrentHall;
		
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
	}
}

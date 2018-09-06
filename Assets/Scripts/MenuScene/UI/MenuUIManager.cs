using System;
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
		[SerializeField] private SchedulePanelScript _schedulePanel;
		[SerializeField] private ScenesManager _scenesManager;

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public int CurrentDay = 1;

		public void ShowSignInPanel()
		{
			_signInPanel.SetActive(true);
			_menuPanel.SetActive(false);
		}

		public void ShowMenu()
		{
			if(_scenesManager.SceneToGo != String.Empty) return;
			_signInPanel.SetActive(false);
			_descriptionPanel.SetActive(false);
			_schedulePanel.DisablePanel();
			_menuPanel.SetActive(true);
		}

		public void OnBackToMenuButtonClick()
		{
			_scenesManager.ResetSceneToGo();
			_signInPanel.SetActive(false);
			_descriptionPanel.SetActive(false);
			_schedulePanel.DisablePanel();
			_menuPanel.SetActive(true);
		}

		public void ShowSchedule()
		{
			_menuPanel.SetActive(false);
			_schedulePanel.EnablePanel(CurrentDay);
		}
		
		public void ShowSpeechDescription(GameObject speech)
		{
			_descriptionPanel.SetActive(true);
			_descriptionPanel.SetData(speech.GetComponent<SpeechItemScript>().GetDescription());
		}

		public void SetCurrentDay(int day)
		{
			CurrentDay = day;
		}
	}
}
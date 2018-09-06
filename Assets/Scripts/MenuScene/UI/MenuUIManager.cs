using UnityEngine;
using DeadMosquito.AndroidGoodies;

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
		
#if UNITY_ANDROID
		private AGProgressDialog _loginSpinner;
#endif
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Awake()
		{
#if UNITY_ANDROID
			_loginSpinner = AGProgressDialog.CreateSpinnerDialog("Please wait", "Signing in...", AGDialogTheme.Dark);
#endif
		}

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
			_signInPanel.SetActive(false);
			_descriptionPanel.SetActive(false);
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

		public void OnSignIn()
		{
#if UNITY_ANDROID
			//_loginSpinner.Show();
#endif
		}

		public void OnSignInFinished()
		{
#if UNITY_ANDROID
			//_loginSpinner.Dismiss();
#endif
		}
	}
}
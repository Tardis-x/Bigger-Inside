using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class UserPopUp : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Fade _fadeImage;
		[SerializeField] private Button _logOutButton;
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private bool _active;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void Toggle()
		{
			_active = !_active;
			if(_active) Show();
			else Hide();
		}

		public void Show()
		{
			_fadeImage.FadeOut();
			_logOutButton.enabled = true;
		}

		public void Hide()
		{
			_fadeImage.FadeIn();
			_logOutButton.enabled = false;
		}
	}
}
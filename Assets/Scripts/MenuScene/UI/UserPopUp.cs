using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

namespace ua.org.gdg.devfest
{
	public class UserPopUp : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Fade _fadeImage;
		[SerializeField] private Button _signInOutButton;
		[SerializeField] private Text _signInOutText;

		[Space] 
		[Header("Events")] 
		[SerializeField] private GameEvent _signOutRequest;
		[SerializeField] private GameEvent _signInRequest;

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		public bool Active { get; set; }

		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private void SetSignInButton(bool userSignedIn)
		{
			_signInOutButton.onClick.RemoveAllListeners();
			
			if (userSignedIn)
			{
				_signInOutText.text = "Sign Out";
				_signInOutButton.onClick.AddListener(SignOut);
			}
			else
			{
				_signInOutText.text = "Sign In";
				_signInOutButton.onClick.AddListener(SignIn);
			}
		}

		private void SignOut()
		{
			_signOutRequest.Raise();
			Hide();
		}

		private void SignIn()
		{
			_signInRequest.Raise();
			Hide();
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void Toggle()
		{
			Active = !Active;
			if(Active) Show();
			else Hide();
		}

		public void Show()
		{
			SetSignInButton(FirebaseAuth.DefaultInstance.CurrentUser != null);

			Active = true;
			_fadeImage.FadeOut();
			_signInOutButton.enabled = true;
		}

		public void Hide()
		{
			Active = false;
			_fadeImage.FadeIn();
			_signInOutButton.enabled = false;
		}
	}
}
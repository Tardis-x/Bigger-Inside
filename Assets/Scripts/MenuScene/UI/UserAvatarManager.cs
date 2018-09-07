using System;
using System.Collections;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

namespace ua.org.gdg.devfest
{
	public class UserAvatarManager : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Texture _defaultUserAvatar;
		[SerializeField] private RawImage _userAvatar;
		[SerializeField] private ImageLoader _imageLoader;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			SetUserAvatar();
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void SetDefaultUserAvatar()
		{
			_userAvatar.texture = _defaultUserAvatar;
		}

		public void SetUserAvatar()
		{
			if (FirebaseAuth.DefaultInstance.CurrentUser == null) return;

			if(FB.IsLoggedIn) FB.API("/me/picture?redirect=false", HttpMethod.GET, ProfilePhotoCallback);
			else _imageLoader.LoadImage(FirebaseAuth.DefaultInstance.CurrentUser.PhotoUrl.OriginalString, _userAvatar);
		}
		
		private void ProfilePhotoCallback (IGraphResult result)
		{
			if (String.IsNullOrEmpty(result.Error) && !result.Cancelled) {
				IDictionary data = result.ResultDictionary["data"] as IDictionary;
				string photoURL = data["url"] as String;
 
				_imageLoader.LoadImage(photoURL, _userAvatar);
			}
		}
	}
}
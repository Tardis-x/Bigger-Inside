using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using ua.org.gdg.devfest;

public class UserAvatarManager : MonoBehaviour 
{

	//---------------------------------------------------------------------
	// Editor
	//---------------------------------------------------------------------

	[SerializeField] private Texture _defaultUserAvatar;
	[SerializeField] private RawImage _userAvatar;
	[SerializeField] private ImageLoader _imageLoader;
		
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	public void SetDefaultUserAvatar()
	{
		_userAvatar.texture = _defaultUserAvatar;
	}

	public void SetUserAvatar()
	{
		_imageLoader.LoadImage(FirebaseAuth.DefaultInstance.CurrentUser.PhotoUrl.OriginalString, _userAvatar);
	}
}

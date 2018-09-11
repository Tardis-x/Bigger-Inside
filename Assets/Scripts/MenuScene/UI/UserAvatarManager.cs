using System.Linq;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

namespace ua.org.gdg.devfest
{
	public class UserAvatarManager : MonoBehaviour
	{
		private const string USER_AVATAR_FILENAME = "img_user_avatar.jpg";
		
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

			string photoUrl = FirebaseAuth.DefaultInstance.CurrentUser.PhotoUrl.OriginalString;
			photoUrl = GetHigherResProfilePic(photoUrl);
			
			Debug.Log("PHOTO_URL: " + photoUrl);
			
			_imageLoader.LoadImage(photoUrl, USER_AVATAR_FILENAME, _userAvatar);
		}

		public void ClearUserAvatarCache()
		{
			_imageLoader.DeleteImage(USER_AVATAR_FILENAME);
		}

		private string GetHigherResProfilePic(string photoUrl)
		{
			string result = photoUrl;
			
			if (photoUrl.Contains("s96-c"))
			{
				result = photoUrl.Replace("s96-c", "s400-c");
			}
			else
			{
				result = photoUrl + "?type=large";
			}
			return result;
		}
		
	}
}
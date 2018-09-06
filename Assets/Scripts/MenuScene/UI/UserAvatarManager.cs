using UnityEngine;
using UnityEngine.UI;

public class UserAvatarManager : MonoBehaviour 
{

	//---------------------------------------------------------------------
	// Editor
	//---------------------------------------------------------------------

	[SerializeField] private Texture _defaultUserAvatar;
	[SerializeField] private RawImage _userAvatar;
		
	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	public void SetDefaultUserAvatar()
	{
		_userAvatar.texture = _defaultUserAvatar;
	}
}

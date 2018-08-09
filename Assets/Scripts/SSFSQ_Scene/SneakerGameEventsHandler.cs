using UnityEngine;

public class SneakerGameEventsHandler : MonoBehaviour {
	
	//---------------------------------------------------------------------
	// Events
	//---------------------------------------------------------------------

	public void OnGameOver()
	{
		ShowSneaker(true);
	}

	public void OnCountdownStart()
	{
		Debug.Log("AnimationManager: OnCountDownStart");
		ShowSneaker(false);
	}

	//---------------------------------------------------------------------
	// Public
	//---------------------------------------------------------------------

	private void ShowSneaker(bool value)
	{
		gameObject.SetActive(value);
	}
}

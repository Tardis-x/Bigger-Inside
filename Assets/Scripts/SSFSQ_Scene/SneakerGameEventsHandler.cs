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

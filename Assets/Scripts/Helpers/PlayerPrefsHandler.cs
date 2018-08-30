using UnityEngine;

public static class PlayerPrefsHandler
{
	private const string PASSED_TUTORIAL = "PASSED_TUTORIAL";

	public static void SetTutorState(bool value)
	{
		PlayerPrefs.SetInt(PASSED_TUTORIAL, value ? 1 : 0);
	}

	public static bool GetTutorState()
	{
		return PlayerPrefs.GetInt(PASSED_TUTORIAL, 0) == 1;
	}
}

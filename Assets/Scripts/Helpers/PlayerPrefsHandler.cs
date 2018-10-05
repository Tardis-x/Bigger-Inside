using UnityEngine;

public static class PlayerPrefsHandler
{
	private const string PASSED_TUTORIAL_TD = "PASSED_TUTORIAL_TD";
	private const string PASSED_TUTORIAL_SSFSQ = "PASSED_TUTORIAL_SSFSQ";
	private const string PASSED_TUTORIAL_AR_MAP = "PASSED_TUTORIAL_AR_MAP";

	public static void SetTutorStateTD(bool value)
	{
		PlayerPrefs.SetInt(PASSED_TUTORIAL_TD, value ? 1 : 0);
	}

	public static bool GetTutorStateTD()
	{
		return PlayerPrefs.GetInt(PASSED_TUTORIAL_TD, 0) == 1;
	}
	
	public static void SetTutorStateSSFSQ(bool value)
	{
		PlayerPrefs.SetInt(PASSED_TUTORIAL_SSFSQ, value ? 1 : 0);
	}

	public static bool GetTutorStateSSFSQ()
	{
		return PlayerPrefs.GetInt(PASSED_TUTORIAL_SSFSQ, 0) == 1;
	}
	
	public static void SetTutorStateARMap(bool value)
	{
		PlayerPrefs.SetInt(PASSED_TUTORIAL_AR_MAP, value ? 1 : 0);
	}

	public static bool GetTutorStateARMap()
	{
		return PlayerPrefs.GetInt(PASSED_TUTORIAL_AR_MAP, 0) == 1;
	}
}

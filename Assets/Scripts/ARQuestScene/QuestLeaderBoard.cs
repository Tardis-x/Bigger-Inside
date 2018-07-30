using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLeaderBoard : MonoBehaviour
{
	public Text scoreText;
	QuestProgress _questProgress;
	public Button playerInfoButtonPrefab;
	public GameObject gridForList;
	private QuestLeaderboardEntry[] _leaderbordData;
	
	public void UpdateScore()
	{
		scoreText.text = "Your score is: " + _questProgress.globalScore;
	}

	public void UpdateLeaderBoard()
	{
		//TODO
	}
	
}

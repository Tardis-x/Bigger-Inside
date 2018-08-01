using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeadMosquito.AndroidGoodies;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class QuestLeaderBoardController : MonoBehaviour
{
	public Text scoreText;
	public Button playerInfoButtonPrefab;
	public Transform gridForList;
	QuestManager _questManager;
	
	void Awake()
	{
		Debug.Log("QuestLeaderBoardController.Awake");
		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();
	}

	private void OnEnable()
	{
		Debug.Log("QuestLeaderBoardController.OnEnable");
		_questManager.UpdateUserScoreInLeaderBoard();
		UpdateLeaderBoard();
	}

	void QuestManagerReferenceInitialization()
	{
		GameObject questManagerTemp = GameObject.Find("QuestManager");

		if (questManagerTemp != null)
		{
			_questManager = questManagerTemp.GetComponent<QuestManager>();

			if (_questManager == null)
			{
				Debug.LogError("Could not locate QuestManager component on " + questManagerTemp.name);
			}
		}
		else
		{
			Debug.LogError("Could not locate quest manager object in current scene!");
		}
	} 
	
	void UpdateLeaderBoard()
	{
		//Update player global score
		scoreText.text = "Your score is: " + _questManager.questProgress.globalScore;
		//Sort the leaderboard and present it
		var items = from pair in _questManager.QuestLeaderboardData
			orderby pair.Value descending 
			select pair;
		int i = 1;
		foreach (KeyValuePair<string, int> pair in items)
		{
			Button x = Instantiate(playerInfoButtonPrefab, transform.position, Quaternion.identity, gridForList);
			x.GetComponentInChildren<Text>().text = i + ". " + pair.Key + ". Score: " + pair.Value + " point(s).";
			i++;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestLeaderBoardController : MonoBehaviour
{
	public Text scoreText;
	public Text textPrefab;
	public Transform gridForList;
	QuestManager _questManager;
	public Scrollbar _scrollbar;
	
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
		int i = 1;
		foreach (KeyValuePair<string, int> pair in _questManager.QuestLeaderboardData.OrderByDescending(key => key.Value))
		{
			Text userInfo = Instantiate(textPrefab, transform.position, Quaternion.identity, gridForList);
			userInfo.text = String.Format("   {0}. {1} \n    Score: {2} point(s).", i, pair.Key, pair.Value);
			if (pair.Key == _questManager.currentUserUserId)
			{
				Color color = new Color(0.359f, 0.672f, 0.93f);
				userInfo.color = color;
				scoreText.text = i + ". Your score is: " + _questManager.questProgress.globalScore;
			}
			i++;
		}

		_scrollbar.value = 1;
	}
}

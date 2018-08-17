using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestLeaderBoardController : MonoBehaviour
{
	public GameObject leaderboardEntryPrefab;
	public Transform gridForList;
	QuestManager _questManager;
	public Scrollbar _scrollbar;
	public Sprite currentUserSprite;
	
	void Awake()
	{
		Debug.Log("QuestLeaderBoardController.Awake");
		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();
	}

	void OnEnable()
	{
		Debug.Log("QuestLeaderBoardController.OnEnable");
		_questManager.UpdateUserScoreInLeaderBoard();
		UpdateLeaderBoard();
	}

	void QuestManagerReferenceInitialization()
	{
		var questManagerTemp = GameObject.Find("QuestManager");

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
		var i = 1;
		foreach (var pair in _questManager.QuestLeaderboardData.OrderByDescending(entry => entry.Value.globalScore))
		{
			var userInfo = Instantiate(leaderboardEntryPrefab, transform.position, Quaternion.identity, gridForList);
			
			Text[] texts = userInfo.GetComponentsInChildren<Text>();

			foreach (var text in texts)
			{
				if (text.CompareTag("PositionText"))
				{
					text.text = i.ToString();
				}
				else if (text.CompareTag("UserNameText"))
				{
					text.text = pair.Key;
				}
				else if (text.CompareTag("UserScoreText"))
				{
					text.text = pair.Value.globalScore.ToString();
				}
			}
			
			Image[] images = userInfo.GetComponentsInChildren<Image>();

			foreach (var image in images)
			{
				if (image.CompareTag("UserImage"))
				{
					StartCoroutine(LoadUserImageFromUrl(pair.Value.userPhotoUrl.ToString(), image));
				}
			}
			
			//Extra for current user
			if (pair.Key == _questManager.currentUserUserId)
			{
				Color color = Color.white;
				color.a = 1;
				userInfo.GetComponent<Image>().color = color;
				userInfo.GetComponent<Image>().sprite = currentUserSprite;
			}
			i++;
		}
		_scrollbar.value = 1;
	}
	
	IEnumerator LoadUserImageFromUrl(string url, Image image)
	{
		var www = new WWW(url);
		yield return www;
		image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	}
}

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestUserScoreController : MonoBehaviour
{
	QuestManager _questManager;

	public QuestFirebaseData firebaseData;
	public Text scoreText;
	public Text positionInfoText;
	public Text gate2ScoreText;
	public Text gate3ScoreText;
	
	void Awake()
	{
		QuestManagerReferenceInitialization();
	}

	void OnEnable()
	{
		_questManager.UpdateUserScoreInLeaderBoard();
		UpdateUserInfo();
		StartCoroutine(LateUpdateUserInfo());
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

	void UpdateUserInfo()
	{
		scoreText.text = _questManager.questProgress.globalScore.ToString();
		var userRank = FindUserPositionInRankings().ToString();
		var lastNumber = userRank[userRank.Length - 1];
		switch (lastNumber)
		{
			case '1':
				userRank += "st";
				break;
			case '2':
				userRank += "nd";
				break;
			case '3':
				userRank += "rd";
				break;
			default:
				userRank += "th";
				break;
		}
		positionInfoText.text = string.Format("The {0} in the ranking", userRank);
		gate2ScoreText.text = string.Format("+{0}",_questManager.questProgress.vrGameData.score);
		gate3ScoreText.text = string.Format("+{0}",_questManager.questProgress.globalScore - _questManager.questProgress.vrGameData.score);
	}

	int FindUserPositionInRankings()
	{
		var pos = 0;
		var i = 1;
		foreach (var pair in _questManager.QuestLeaderboardData.OrderByDescending(entry => entry.Value.globalScore))
		{
			if (pair.Key == firebaseData.currentUserUserId)
			{
				pos = i;
			}
			i++;
		}

		return pos;
	}

	IEnumerator LateUpdateUserInfo()
	{
		yield return new WaitForSeconds(1);
		UpdateUserInfo();
	}
}

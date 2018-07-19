﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRiddlesController : MonoBehaviour
{
	[SerializeField]
	Text _riddleText;

	[SerializeField]
	Button _scanButton;
	
	QuestManager _questManager;

	QuestRiddleData _currentRiddleData;
	
	void Awake()
	{
		Debug.Log("QuestRiddlesController.Awake");
		
		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();
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

	void OnEnable()
	{
		Debug.Log("QuestRiddlesController.OnEnable");

		UpdateRiddlesScreen();
	}

	void UpdateRiddlesScreen()
	{
		bool anyRiddles = false;
		
		var riddles = _questManager.QuestRiddles;
		
		for (int i = 0; i < riddles.Count; i++)
		{
			var riddleData = riddles[i];

			if (!riddleData.State)
			{
				_riddleText.text = riddleData.Description;
				_currentRiddleData = riddleData;
				anyRiddles = true;
				break;
			}
		}

		if (!anyRiddles)
		{
			_riddleText.text = "You completed all riddles!";
			_scanButton.gameObject.SetActive(false);
		}
	}

	public void OnScanButtonClicked()
	{
		Debug.Log("QuestRiddlesController.OnScanButtonClicked");

		_currentRiddleData.State = true;
		
		UpdateRiddlesScreen();
	}
}

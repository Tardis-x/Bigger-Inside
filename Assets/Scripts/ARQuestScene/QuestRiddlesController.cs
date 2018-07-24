using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRiddlesController : MonoBehaviour
{
	[SerializeField]
	Text _riddleText;

	[SerializeField]
	Button _scanButton;

	[SerializeField]
	Camera _mainCamera;
	[SerializeField]
	Camera _arCamera;
	
	QuestManager _questManager;

	string _currentRiddle;
	
	void Awake()
	{
		Debug.Log("QuestRiddlesController.Awake");
		
		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();

		Debug.Log("QuestRiddlesController.Awake - Camera count - " + Camera.allCameras.Length);
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

	public void UpdateRiddlesScreen()
	{
		bool anyRiddles = false;
		
		var riddlesProgress = _questManager.QuestRiddlesProgress;
		var riddlesData = _questManager.QuestRiddlesData;

		foreach (var riddle in riddlesProgress)
		{
			if (!riddle.Value)
			{
				_riddleText.text = riddlesData[riddle.Key].description;
				_currentRiddle = riddle.Key;
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
		
		_mainCamera.gameObject.SetActive(false);
		_arCamera.gameObject.SetActive(true);
		
		_questManager.CompleteRiddle(_currentRiddle, this);
	}
}

using System.Collections.Generic;
using System.IO;
using Firebase.Auth;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
	Dictionary<int, QuestRiddleData> _questRiddles;

	QuestUI _questUi;

	int _currentStep;
	

	public Dictionary<int, QuestRiddleData> QuestRiddles
	{
		get { return _questRiddles; }
	}

	void Awake()
	{
		Debug.Log("QuestManager.Awake");

		// obtain reference to object that represents quest UI
		UiReferenceInitialization();

		// initialize riddle data
		DummyQuestRiddleDataInitizalization();
	}

	void UiReferenceInitialization()
	{
		GameObject questCanvas = GameObject.Find("QuestCanvas");

		if (questCanvas != null)
		{
			_questUi = questCanvas.GetComponent<QuestUI>();

			if (_questUi == null)
			{
				Debug.LogError("Could not locate QuestUI component on " + questCanvas.name);
			}
		}
		else
		{
			Debug.LogError("Could not locate quest canvas object in current scene!");
		}
	}
	
	// TODO: Get rid of this method after real quest steps data will be available
	void DummyQuestRiddleDataInitizalization()
	{
		_questRiddles = new Dictionary<int, QuestRiddleData>();
		
		QuestRiddleData androidRiddle = new QuestRiddleData(0, "Android", "Popular mobile OS that is powered by Google?", RiddleMarkerType.Android);
		_questRiddles.Add(0, androidRiddle);

		QuestRiddleData angularRiddle = new QuestRiddleData(1, "Angular", "TypeScript-based open-source front-end web application platform?", RiddleMarkerType.Angular);
		_questRiddles.Add(1, angularRiddle);

		QuestRiddleData arcoreRiddle = new QuestRiddleData(2, "Arcore", "Software development kit developed by Google that allow for mixed reality applications to be built?", RiddleMarkerType.Arcore);
		_questRiddles.Add(2, arcoreRiddle);

		QuestRiddleData firebaseRiddle = new QuestRiddleData(3, "Firebase", "Mobile and web application development platform?", RiddleMarkerType.Firebase);
		_questRiddles.Add(3, firebaseRiddle);
	}
}
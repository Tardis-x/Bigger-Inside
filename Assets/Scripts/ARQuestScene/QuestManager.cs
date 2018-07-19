using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	Dictionary<int, QuestStepData> _questSteps;

	QuestUI _questUi;

	int _currentStep;

	void Awake()
	{
		Debug.Log("QuestManager.Awake()");

		// obtain reference to object that represents quest UI
		UiReferenceInitialization();
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
}
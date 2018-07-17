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

		// set up quest steps data
		// TODO: this is temporary solution - just to have some dummy data to work with
		DummyDataInitizalization();

		// determine current quest step
		// TODO: implement more sophisticated mechanism for determining current quest step
		
		// update application UI according to current quest step
		SetUpStep(0);
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
	void DummyDataInitizalization()
	{
		_questSteps = new Dictionary<int, QuestStepData>();

		QuestStepData welcomeStep;
		welcomeStep.number = 0;
		welcomeStep.name = "Welcome";
		welcomeStep.description = "This is welcome screen";
		welcomeStep.state = false;
		welcomeStep.type = QuestStepType.Welcome;

		_questSteps.Add(0, welcomeStep);

		QuestStepData photoStep;
		photoStep.number = 1;
		photoStep.name = "Photo";
		photoStep.description = "This is take photo screen";
		photoStep.state = false;
		photoStep.type = QuestStepType.Photo;

		_questSteps.Add(1, photoStep);

		QuestStepData arObjectStep;
		arObjectStep.number = 2;
		arObjectStep.name = "AR Object";
		arObjectStep.description = "This is AR object scan screen";
		arObjectStep.state = false;
		arObjectStep.type = QuestStepType.ArObject;

		_questSteps.Add(2, arObjectStep);

		QuestStepData finalStep;
		finalStep.number = 3;
		finalStep.name = "Final";
		finalStep.description = "This is final screen";
		finalStep.state = false;
		finalStep.type = QuestStepType.Final;

		_questSteps.Add(3, finalStep);
	}

	void SetUpStep(int stepNumber)
	{
		Debug.Log("QuestManager.SetUpStep() - current step: " + stepNumber);
		
		if (stepNumber < _questSteps.Count)
		{
			// update quest UI
			_questUi.UpdateQuestUi(_questSteps[stepNumber]);
		}
	}

	public void CompleteCurrentStep()
	{
		if (_currentStep < _questSteps.Count)
		{
			// mark current quest step as completed
			var currentQuestStepData = _questSteps[_currentStep];
			currentQuestStepData.state = true;
			_questSteps[_currentStep] = currentQuestStepData;

			// prepare next quest step
			++_currentStep;
			SetUpStep(_currentStep);
		}
		else
		{
			// TODO: After last quest step is completed application should return to main menu
			Debug.LogWarning("QuestManager.CompleteCurrentStep() - No more steps left!");
		}
	}
}
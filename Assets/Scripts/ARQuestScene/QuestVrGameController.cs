using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestVrGameController : MonoBehaviour 
{
	[SerializeField]
	Camera _mainCamera;
	[SerializeField]
	Camera _arCamera;
	
	[SerializeField]
	Text _statusText;
	[SerializeField]
	Text _descriptionText;
	[SerializeField]
	InputField _scoreInputField;
	[SerializeField]
	Button _scanButton;
	
	QuestManager _questManager;
	
	void Awake()
	{
		Debug.Log("QuestVrGameController.Awake");
		
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
		Debug.Log("QuestVrGameController.OnEnable");

		UpdateVrGameScreen();
	}

	public void UpdateVrGameScreen()
	{
		if (_questManager.questProgress.vrGameData.state)
		{
			_descriptionText.text = "";
			_statusText.gameObject.SetActive(true);
			_statusText.text = string.Format("You have completed VR game with score: {0} points!",_questManager.questProgress.vrGameData.gameScore);
			_scoreInputField.gameObject.SetActive(false);
			_scanButton.gameObject.SetActive(false);
		}
		else
		{
			_statusText.gameObject.SetActive(false);
			_scoreInputField.gameObject.SetActive(true);
			_scanButton.gameObject.SetActive(true);
		}
	}
	
	public void OnScanButtonClicked()
	{
		Debug.Log("QuestVrGameController.OnScanButtonClicked");
		
		_mainCamera.gameObject.SetActive(false);
		_arCamera.gameObject.SetActive(true);
	}
	
	public void OnImageScanned(string scannedMarker)
	{
		Debug.Log("QuestVrGameController.OnImageScanned");
		
		if (!_questManager.questProgress.vrGameData.state)
		{
			if (scannedMarker == "vrGame")
            {
            	_questManager.CompleteVrGame(Int32.Parse(_scoreInputField.text), this);
            	
            	_mainCamera.gameObject.SetActive(true);
            	_arCamera.gameObject.SetActive(false);
            }
		}
		
	}
}

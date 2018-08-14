using System;
using System.Collections;
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
	[SerializeField] 
	Text _scanStatusText;
	
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
            	_scanStatusText.color = Color.green;
	            _scanStatusText.text = "Congratulations! Step completed!\nLoading next step...";
	            StartCoroutine(CameraSwitchDelay());
            }
		}
	}
	
	IEnumerator CameraSwitchDelay()
	{
		//Hide panel's elements for better smoothness
		_scoreInputField.gameObject.SetActive(false);
		_scanButton.gameObject.SetActive(false);
		_statusText.text = "";
		_descriptionText.text = "";
		
		yield return new WaitForSeconds(3);
		_scanStatusText.text = "";
		//Switch cameras
		_mainCamera.gameObject.SetActive(true);
		_arCamera.gameObject.SetActive(false);
		
		_questManager.CompleteVrGame(Int32.Parse(_scoreInputField.text), this);
	}
}

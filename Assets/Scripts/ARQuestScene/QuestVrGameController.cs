using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestVrGameController : MonoBehaviour 
{
	[SerializeField]
	Camera _mainCamera;
	[SerializeField]
	Camera _arCamera;
	
	[SerializeField]
	InputField _ScoreInputField;
	
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
	
	public void OnScanButtonClicked()
	{
		Debug.Log("QuestVrGameController.OnScanButtonClicked");
		
		_mainCamera.gameObject.SetActive(false);
		_arCamera.gameObject.SetActive(true);
	}
	
	public void OnImageScanned(string scannedMarker)
	{
		Debug.Log("QuestVrGameController.OnImageScanned");

		if (scannedMarker == "vrGame")
		{
			_questManager.CompleteVrGame(0, this);
			
			_mainCamera.gameObject.SetActive(true);
			_arCamera.gameObject.SetActive(false);
		}
	}
}

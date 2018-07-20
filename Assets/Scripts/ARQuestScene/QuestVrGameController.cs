using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestVrGameController : MonoBehaviour 
{
	[SerializeField]
	Camera _mainCamera;
	[SerializeField]
	Camera _arCamera;
	
	public void OnScanButtonClicked()
	{
		Debug.Log("QuestVrGameController.OnScanButtonClicked");
		
		_mainCamera.gameObject.SetActive(false);
		_arCamera.gameObject.SetActive(true);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestArUI : MonoBehaviour 
{
	[SerializeField]
	Camera _mainCamera;
	[SerializeField]
	Camera _arCamera;
	
	public void OnBackButtonClicked()
	{
		Debug.Log("QuestArUI.OnBackButtonClicked");
		
		_mainCamera.gameObject.SetActive(true);
		_arCamera.gameObject.SetActive(false);
	}
}

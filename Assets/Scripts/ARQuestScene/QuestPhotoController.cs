using System.Collections;
using System.IO;
using UnityEngine;
using DeadMosquito.AndroidGoodies;
using DeadMosquito.AndroidGoodies.Internal;
using DeadMosquito.IosGoodies;
using Firebase.Auth;
using Firebase.Storage;
using ua.org.gdg.devfest;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class QuestPhotoController : MonoBehaviour
{
	QuestManager _questManager;
	string _photoComment;
	[SerializeField] Camera _mainCamera;
	[SerializeField] Camera _arCamera;
	[SerializeField] Text _scanStatusText;

	void Awake()
	{
		Debug.Log("QuestPhotoController.Awake");
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
		Debug.Log("QuestPhotoController.OnScanButtonClicked");

		_mainCamera.gameObject.SetActive(false);

		_arCamera.gameObject.SetActive(true);
	}

	public void OnImageScanned(string scannedMarker)
	{
		Debug.Log("QuestPhotoController.OnImageScanned");

		if (!_questManager.QuestProgress.PhotoData.State)
		{
			if (scannedMarker == "Photo")
			{
				_scanStatusText.color = Color.green;
				_scanStatusText.text = "Congratulations! Step completed!\nLoading next step...";
				StartCoroutine(CameraSwitchDelay());
			}
		}
	}

	IEnumerator CameraSwitchDelay()
	{
		yield return new WaitForSeconds(3);
		_scanStatusText.text = "";
		_mainCamera.gameObject.SetActive(true);
		_arCamera.gameObject.SetActive(false);
		_questManager.CompletePhoto();
	}
}
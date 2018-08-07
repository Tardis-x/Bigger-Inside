using System;
using System.Collections;
using System.Collections.Generic;
using DeadMosquito.AndroidGoodies;
using UnityEngine;
using UnityEngine.UI;

public class QuestRiddlesController : MonoBehaviour
{
	[SerializeField]
	Text _riddleText;
	
	[SerializeField]
	Text _descriptionText;
	
	[SerializeField]
	Text _scanStatusText;

	[SerializeField]
	Button _scanButton;
	
	[SerializeField]
	Button _submitButton;

	[SerializeField] 
	InputField _inputField;
	
	[SerializeField]
	Image _riddleImageHolder;

	[SerializeField]
	Camera _mainCamera;
	
	[SerializeField]
	Camera _arCamera;
	
	QuestManager _questManager;

	string _currentRiddle;

	bool _isWrongAnswerSubmitted;
	
	bool _isCorrectAnswerSubmitted;
	
	ColorBlock _colors;
	
	Color _color;
	
	void Awake()
	{
		Debug.Log("QuestRiddlesController.Awake");
		
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
		Debug.Log("QuestRiddlesController.OnEnable");

		UpdateRiddlesScreen();
	}

	void FixedUpdate()
	{
		if (_isWrongAnswerSubmitted)
		{
			_color = _inputField.image.color;
			_color.g += 0.02f;
			_color.b += 0.02f;
			_inputField.image.color = _color;
		}
		else if (_isCorrectAnswerSubmitted)
		{
			_color = _inputField.image.color;
			_color.r += 0.02f;
			_color.b += 0.02f;
			_inputField.image.color = _color;
		}
	}
	public void UpdateRiddlesScreen()
	{
		_questManager.ReadRiddleDataFromQuestProgress();
		bool anyRiddles = false;
		
		foreach (KeyValuePair<string, QuestRiddleDataFull> riddle in _questManager.QuestRiddlesDataFull)
		{
			if (!riddle.Value.isCompleted)
			{
				_currentRiddle = riddle.Key;
				if (riddle.Value.isText)
				{
					_descriptionText.text = "Type the correct answer and press Submit";
					_riddleText.text = riddle.Value.description;
					_scanButton.gameObject.SetActive(false);
					_submitButton.gameObject.SetActive(true);
					_inputField.gameObject.SetActive(true);
					//Making Image field invisible
					Color color = _riddleImageHolder.color;
					color.a = 0;
					_riddleImageHolder.color = color;
				}
				else if(!riddle.Value.isText)
				{
					_riddleText.text = "";
					_descriptionText.text = "Guess the technology, shown on the picture, and find the right mark";
					_riddleImageHolder.sprite = Sprite.Create(riddle.Value.texture, new Rect(0.0f, 0.0f, 
						riddle.Value.texture.width, riddle.Value.texture.height), new Vector2(0f, 0f), 100.0f);
					_submitButton.gameObject.SetActive(false);
					_inputField.gameObject.SetActive(false);
					_scanButton.gameObject.SetActive(true);
					//Making Image field visible
					Color color = _riddleImageHolder.color;
					color.a = 1;
					_riddleImageHolder.color = color;
				}
				anyRiddles = true;
				break;
			}
		}
		if (!anyRiddles)
		{
			_descriptionText.text = "You completed all riddles!";
			
			//Mark completion of third quest step for UI
			_questManager.CompleteAllRiddles();
			
			_riddleImageHolder.gameObject.SetActive(false);
			_inputField.gameObject.SetActive(false);
			_submitButton.gameObject.SetActive(false);
			_scanButton.gameObject.SetActive(false);
		}
	}

	public void OnScanButtonClicked()
	{
		Debug.Log("QuestRiddlesController.OnScanButtonClicked");
		
		_mainCamera.gameObject.SetActive(false);
		_arCamera.gameObject.SetActive(true);
	}

	public void OnImageScanned(string scannedMarker)
	{
		Debug.Log("QuestRiddlesController.OnImageScanned:" + scannedMarker);
		scannedMarker = scannedMarker.ToLower().Replace(" ", String.Empty);
		string answer = _currentRiddle.ToLower().Replace(" ", String.Empty);
		if (!_questManager.questProgress.riddlesData[_currentRiddle].isCompleted)
		{
			if (scannedMarker == answer)
			{
				Debug.Log("QuestRiddlesController.OnImageScanned: strings are equal.");
				_questManager.CompleteRiddle(_currentRiddle, this);
				_scanStatusText.color = Color.green;
				_scanStatusText.text = "You scanned the correct marker!";
				StartCoroutine(CameraSwitchDelay());
			}
			else
			{
				Debug.Log("QuestRiddlesController.OnImageScanned: strings are not equal.");
				_scanStatusText.color = Color.red;
				_scanStatusText.text = "You scanned the wrong marker!";
				StartCoroutine(StatusTextDelay());
			}
		}
	}

	public void OnSubmitButtonClicked()
	{
		Debug.Log("QuestRiddlesController.OnAnswerSubmitted");
		
		string answer = _inputField.text;
		answer = answer.ToLower().Replace(" ", String.Empty);
		string correctAnswer = _currentRiddle.ToLower().Replace(" ", String.Empty);
		if (answer == correctAnswer)
		{
			_inputField.text = "";
			_inputField.image.color = Color.green;
			_isCorrectAnswerSubmitted = true;
			StartCoroutine(CorrectAnswerHighlight());
			_questManager.CompleteRiddle(_currentRiddle, this);
		}
		else
		{
			_inputField.image.color = Color.red;
			_isWrongAnswerSubmitted = true;
			StartCoroutine(WrongAnswerHighlight());
		}
	}

	IEnumerator CameraSwitchDelay()
	{
		yield return new WaitForSeconds(3);
		_scanStatusText.text = "";
		_mainCamera.gameObject.SetActive(true);
		_arCamera.gameObject.SetActive(false);
	}
	IEnumerator StatusTextDelay()
	{
		yield return new WaitForSeconds(3);
		_scanStatusText.text = "";
	}
	
	IEnumerator WrongAnswerHighlight()
	{
		yield return new WaitForSeconds(1);
		_isWrongAnswerSubmitted = false;
		_inputField.image.color = Color.white;
	}
	IEnumerator CorrectAnswerHighlight()
	{
		yield return new WaitForSeconds(1);
		_isCorrectAnswerSubmitted = false;
		_inputField.image.color = Color.white;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGoogleColorsController : MonoBehaviour
{
	readonly string _answerCheck = "314321";
	
	QuestManager _questManager;

	[SerializeField]
	GameObject _colorsPanel;

	[SerializeField] 
	Button _letterButton1;
	
	[SerializeField] 
	Button _letterButton2;
	
	[SerializeField] 
	Button _letterButton3;
	
	[SerializeField] 
	Button _letterButton4;
	
	[SerializeField] 
	Button _letterButton5;
	
	[SerializeField] 
	Button _letterButton6;

	[SerializeField] 
	Button _submitResultButton;
	
	Dictionary<int, Button> _buttons;
		
	Dictionary<int, Color> _colors;
	
	int[] _userColors = {0,0,0,0,0,0};
	
	int _buttonIndex;
	
	bool _isWrongAnswerSubmitted;

	Color _color;
	
	void Awake()
	{
		Debug.Log("QuestUI.Awake()");

		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();
		_colors = new Dictionary<int, Color>
		{
			{1, new Color(204/256f, 45/256f, 45/256f)},
			{2, new Color(42/256f, 176/256f, 34/256f)},
			{3, new Color(55/256f, 74/256f, 190/256f)},
			{4, new Color(202/256f, 199/256f, 37/256f)}
		};
		_buttons = new Dictionary<int, Button>
		{
			{0, _letterButton1},
			{1, _letterButton2},
			{2, _letterButton3},
			{3, _letterButton4},
			{4, _letterButton5},
			{5, _letterButton6}
		};
	}
	
	void FixedUpdate()
     	{
     		if (_isWrongAnswerSubmitted)
     		{
     			
     		}
     	}

	void QuestManagerReferenceInitialization()
	{
		var questManagerTemp = GameObject.Find("QuestManager");

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

	void UpdateLetterColor(int colorIndex)
	{
		_userColors[_buttonIndex] = colorIndex;
		_buttons[_buttonIndex].GetComponentInChildren<Text>().color = _colors[colorIndex];
	}
	
	public void OnSubmitColorsButtonClicked()
	{
		var checkString = "";
		foreach (var x in _userColors)
		{
			checkString += x.ToString();
		}
		if (checkString == _answerCheck)
		{
			_questManager.CompleteGoogleColorsRiddle();
		}
		else
		{
			_isWrongAnswerSubmitted = true;
			StartCoroutine(WrongAnswerHighlight());
		}
	}

	public void OnGoogleLetterButtonClicked(int buttonIndex)
	{
		_colorsPanel.SetActive(true);
		_buttonIndex = buttonIndex;
		Debug.Log("Button index is: " + _buttonIndex);
	}

	public void OnGoogleColorButtonClicked(int colorIndex)
	{
		_colorsPanel.SetActive(false);
		Debug.Log("Selected color index is: " + colorIndex);
		UpdateLetterColor(colorIndex);
	}
	
	IEnumerator WrongAnswerHighlight()
	{
		yield return new WaitForSeconds(1);
		_isWrongAnswerSubmitted = false;
		
	}
}

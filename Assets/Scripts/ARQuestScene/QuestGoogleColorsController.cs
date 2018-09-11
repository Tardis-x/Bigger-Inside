using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGoogleColorsController : MonoBehaviour
{
	const string AnswerCheck = "314321";

	QuestManager _questManager;

	[SerializeField]
	GameObject _colorsPanel;
	
	[SerializeField]
	GameObject _wellDonePanel;

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

	int[] _userColors = {0, 0, 0, 0, 0, 0};

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
			{1, new Color(239 / 255f, 68 / 255f, 56 / 255f)},
			{2, new Color(73 / 255f, 176 / 256f, 80 / 255f)},
			{3, new Color(70 / 255f, 143 / 255f, 204 / 255f)},
			{4, new Color(250 / 255f, 237 / 255f, 56 / 255f)},
			{5, new Color(248 / 255f, 152 / 255f, 28 / 255f)},
			{6, new Color(144 / 255f, 62 / 255f, 152 / 255f)}
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
			_color.g += 0.02f;
			_color.b += 0.02f;
			_submitResultButton.GetComponentInChildren<Text>().color = _color;
			_submitResultButton.image.color = _color;
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

		if (checkString == AnswerCheck)
		{
			ShowWellDonePanel();
		}
		else
		{
			_isWrongAnswerSubmitted = true;
			StopAllCoroutines();
			_color = Color.red;
			_submitResultButton.GetComponentInChildren<Text>().color = _color;
			_submitResultButton.image.color = _color;
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

	public void OnProceedButtonClicked()
	{
		_wellDonePanel.gameObject.SetActive(false);
		_questManager.CompleteGoogleColorsRiddle();
	}

	public void ShowWellDonePanel()
	{
		_wellDonePanel.gameObject.SetActive(true);
	}

	IEnumerator WrongAnswerHighlight()
	{
		yield return new WaitForSeconds(1);
		_isWrongAnswerSubmitted = false;
		_color = Color.white;
		_submitResultButton.GetComponentInChildren<Text>().color = _color;
		_submitResultButton.image.color = _color;
	}
}
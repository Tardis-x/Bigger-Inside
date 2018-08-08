using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DeadMosquito.AndroidGoodies;
using ua.org.gdg.devfest;

public class QuestUI : MonoBehaviour
{
	[SerializeField] Image _fadeImage;
	[SerializeField] GameObject _photoPanel;
	[SerializeField] GameObject _vrGamePanel;
	[SerializeField] GameObject _riddlesPanel;
	[SerializeField] GameObject _leaderBoardPanel;
	[SerializeField] GameObject _googleColorsPanel;
	[SerializeField] GameObject _colorsPanel;
	[SerializeField] GameObject _infoPanel;
	[SerializeField] Text _shortDescription;
	[SerializeField] Text _longDescription;
	[SerializeField] Button _changeInfoButton;
	[SerializeField] Button _proceedButton;

	public GameObject usersList;

	GameObject _activePanel;

	QuestManager _questManager;


	void Awake()
	{
		Debug.Log("QuestUI.Awake()");

		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();
		//Show Info panel with custom content
		
		_infoPanel.SetActive(true);
		_proceedButton.gameObject.SetActive(false);
		_changeInfoButton.gameObject.SetActive(true);

		// hide quest screens with tasks
		_photoPanel.SetActive(false);
		_vrGamePanel.SetActive(false);
		_riddlesPanel.SetActive(false);
		_googleColorsPanel.SetActive(false);
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

	public void OnMainMenuButtonClicked()
	{
		Debug.Log("QuestUI.OnMainMenuButtonClicked");

		SceneManager.LoadScene("MenuScene");
	}

	public void OnBackButtonClicked()
	{
		Debug.Log("QuestUI.OnBackButtonClicked");

		_activePanel.SetActive(false);
		_activePanel = _infoPanel;
		_infoPanel.SetActive(true);
	}

	public void FadeQuestScreenIn()
	{
		_fadeImage.gameObject.SetActive(true);
	}

	public void FadeScreenOut()
	{
		_fadeImage.gameObject.SetActive(false);
	}

	public void OnBackFromLeaderBoardButtonClicked()
	{
		Debug.Log("QuestUI.OnBackButtonClicked");

		_activePanel.SetActive(false);
		_activePanel = _infoPanel;
		_infoPanel.SetActive(true);
		foreach (Transform child in usersList.transform)
		{
			Destroy(child.gameObject);
		}
	}

	public void OnChangeInfoButtonClicked()
	{
		//Disable previous panels
		_photoPanel.SetActive(false);
		_vrGamePanel.SetActive(false);
		_riddlesPanel.SetActive(false);
		_googleColorsPanel.SetActive(false);
		//Switch the buttons
		_changeInfoButton.gameObject.SetActive(false);
		_proceedButton.gameObject.SetActive(true);
		//Google Riddle Info
		if (!_questManager.questProgress.isGoogleColorsCompleted)
		{
			ShowInfoPanel("Test of worthiness",
				"In order to begin the Quest you have to solve a riddle to prove your capability for the future tasks");
		}
		//Photo Panel
		else if (!_questManager.questProgress.photoData.state)
		{
			ShowInfoPanel("First task is Social Gate",
				"You have to:\n- Take a photo with a speaker;\n" +
				"- Take a crazy photo with your friend(s) in front of Press Wall;\n" +
				"- Share those photos with hashtags #dfua, #devfest and #dfua_gate1;\n" +
				"- Show Quest guys your post(s) for confirmation.");
		}
		//VR Game Panel
		else if (!_questManager.questProgress.vrGameData.state)
		{
			ShowInfoPanel("Second task is Virtual Reality Gate",
				"You have to:\n" +
				"- Find a stand with Beat Saber Game demo;\n" +
				"- Play the game and achieve a minimum of 50000 points;\n" +
				"- Show Quest Guys your result for confirmation.");
		}
		//Riddles Panel
		else if (!_questManager.questProgress.allRiddlesCompleted)
		{
			_questManager.CheckIfQuestIsActivated();
			if (_questManager.isQuestActivated)
			{
				ShowInfoPanel("Third task is Knowledge Gate",
					"You will have to answer different questions about Google " +
					"and its technologies and find the cyphered technologies logos, " +
					"hidden in Planeta Kino.\n Good luck, friend!");
			}
			else
			{
				ShowInfoPanel("Third task is Knowledge Gate",
					"Will become available tomorrow!");
				_proceedButton.gameObject.SetActive(false);
				_changeInfoButton.gameObject.SetActive(false);
			}
		}
		//Leaderboard Panel
		else
		{
			ShowInfoPanel("Congratulations!",
				"You have completed all the steps in the quest. Press the button to look at the Leaderboards.");
			_proceedButton.GetComponentInChildren<Text>().text = "Show Leaderboard";
		}
	}

	public void OnProceedButtonClicked()
	{
		//Google Riddle Panel
		if (!_questManager.questProgress.isGoogleColorsCompleted)
		{
			_activePanel = _googleColorsPanel;
			_googleColorsPanel.SetActive(true);
		}
		//Photo Panel
		else if (!_questManager.questProgress.photoData.state)
		{
			_activePanel = _photoPanel;
			_photoPanel.SetActive(true);
		}
		//VR Game Panel
		else if (!_questManager.questProgress.vrGameData.state)
		{
			_activePanel = _vrGamePanel;
			_vrGamePanel.SetActive(true);
		}
		//Riddles Panel
		else if (!_questManager.questProgress.allRiddlesCompleted)
		{
			_questManager.CheckIfQuestIsActivated();
			if (_questManager.isQuestActivated)
			{
				Debug.Log("Quest is activated.");
				_activePanel = _riddlesPanel;
				_riddlesPanel.SetActive(true);
			}
			else
			{
				Debug.Log("Quest is not activated.");
#if UNITY_ANDROID
				AGUIMisc.ShowToast("Riddles will be available tomorrow.");
#endif
			}
		}
		//Leaderboard Panel
		else
		{
			_activePanel = _leaderBoardPanel;
			_leaderBoardPanel.SetActive(true);
		}
		//Switch the buttons
		_changeInfoButton.gameObject.SetActive(true);
		_proceedButton.gameObject.SetActive(false);
		_infoPanel.SetActive(false);
	}

	public void ShowInfoPanel(string shortDescription, string longDescription)
	{
		_infoPanel.SetActive(true);
		_shortDescription.text = shortDescription;
		_longDescription.text = longDescription;
	}
}
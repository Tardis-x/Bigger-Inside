using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DeadMosquito.AndroidGoodies;

public class QuestUI : MonoBehaviour
{
	[SerializeField] Image _fadeImage;
	[SerializeField] GameObject _photoPanel;
	[SerializeField] GameObject _vrGamePanel;
	[SerializeField] GameObject _riddlesPanel;
	[SerializeField] GameObject _leaderBoardPanel;
	[SerializeField] GameObject _googleColorsPanel;
	[SerializeField] GameObject _infoPanel;
	[SerializeField] GameObject _userScorePanel;
	[SerializeField] GameObject _congratzPanel;
	[SerializeField] Text _shortDescription;
	[SerializeField] Text _longDescription;
	[SerializeField] Button _changeInfoButton;
	[SerializeField] Button _proceedButton;

	public GameObject usersList;
	public QuestFirebaseData firebaseData;
	
	public Text questStepScore;
	public Image step1CongratzImage;
	public Image step2CongratzImage;
	public Image step3CongratzImage;
	public Button leaderboardButton;
	public Button nextButton;
	public Text congratzTitle;

	GameObject _activePanel;

	QuestManager _questManager;
	
	void Awake()
	{
		Debug.Log("QuestUI.Awake()");

		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();
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
		DontDestroyOnLoad(firebaseData);
		SceneManager.LoadScene("MenuScene");
	}

	public void OnBackButtonClicked()
	{
		Debug.Log("QuestUI.OnBackButtonClicked");

		_activePanel.SetActive(false);
		_activePanel = _infoPanel;
		_infoPanel.SetActive(true);
		_proceedButton.gameObject.SetActive(true);
		_changeInfoButton.gameObject.SetActive(false);
		_userScorePanel.gameObject.SetActive(false);
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
		
		foreach (Transform child in usersList.transform)
		{
			Destroy(child.gameObject);
		}
		
		OnChangeInfoButtonClicked();
	}

	public void OnChangeInfoButtonClicked()
	{
		//Disable previous panels
		_activePanel.SetActive(false);
		_activePanel = _infoPanel;
		//Google Riddle Info
		if (!_questManager.QuestProgress.isGoogleColorsCompleted)
		{
			ShowInfoPanel("Test of worthiness",
				"In order to begin the Quest you have to solve a riddle to prove your capability for the future tasks");
			//Switch the buttons
			_changeInfoButton.gameObject.SetActive(false);
			_proceedButton.gameObject.SetActive(true);
		}
		//Photo Panel
		else if (!_questManager.QuestProgress.PhotoData.State)
		{
			ShowInfoPanel("Social part",
				"To complete this part you have to:\n- Take a photo with a speaker;\n" +
				"- Take a crazy photo with your friend(s) in front of Press Wall;\n" +
				"- Share those photos with hashtags #DevFest18, #dfua, #quest as public space in Facebook, Twitter or Instagram;\n" +
				"- Scan a special marker from Quest guys");
			//Switch the buttons
			_changeInfoButton.gameObject.SetActive(false);
			_proceedButton.gameObject.SetActive(true);
		}
		//VR Game Panel
		else if (!_questManager.QuestProgress.VrGameData.State)
		{
			ShowInfoPanel("Virtual reality part",
				"To complete this part:\n" +
				"- Find HTC Vive demo;\n" +
				"- Play Beast Saber and earn minimum of 50000 points;\n" +
				"- Scan a special marker from Quest guys");
			//Switch the buttons
			_changeInfoButton.gameObject.SetActive(false);
			_proceedButton.gameObject.SetActive(true);
		}
		//Riddles Panel
		else if (!_questManager.QuestProgress.allRiddlesCompleted)
		{
			_questManager.CheckIfQuestIsActivated();
			if (_questManager.isQuestActivated)
			{
				ShowInfoPanel("Wisdom part", "Solve the riddles and find the right answers");
				//Switch the buttons
				_changeInfoButton.gameObject.SetActive(false);
				_proceedButton.gameObject.SetActive(true);
			}
			else
			{
				ShowInfoPanel("Third task is Knowledge Gate",
					"Will become available tomorrow!");
				_proceedButton.gameObject.SetActive(false);
				_changeInfoButton.gameObject.SetActive(false);
			}
		}
		//User Score Panel
		else
		{
			_activePanel.gameObject.SetActive(false);
			_activePanel = _userScorePanel;
			_userScorePanel.gameObject.SetActive(true);
			//Switch the buttons
			_changeInfoButton.gameObject.SetActive(false);
			_proceedButton.gameObject.SetActive(true);
		}
	}

	public void OnProceedButtonClicked()
	{
		//Google Riddle Panel
		if (!_questManager.QuestProgress.isGoogleColorsCompleted)
		{
			_activePanel = _googleColorsPanel;
			_googleColorsPanel.SetActive(true);
		}
		//Photo Panel
		else if (!_questManager.QuestProgress.PhotoData.State)
		{
			_activePanel = _photoPanel;
			_photoPanel.SetActive(true);
		}
		//VR Game Panel
		else if (!_questManager.QuestProgress.VrGameData.State)
		{
			_activePanel = _vrGamePanel;
			_vrGamePanel.SetActive(true);
		}
		//Riddles Panel
		else if (!_questManager.QuestProgress.allRiddlesCompleted)
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
			OnLeaderboardButtonClicked();
		}
		//Switch the buttons
		_changeInfoButton.gameObject.SetActive(true);
		_proceedButton.gameObject.SetActive(false);
		
		_infoPanel.SetActive(false);
	}

	public void OnLeaderboardButtonClicked()
	{
		_activePanel.SetActive(false);
		_activePanel = _leaderBoardPanel;
		_leaderBoardPanel.SetActive(true);
	}

	public void ShowInfoPanel(string shortDescription, string longDescription)
	{
		if (_activePanel != null)
		{
			_activePanel.SetActive(false);
		}
		_activePanel = _infoPanel;
		_infoPanel.SetActive(true);
		_shortDescription.text = shortDescription;
		_longDescription.text = longDescription;
		_proceedButton.gameObject.SetActive(false);
		_changeInfoButton.gameObject.SetActive(true);
	}

	public void ShowUserScorePanel()
	{
		if (_activePanel)
		{
			_activePanel.gameObject.SetActive(false);
		}
		_activePanel = _userScorePanel;
		_userScorePanel.SetActive(true);
	}

	public void ShowCongratzPanel(int score, int questStepNumber)
	{
		_activePanel.gameObject.SetActive(false);
		_activePanel = _congratzPanel;
		_congratzPanel.gameObject.SetActive(true);
		
		questStepScore.text = "+" + score;
		if (questStepNumber == 1)
		{
			step1CongratzImage.gameObject.SetActive(true);
			step2CongratzImage.gameObject.SetActive(false);
			step3CongratzImage.gameObject.SetActive(false);
		}
		else if (questStepNumber == 2)
		{
			step1CongratzImage.gameObject.SetActive(false);
			step2CongratzImage.gameObject.SetActive(true);
			step3CongratzImage.gameObject.SetActive(false);
		}
		else if (questStepNumber == 3)
		{
			if (!_questManager.QuestProgress.allRiddlesCompleted)
			{
				_activePanel.gameObject.SetActive(false);
				_questManager.CompleteAllRiddles();
			}
			
			step1CongratzImage.gameObject.SetActive(false);
			step2CongratzImage.gameObject.SetActive(false);
			step3CongratzImage.gameObject.SetActive(true);
			congratzTitle.text += "\nYou have completed the Quest!";
			nextButton.gameObject.SetActive(false);
			var pos = new Vector3(0, 200, -20);
			leaderboardButton.transform.position = pos;
		}
	}
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DeadMosquito.AndroidGoodies;
using ua.org.gdg.devfest;

public class QuestUI : MonoBehaviour
{
	[SerializeField] Image _fadeImage;
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
	[SerializeField] Button _scanPhotoMarkerButton;
	[SerializeField] private UserAvatarManager _avatarManager;

	public Image infoPanelSwordImage;
	public Image infoPanelSelfieImage;
	public Image infoPanelVrImage;
	public Image infoPanelKnowledgeImage;
	
	public GameObject usersList;
	public QuestFirebaseData firebaseData;
	
	public Text questStepScore;
	public Image step1CongratzImage;
	public Image step2CongratzImage;
	public Image step3CongratzImage;
	public Button nextButton;
	public Button leaderboardButton;
	public Button hiddenLeaderboardButton;
	public Text congratzTitle;
	
	QuestManager _questManager;
	
	void Awake()
	{
		Debug.Log("QuestUI.Awake()");

		// obtain reference to object that represents quest manager
		QuestManagerReferenceInitialization();
		
	}

	private void Start()
	{
		_avatarManager.SetUserAvatar();
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

		DisableAllPanels();
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
		DisableAllPanels();
		//Google Riddle Info
		if (!_questManager.QuestProgress.isGoogleColorsCompleted)
		{
			ShowInfoPanel("Test of worthiness",
				"In order to begin the Quest you have to solve a riddle to prove your capability for the future tasks", 0);
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
				"- Scan a special marker from Quest guys", 1);
			//Switch the buttons
			_changeInfoButton.gameObject.SetActive(false);
			_proceedButton.gameObject.SetActive(false);
			_scanPhotoMarkerButton.gameObject.SetActive(true);
		}
		//VR Game Panel
		else if (!_questManager.QuestProgress.VrGameData.State)
		{
			ShowInfoPanel("Virtual reality part",
				"To complete this part:\n" +
				"- Find HTC Vive demo;\n" +
				"- Play Beast Saber and earn minimum of 50000 points;\n" +
				"- Scan a special marker from Quest guys", 2);
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
				ShowInfoPanel("Wisdom part", "Solve the riddles and find the right answers", 3);
				//Switch the buttons
				_changeInfoButton.gameObject.SetActive(false);
				_proceedButton.gameObject.SetActive(true);
			}
			else
			{
				ShowInfoPanel("Third task is Knowledge Gate",
					"Will become available tomorrow!", 3);
				_proceedButton.gameObject.SetActive(false);
				_changeInfoButton.gameObject.SetActive(false);
			}
		}
		//User Score Panel
		else
		{
			_userScorePanel.gameObject.SetActive(true);
			//Switch the buttons
			_changeInfoButton.gameObject.SetActive(false);
			_proceedButton.gameObject.SetActive(true);
			_scanPhotoMarkerButton.gameObject.SetActive(false);
		}
	}

	public void OnProceedButtonClicked()
	{
		DisableAllPanels();
		//Google Riddle Panel
		if (!_questManager.QuestProgress.isGoogleColorsCompleted)
		{
			_googleColorsPanel.SetActive(true);
		}
		//VR Game Panel
		else if (!_questManager.QuestProgress.VrGameData.State)
		{
			_vrGamePanel.SetActive(true);
		}
		//Riddles Panel
		else if (!_questManager.QuestProgress.allRiddlesCompleted)
		{
			_questManager.CheckIfQuestIsActivated();
			if (_questManager.isQuestActivated)
			{
				Debug.Log("Quest is activated.");
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
		_scanPhotoMarkerButton.gameObject.SetActive(false);
	}

	public void OnLeaderboardButtonClicked()
	{
		DisableAllPanels();
		_leaderBoardPanel.SetActive(true);
	}

	public void ShowInfoPanel(string shortDescription, string longDescription, int i)
	{
		DisableAllPanels();
		EnableCorrectInfoImage(i);
		
		_infoPanel.SetActive(true);
		_shortDescription.text = shortDescription;
		_longDescription.text = longDescription;
		_proceedButton.gameObject.SetActive(false);
		_changeInfoButton.gameObject.SetActive(true);
		_scanPhotoMarkerButton.gameObject.SetActive(false);
	}

	public void DisableAllInfoPanelImages()
	{
		infoPanelSwordImage.gameObject.SetActive(false);
		infoPanelSelfieImage.gameObject.SetActive(false);
		infoPanelVrImage.gameObject.SetActive(false);
		infoPanelKnowledgeImage.gameObject.SetActive(false);
	}

	public void EnableCorrectInfoImage(int stepNumber)
	{
		DisableAllInfoPanelImages();
		switch (stepNumber)
		{
			case 0:
			{
				infoPanelSwordImage.gameObject.SetActive(true);
				break;
			}
			case 1:
			{
				infoPanelSelfieImage.gameObject.SetActive(true);
				break;
			}
			case 2:
			{
				infoPanelVrImage.gameObject.SetActive(true);
				break;
			}
			case 3:
			{
				infoPanelKnowledgeImage.gameObject.SetActive(true);
				break;
			}
		}
	}
	
	public void ShowUserScorePanel()
	{
		DisableAllPanels();
		_userScorePanel.SetActive(true);
	}

	public void ShowCongratzPanel(int score, int questStepNumber)
	{
		DisableAllPanels();
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
			step1CongratzImage.gameObject.SetActive(false);
			step2CongratzImage.gameObject.SetActive(false);
			step3CongratzImage.gameObject.SetActive(true);
			congratzTitle.text += "\nYou have completed Quest!";
			nextButton.gameObject.SetActive(false);
			leaderboardButton.gameObject.SetActive(false);
			hiddenLeaderboardButton.gameObject.SetActive(true);
		}
	}

	public void DisableAllPanels()
	{
		_vrGamePanel.gameObject.SetActive(false);
		_riddlesPanel.gameObject.SetActive(false);
		_leaderBoardPanel.gameObject.SetActive(false);
		_googleColorsPanel.gameObject.SetActive(false);
		_infoPanel.gameObject.SetActive(false);
		_userScorePanel.gameObject.SetActive(false);
		_congratzPanel.gameObject.SetActive(false);
	}
}
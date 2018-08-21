using System;
using System.Collections;
using System.Collections.Generic;
using DeadMosquito.AndroidGoodies;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
	const int maxRiddleScore = 5000;
	const int maxVRGameScore = 40000;

	QuestUI _questUi;
	DatabaseReference _database;
	FirebaseAuth _auth;
	public QuestProgress questProgress { get; set; }

	public Dictionary<string, QuestLeaderboardEntry> QuestLeaderboardData { get; set; }

	public Dictionary<string, QuestRiddleDataFull> QuestRiddlesDataFull { get; set; }
	public bool isQuestActivated;
	int _timesCompleted;
	public string currentUserUserId;
	public Texture2D[] riddleImages;
	public Uri userPhotoUrl;
	public Image userPhotoImage;

	void Awake()
	{
		Debug.Log("QuestManager.Awake");
		// Get the root reference location of the database.
		_auth = FirebaseAuth.DefaultInstance;
		Debug.Log("QuestManager.Awake.Auth");
		_database = FirebaseDatabase.DefaultInstance.RootReference;
		Debug.Log("QuestManager.Awake:Database");
		currentUserUserId = _auth.CurrentUser.DisplayName;
		Debug.Log("QuestManager.Awake.Username");
		userPhotoUrl = _auth.CurrentUser.PhotoUrl;
		Debug.Log("QuestManager.Awake.PhotoURL");
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hoverboard-v2-dev.firebaseio.com/");
		Debug.Log("QuestManager.Awake.URL");
		// obtain reference to object that represents quest UI
		UiReferenceInitialization();
		Debug.Log("QuestManager.Awake.UI");
		// initialize riddle data
		RiddleDataInitizalization();
		Debug.Log("QuestManager.Awake.Riddle");
		//Initialize Leaderboard
		LeaderBoardInitialization();
		Debug.Log("QuestManager.Awake.Leaderboard");
	}
	
	void Start()
	{
		Debug.Log("QuestManager.Start");
		
#if UNITY_ANDROID
		var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait", "Updating Quest Data...", AGDialogTheme.Dark);
		spinner.Show();
#endif
		_questUi.FadeQuestScreenIn();
		_database.Child("users").Child(currentUserUserId).GetValueAsync().ContinueWith(readTask => {
			if (readTask.Result == null)
			{
				var json = JsonConvert.SerializeObject(questProgress);
				_database.Child("users").Child(currentUserUserId)
					.SetRawJsonValueAsync(json).ContinueWith(writeTask => {
						if (writeTask.IsFaulted)
						{
							Debug.LogError("QuestManager: Failed to write default quest data to firebase realtime database!");
							Debug.LogError("Error message: " + writeTask.Exception.Message);
						}
						else if (writeTask.IsCompleted)
						{
							Debug.Log("QuestManager: Default quest data was successfully set up!");
							_questUi.FadeScreenOut();
						}
#if UNITY_ANDROID
						spinner.Dismiss();
#endif
						Debug.Log("Default info screen");
						_questUi.ShowInfoPanel("Welcome to the DevFest Quest Adventure!",
							"You will have to complete different tasks in order to proceed with the Quest. Are you ready?");
					});
			}
			else
			{
				if (readTask.IsFaulted) 
				{
					Debug.LogError("QuestManager: Failed to retrieve quest data from firebase realtime database!");
					Debug.LogError("Error message: " + readTask.Exception.Message);
					
					_questUi.FadeScreenOut();
				}
				else if (readTask.IsCompleted) 
				{
					// retrieve user quest progress
					var snapshot = readTask.Result;
					questProgress = JsonConvert.DeserializeObject<QuestProgress>(snapshot.GetRawJsonValue());
					
					Debug.Log("QuestManager: Quest data was successfully set up!");
					_questUi.FadeScreenOut();
					
					Debug.Log("Updating Info screen");
					if (questProgress.allRiddlesCompleted)
					{
						_questUi.ShowUserScorePanel();
					}
					else
					{
						Debug.Log("Default info screen");
						_questUi.ShowInfoPanel("Welcome to the DevFest Quest Adventure!",
							"You will have to complete different tasks in order to proceed with the Quest. Are you ready?");
					}
				}
#if UNITY_ANDROID
				spinner.Dismiss();
#endif
			}
			//Check if Quest is activated
			CheckIfQuestIsActivated();
		});
		UpdateUserScoreInLeaderBoard();
		StartCoroutine(LoadUserImageFromUrl());
	}

	void UiReferenceInitialization()
	{
		GameObject questCanvas = GameObject.Find("QuestCanvas");

		if (questCanvas != null)
		{
			_questUi = questCanvas.GetComponent<QuestUI>();

			if (_questUi == null)
			{
				Debug.LogError("Could not locate QuestUI component on " + questCanvas.name);
			}
		}
		else
		{
			Debug.LogError("Could not locate quest canvas object in current scene!");
		}
	}
	
	void RiddleDataInitizalization()
	{
		questProgress = new QuestProgress {userPhotoUrl = userPhotoUrl};

		QuestRiddlesDataFull = new Dictionary<string, QuestRiddleDataFull>();
		Debug.Log("RiddleDataInitialization");
		var riddle1 = new QuestRiddleDataFull(true, "How are new comers called in Google?");
		QuestRiddlesDataFull.Add("Noogler", riddle1);
		var riddle5 = new QuestRiddleDataFull(false, riddleImages[0]);
        		QuestRiddlesDataFull.Add("Angular", riddle5);
		var riddle2 = new QuestRiddleDataFull(true, "What was the first google doodle in 1998?");
		QuestRiddlesDataFull.Add("BurningMan", riddle2);
		var riddle6 = new QuestRiddleDataFull(false, riddleImages[1]);
        		QuestRiddlesDataFull.Add("Firebase", riddle6);
		var riddle3 = new QuestRiddleDataFull(true,
			"One day a computer failure stumped Grace Hopper and her team until she opened the machine and found THIS inside!");
		QuestRiddlesDataFull.Add("Bug", riddle3);
		var riddle7 = new QuestRiddleDataFull(false, riddleImages[2]);
        		QuestRiddlesDataFull.Add("GoogleSearch", riddle7);
		var riddle8 = new QuestRiddleDataFull(false, riddleImages[3]);
		QuestRiddlesDataFull.Add("Snap", riddle8);
		var riddle4 = new QuestRiddleDataFull(true, "The most important thing in the programming language is THIS. A language will not succeed without a good THIS. I have recently invented a very good THIS and now I am looking for a suitable language. -- Donald Knuth");
		QuestRiddlesDataFull.Add("Name", riddle4);
		WriteRiddleDataInQuestProgress();
	}

	void LeaderBoardInitialization()
	{
		QuestLeaderboardData = new Dictionary<string, QuestLeaderboardEntry>();
		var url = _auth.CurrentUser.PhotoUrl;
		for (var i = 0; i < 500; i++)
		{
			var entry = new QuestLeaderboardEntry(url, i*5);
			QuestLeaderboardData.Add(i.ToString(), entry);
		}
	}

	
	public void CompleteVrGame(int score, QuestVrGameController vrGameController)
	{	
		//Get the global data from FireBase
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		_database.Child("GlobalQuestData/VRGame").GetValueAsync().ContinueWith(task =>
		{
			var snapshot = task.Result;
			_timesCompleted = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());
			//Calculate and write the score
			questProgress.vrGameData.score = maxVRGameScore - _timesCompleted * 8;
			questProgress.globalScore += questProgress.vrGameData.score;
			_timesCompleted++;
			// update VR progress data in database
			childUpdates["GlobalQuestData/VRGame"] = _timesCompleted;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/vrGameData/score"] = questProgress.vrGameData.score;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/vrGameData/gameScore"] = score;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/vrGameData/state"] = true;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/globalScore"] = questProgress.globalScore;
			_database.UpdateChildrenAsync(childUpdates).ContinueWith(task1 => {
				if (task1.IsCompleted)
				{
					// mark VR game as completed in local storage
					questProgress.vrGameData.gameScore = score;
					questProgress.vrGameData.state = true;
					//Refresh the info panel
					_questUi.OnChangeInfoButtonClicked();
					//Update local leaderboard
					UpdateUserScoreInLeaderBoard();
					
					UpdateFirebaseLeaderboard();
				}
				else if (task1.IsFaulted)
				{
					Debug.LogError("QuestManager: Failed to update quest data in firebase realtime database!");
					Debug.LogError("Error message: " + task1.Exception.Message);
				}
				else if (task1.IsCanceled)
				{
					Debug.LogError("QuestManager: Cancel updating quest data in firebase realtime database!");
				}
			});
		});
	}

	public void CompletePhoto()
	{	
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		
		questProgress.photoData.state = true;
		//Refresh the info panel
		_questUi.OnChangeInfoButtonClicked();
		childUpdates["users/" + currentUserUserId + "/photoData/state"] = true;
		// mark photo step as completed in firebase
		_database.UpdateChildrenAsync(childUpdates).ContinueWith(task => 
		{
			if (task.IsCompleted)
			{
				
			}
			else if (task.IsFaulted)
			{
				Debug.LogError("QuestManager: Failed to update quest data in firebase realtime database!");
				Debug.LogError("Error message: " + task.Exception.Message);
			}
			else if (task.IsCanceled)
			{
				Debug.LogError("QuestManager: Cancel updating quest data in firebase realtime database!");
			}
		});
	}
	
	public void CompleteRiddle(string riddleKey, QuestRiddlesController riddlesController)
	{
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		
		//Check, how many times before riddle was completed and assign a score
		_database.Child("GlobalQuestData/" + riddleKey).GetValueAsync().ContinueWith(task =>
		{
			var snapshot = task.Result;
            _timesCompleted = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());
			//Update quest riddle progress data locally
			QuestRiddlesDataFull[riddleKey].score = maxRiddleScore - _timesCompleted;
			questProgress.riddlesData[riddleKey].score = maxRiddleScore - _timesCompleted;
			questProgress.globalScore += questProgress.riddlesData[riddleKey].score;
			_timesCompleted++;
			// mark riddle as completed in local storage
			QuestRiddlesDataFull[riddleKey].isCompleted = true;
			questProgress.riddlesData[riddleKey].isCompleted = true;
			
			//Update riddle screen
			ReadRiddleDataFromQuestProgress();
			riddlesController.UpdateRiddlesScreen();
			
			// update quest riddle progress data in database
			childUpdates["GlobalQuestData/" + riddleKey] = _timesCompleted;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/riddlesData/" + riddleKey + "/isCompleted"] = true;
            childUpdates["users/" + _auth.CurrentUser.DisplayName + "/riddlesData/" + riddleKey + "/score"] = 
	            questProgress.riddlesData[riddleKey].score;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/globalScore"] = questProgress.globalScore;
			_database.UpdateChildrenAsync(childUpdates).ContinueWith(task1 => 
			{
				if (task1.IsCompleted)
				{
					//Update local score
					UpdateUserScoreInLeaderBoard();
					
					UpdateFirebaseLeaderboard();
				}
				else if (task1.IsFaulted)
				{
					Debug.LogError("QuestManager: Failed to update quest data in firebase realtime database!");
					Debug.LogError("Error message: " + task1.Exception.Message);
				}
				else if (task1.IsCanceled)
				{
					Debug.LogError("QuestManager: Cancel updating quest data in firebase realtime database!");
				}
			});
		});
	}

	public void CompleteAllRiddles()
	{
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		
		questProgress.allRiddlesCompleted = true;
		//Refresh the info panel
		_questUi.OnChangeInfoButtonClicked();
		
		childUpdates["users/" + _auth.CurrentUser.DisplayName + "/allRiddlesCompleted"] = true;
		_database.UpdateChildrenAsync(childUpdates).ContinueWith(task => 
		{
			if (task.IsCompleted)
			{
				
			}
			else if (task.IsFaulted)
			{
				Debug.LogError("QuestManager: Failed to update quest data in firebase realtime database!");
				Debug.LogError("Error message: " + task.Exception.Message);
			}
			else if (task.IsCanceled)
			{
				Debug.LogError("QuestManager: Cancel updating quest data in firebase realtime database!");
			}
		});
	}
	
	void GetLeaderboardDataFromFirebase()
	{
#if UNITY_ANDROID
		var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait", "Updating Leaderboard...", AGDialogTheme.Dark);
		spinner.Show();
#endif
		//Try to get data from firebase
		_database.Child("GlobalQuestData/Leaderboards").GetValueAsync().ContinueWith(readTask =>
		{
			if (readTask.Result == null)
			{
				//Create data in firebase
				var json = JsonConvert.SerializeObject(QuestLeaderboardData);
				_database.Child("GlobalQuestData/Leaderboards")
					.SetRawJsonValueAsync(json).ContinueWith(writeTask => {
						if (writeTask.IsFaulted)
						{
							Debug.LogError("QuestManager: Failed to write default leaderboard data to firebase realtime database!");
							Debug.LogError("Error message: " + writeTask.Exception.Message);
						}
						else if (writeTask.IsCompleted)
						{
							Debug.Log("QuestManager: Default leaderboard data was successfully set up!");
						}
#if UNITY_ANDROID
						spinner.Dismiss();
#endif
					});
			}
			else
			{
				if (readTask.IsFaulted) 
				{
					Debug.LogError("QuestManager: Failed to retrieve leaderboard data from firebase realtime database!");
					Debug.LogError("Error message: " + readTask.Exception.Message);
				}
				else if (readTask.IsCompleted) 
				{
					// retrieve current leaderboard from firebase
					var snapshot = readTask.Result;
					QuestLeaderboardData = JsonConvert.DeserializeObject<Dictionary<string, QuestLeaderboardEntry>>
						(snapshot.GetRawJsonValue());
					Debug.Log("QuestManager: Leaderboard data was successfully updated!");
				}
#if UNITY_ANDROID
				spinner.Dismiss();
#endif
			}
		});
	}
	
	public void UpdateUserScoreInLeaderBoard()
	{
		GetLeaderboardDataFromFirebase();
		if (!QuestLeaderboardData.ContainsKey(currentUserUserId))
		{
			QuestLeaderboardData.Add(currentUserUserId, new QuestLeaderboardEntry(questProgress.userPhotoUrl, questProgress.globalScore));
		}
		else
		{
			QuestLeaderboardData[currentUserUserId] = new QuestLeaderboardEntry(questProgress.userPhotoUrl, questProgress.globalScore);
		}
	}

	void UpdateFirebaseLeaderboard()
	{
		var json = JsonConvert.SerializeObject(QuestLeaderboardData);
		_database.Child("GlobalQuestData/Leaderboards").SetRawJsonValueAsync(json);
	}
	
	public void CheckInPhoto(QuestPhotoController photoController)
	{
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		
		childUpdates["users/" + _auth.CurrentUser.DisplayName + "/photoData/imageURLSpeaker"] = questProgress.photoData.imgUrlSpeaker;
		
		childUpdates["users/" + _auth.CurrentUser.DisplayName + "/photoData/imageURLFriend"] = questProgress.photoData.imgUrlFriend;
		
		_database.UpdateChildrenAsync(childUpdates).ContinueWith(task => {
			if (task.IsCompleted)
			{
				
			}
			else if (task.IsFaulted)
			{
				Debug.LogError("QuestManager: Failed to update quest data in firebase realtime database!");
				Debug.LogError("Error message: " + task.Exception.Message);
			}
			else if (task.IsCanceled)
			{
				Debug.LogError("QuestManager: Cancel updating quest data in firebase realtime database!");
			}
		});
	}

	public void CheckIfQuestIsActivated()
	{
		_database.Child("GlobalQuestData/QuestActivation").GetValueAsync().ContinueWith(task =>
		{
			DataSnapshot snapshot = task.Result;
			isQuestActivated = JsonConvert.DeserializeObject<bool>(snapshot.GetRawJsonValue());
			Debug.Log("Quest activation status: " + isQuestActivated);
		});
	}

	void WriteRiddleDataInQuestProgress()
	{
		Debug.Log("RiddleDataInitialization2");
		foreach (var fullRiddle in QuestRiddlesDataFull)
		{
			var riddle = new QuestRiddleData(fullRiddle.Value.isCompleted, fullRiddle.Value.score);
			questProgress.riddlesData.Add(fullRiddle.Key, riddle);
		}
		Debug.Log("RiddleDataInitialization3");
	}

	public void ReadRiddleDataFromQuestProgress()
	{
		foreach (var riddle in questProgress.riddlesData)
		{
			QuestRiddlesDataFull[riddle.Key].score = riddle.Value.score;
			QuestRiddlesDataFull[riddle.Key].isCompleted = riddle.Value.isCompleted;
		}
	}

	public void CompleteGoogleColorsRiddle()
	{
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		
		questProgress.isGoogleColorsCompleted = true;
		//Refresh the info panel
		_questUi.OnChangeInfoButtonClicked();
		
		childUpdates["users/" + _auth.CurrentUser.DisplayName + "/isGoogleColorsCompleted"] = true;
		
		_database.UpdateChildrenAsync(childUpdates).ContinueWith(task1 => 
		{
			if (task1.IsCompleted)
			{
				
			}
			else if (task1.IsFaulted)
			{
				Debug.LogError("QuestManager: Failed to update quest data in firebase realtime database!");
				Debug.LogError("Error message: " + task1.Exception.Message);
			}
			else if (task1.IsCanceled)
			{
				Debug.LogError("QuestManager: Cancel updating quest data in firebase realtime database!");
			}
		});
	}

	IEnumerator LoadUserImageFromUrl()
	{
		var www = new WWW(userPhotoUrl.ToString());
		yield return www;
		userPhotoImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	}
}

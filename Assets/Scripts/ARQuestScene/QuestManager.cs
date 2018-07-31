using System.Collections.Generic;
using System.Linq;
using DeadMosquito.AndroidGoodies;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using ProBuilder2.Common;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	QuestProgress _questProgress;
	Dictionary<string, QuestRiddleData> _questRiddlesData;
	Dictionary<string, int> _questleaderboardData;
	QuestUI _questUi;
	DatabaseReference _database;
	FirebaseAuth _auth;

	public QuestProgress questProgress
	{
		get { return _questProgress; }
	}

	public Dictionary<string, QuestRiddleData> QuestRiddlesProgress
	{
		get { return _questProgress.riddlesData; }
	}
	public Dictionary<string, QuestRiddleData> QuestRiddlesData
	{
		get { return _questRiddlesData; }
	}
	public Dictionary<string, int> QuestLeaderboardData
	{
		get { return _questleaderboardData; }
	}
	public bool isQuestActivated;
	int _timesCompleted = 0;
	private string currentUserUserId;

	void Awake()
	{
		Debug.Log("QuestManager.Awake");
		// Get the root reference location of the database.
		_auth = FirebaseAuth.DefaultInstance;
		_database = FirebaseDatabase.DefaultInstance.RootReference;
		currentUserUserId = _auth.CurrentUser.DisplayName;
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hoverboard-v2-dev.firebaseio.com/");

		// obtain reference to object that represents quest UI
		UiReferenceInitialization();

		// initialize riddle data
		RiddleDataInitizalization();
		
		//Initialize Leaderboard
		LeaderBoardInitialization();
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
				string json = JsonConvert.SerializeObject(_questProgress);
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
					});
			}
			else
			{
				if (readTask.IsFaulted) 
				{
					Debug.LogError("QuestManager: Failed to retrieve quest data from firebase realtime database!");
					Debug.LogError("Error message: " + readTask.Exception.Message);
				}
				else if (readTask.IsCompleted) 
				{
					// retrieve user quest progress
					DataSnapshot snapshot = readTask.Result;
					_questProgress = JsonConvert.DeserializeObject<QuestProgress>(snapshot.GetRawJsonValue());
				
					Debug.Log("QuestManager: Quest data was successfully set up!");
					
					_questUi.FadeScreenOut();
#if UNITY_ANDROID
					spinner.Dismiss();
#endif
				}
			}
			//Check if Quest is activated
			CheckIfQuestIsActivated();
		});
		
		GetLeaderboardDataFromFirebase();
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
		_questProgress = new QuestProgress();
		
		_questRiddlesData = new Dictionary<string, QuestRiddleData>();
		
		QuestRiddleData androidRiddle = new QuestRiddleData("Popular mobile OS that is powered by Google?");
		_questRiddlesData.Add("Android", androidRiddle);
		_questProgress.riddlesData.Add("Android", androidRiddle);

		QuestRiddleData angularRiddle = new QuestRiddleData("TypeScript-based open-source front-end web application platform?");
		_questRiddlesData.Add("Angular", angularRiddle);
		_questProgress.riddlesData.Add("Angular", angularRiddle);

		QuestRiddleData arcoreRiddle = new QuestRiddleData("Software development kit developed by Google that allow for mixed reality applications to be built?");
		_questRiddlesData.Add("Arcore", arcoreRiddle);
		_questProgress.riddlesData.Add("Arcore", arcoreRiddle);

		QuestRiddleData firebaseRiddle = new QuestRiddleData("Mobile and web application development platform?");
		_questRiddlesData.Add("Firebase", firebaseRiddle);
		_questProgress.riddlesData.Add("Firebase", firebaseRiddle);
	}

	void LeaderBoardInitialization()
	{
		_questleaderboardData = new Dictionary<string, int>();
	}

	
	public void CompleteVrGame(int score, QuestVrGameController vrGameController)
	{	
		//Get the global data from FireBase
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		_database.Child("GlobalQuestData/VRGame").GetValueAsync().ContinueWith(task =>
		{
			DataSnapshot snapshot = task.Result;
			_timesCompleted = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());
			//Calculate and write the score
			_questProgress.vrGameData.score = 8000 - _timesCompleted * 8;
			_questProgress.globalScore += _questProgress.vrGameData.score;
			_timesCompleted++;
			// update VR progress data in database
			childUpdates["GlobalQuestData/VRGame"] = _timesCompleted;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/vrGameData/score"] = _questProgress.vrGameData.score;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/vrGameData/gameScore"] = score;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/vrGameData/state"] = true;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/globalScore"] = _questProgress.globalScore;
			//Update local leaderboard
			UpdateUserScoreInLeaderBoard();
			//Update Firebase Leaderboard
			UpdateFirebaseLeaderboard();
			_database.UpdateChildrenAsync(childUpdates).ContinueWith(task1 => {
				if (task1.IsCompleted)
				{
					// mark VR game as completed in local storage
					_questProgress.vrGameData.gameScore = score;
					_questProgress.vrGameData.state = true;
					vrGameController.UpdateVrGameScreen();
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

	public void CompleteRiddle(string riddleKey, QuestRiddlesController riddlesController)
	{
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		
		//Check, how many times before riddle was completed and assign a score
		_database.Child("GlobalQuestData/" + riddleKey).GetValueAsync().ContinueWith(task =>
		{
			DataSnapshot snapshot = task.Result;
            _timesCompleted = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());
			_questProgress.riddlesData[riddleKey].score = 1000 - _timesCompleted;
			_questProgress.globalScore += _questProgress.riddlesData[riddleKey].score;
			_timesCompleted++;
			// update quest riddle progress data in database
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/riddlesData/" + riddleKey + "/isCompleted"] = true;
            childUpdates["GlobalQuestData/" + riddleKey] = _timesCompleted;
            childUpdates["users/" + _auth.CurrentUser.DisplayName + "/riddlesData/" + riddleKey + "/score"] = _questProgress.riddlesData[riddleKey].score;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/globalScore"] = _questProgress.globalScore;
			UpdateUserScoreInLeaderBoard();
			//Update Firebase Leaderboard
			UpdateFirebaseLeaderboard();
			_database.UpdateChildrenAsync(childUpdates).ContinueWith(task1 => 
			{
				if (task1.IsCompleted)
				{
					// mark riddle as completed in local storage
					_questProgress.riddlesData[riddleKey].isCompleted = true;
					riddlesController.UpdateRiddlesScreen();
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
	
	public void GetLeaderboardDataFromFirebase()
	{
#if UNITY_ANDROID
		var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait", "Updating Quest Data...", AGDialogTheme.Dark);
		spinner.Show();
#endif
		//Try to get data from firebase
		_database.Child("GlobalQuestData/Leaderboards").GetValueAsync().ContinueWith(readTask =>
		{
			if (readTask.Result == null)
			{
				//Create data in firebase
				string json = JsonConvert.SerializeObject(_questleaderboardData);
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
					DataSnapshot snapshot = readTask.Result;
					_questleaderboardData = JsonConvert.DeserializeObject<Dictionary<string, int>>
						(snapshot.GetRawJsonValue());
					Debug.Log("QuestManager: Leaderboard data was successfully updated!");
#if UNITY_ANDROID
					spinner.Dismiss();
#endif
				}
			}
		});
	}
	
	void UpdateUserScoreInLeaderBoard()
	{
		GetLeaderboardDataFromFirebase();
		if (!CheckIfUserIsAlreadyInLeaderboard())
		{
			_questleaderboardData.Add(currentUserUserId, _questProgress.globalScore);
		}
		else
		{
			_questleaderboardData[currentUserUserId] = _questProgress.globalScore;
		}
		SortLeaderBoard();
	}

	void SortLeaderBoard()
	{
		var list = _questleaderboardData.ToList();
		list.Sort((pair1,pair2) => pair1.Value.CompareTo(pair2.Value));
		_questleaderboardData.Clear();
		foreach (var x in list)
		{
			_questleaderboardData.Add(x.Key, x.Value);
		}
	}

	bool CheckIfUserIsAlreadyInLeaderboard()
	{
		bool x = false;
		foreach (KeyValuePair<string, int> entry in _questleaderboardData)
		{
			if (entry.Key == currentUserUserId)
			{
				x = true;
			}
		}
		return x;
	}

	void UpdateFirebaseLeaderboard()
	{
		string json = JsonConvert.SerializeObject(_questleaderboardData);
		_database.Child("GlobalQuestData/Leaderboards").SetRawJsonValueAsync(json);
	}
	
	public void CheckInPhoto(QuestPhotoController photoController)
	{
		// update quest riddle progress data in database
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		//childUpdates["users/" + _auth.CurrentUser.UserId + "/photoData/state"] = true;
		childUpdates["users/" + _auth.CurrentUser.DisplayName + "/photoData/imageURL"] = photoController.imageUrl;
		_database.UpdateChildrenAsync(childUpdates).ContinueWith(task => {
			if (task.IsCompleted)
			{
				// mark photocapture as complete in local storage
				//_questProgress.photoData.state = true;
				_questProgress.photoData.imgUrl = photoController.imageUrl;
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
}

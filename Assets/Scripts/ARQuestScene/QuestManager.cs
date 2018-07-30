using System.Collections.Generic;
using DeadMosquito.AndroidGoodies;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	QuestProgress _questProgress;
	Dictionary<string, QuestRiddleData> _questRiddlesData;

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

	public bool isQuestActivated;
	int _timesCompleted = 0;

	void Awake()
	{
		Debug.Log("QuestManager.Awake");
		// Get the root reference location of the database.
		_auth = FirebaseAuth.DefaultInstance;
		_database = FirebaseDatabase.DefaultInstance.RootReference;
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hoverboard-v2-dev.firebaseio.com/");

		// obtain reference to object that represents quest UI
		UiReferenceInitialization();

		// initialize riddle data
		RiddleDataInitizalization();
	}

	void Start()
	{
		Debug.Log("QuestManager.Start");
		_questUi.FadeQuestScreenIn();
#if UNITY_ANDROID
		var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait", "Updating Quest Data...", AGDialogTheme.Dark);
		spinner.Show();
#endif
		string currentUserUserId = _auth.CurrentUser.DisplayName;
		Debug.Log("String was activated.");
		var with = _database.Child("users").Child(currentUserUserId).GetValueAsync().ContinueWith(readTask => {
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
				}
			}
			//Check if Quest is activated
			CheckIfQuestIsActivated();
			
#if UNITY_ANDROID
			spinner.Dismiss();
#endif
		});
		var continueWith = with;
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

	public void CompleteVrGame(int score, QuestVrGameController vrGameController)
	{	
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		_database.Child("users/GlobalQuestData/VRGame").GetValueAsync().ContinueWith(task =>
		{
			DataSnapshot snapshot = task.Result;
			_timesCompleted = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());
			_questProgress.vrGameData.score = 8000 - _timesCompleted * 8;
			_questProgress.globalScore += _questProgress.vrGameData.score;
			_timesCompleted++;
			// update VR progress data in database
			childUpdates["users/GlobalQuestData/VRGame"] = _timesCompleted;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/vrGameData/score"] = _questProgress.vrGameData.score;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/vrGameData/gameScore"] = score;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/vrGameData/state"] = true;
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
		_database.Child("users/GlobalQuestData/" + riddleKey).GetValueAsync().ContinueWith(task =>
		{
			DataSnapshot snapshot = task.Result;
            _timesCompleted = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());
			_questProgress.riddlesData[riddleKey].score = 1000 - _timesCompleted;
			_questProgress.globalScore += _questProgress.riddlesData[riddleKey].score;
			_timesCompleted++;
			// update quest riddle progress data in database
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/riddlesData/" + riddleKey + "/isCompleted"] = true;
            childUpdates["users/GlobalQuestData/" + riddleKey] = _timesCompleted;
            childUpdates["users/" + _auth.CurrentUser.DisplayName + "/riddlesData/" + riddleKey + "/score"] = _questProgress.riddlesData[riddleKey].score;
			childUpdates["users/" + _auth.CurrentUser.DisplayName + "/globalScore"] = _questProgress.globalScore;
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
		_database.Child("users/GlobalQuestData/QuestActivation").GetValueAsync().ContinueWith(task =>
		{
			DataSnapshot snapshot = task.Result;
			isQuestActivated = JsonConvert.DeserializeObject<bool>(snapshot.GetRawJsonValue());
			Debug.Log("Quest activation status: " + isQuestActivated);
		});
	}
}

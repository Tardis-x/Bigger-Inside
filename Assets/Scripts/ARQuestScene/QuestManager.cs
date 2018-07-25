using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	QuestProgress _questProgress;
	Dictionary<string, QuestRiddleContent> _questRiddlesContent;

	QuestUI _questUi;

	DatabaseReference _database;

	public QuestProgress questProgress
	{
		get { return _questProgress; }
	}

	public Dictionary<string, bool> QuestRiddlesProgress
	{
		get { return _questProgress.riddlesData; }
	}
	
	public Dictionary<string, QuestRiddleContent> QuestRiddlesContent
	{
		get { return _questRiddlesContent; }
	}

	void Awake()
	{
		Debug.Log("QuestManager.Awake");

		// obtain reference to object that represents quest UI
		UiReferenceInitialization();

		// initialize riddle data
		RiddleDataInitizalization();
	}

	void Start()
	{
		Debug.Log("QuestManager.Start");
		
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hoverboard-v2-dev.firebaseio.com/");

		// Get the root reference location of the database.
		_database = FirebaseDatabase.DefaultInstance.RootReference;
		
		_database.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).GetValueAsync().ContinueWith(readTask => {
			if (readTask.Result == null)
			{
				string json = JsonConvert.SerializeObject(_questProgress);
				_database.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).SetRawJsonValueAsync(json).ContinueWith(writeTask => {
					if (writeTask.IsFaulted)
					{
						Debug.LogError("QuestManager: Failed to write default quest data to firebase realtime database!");
						Debug.LogError("Error message: " + writeTask.Exception.Message);
					}
					else if (writeTask.IsCompleted)
					{
						Debug.Log("QuestManager: Default quest data was successfully set up!");
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
				}
			}
		});
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
		
		_questRiddlesContent = new Dictionary<string, QuestRiddleContent>();
		
		QuestRiddleContent androidRiddle = new QuestRiddleContent("Popular mobile OS that is powered by Google?");
		_questRiddlesContent.Add("Android", androidRiddle);
		_questProgress.riddlesData.Add("Android", false);

		QuestRiddleContent angularRiddle = new QuestRiddleContent("TypeScript-based open-source front-end web application platform?");
		_questRiddlesContent.Add("Angular", angularRiddle);
		_questProgress.riddlesData.Add("Angular", false);

		QuestRiddleContent arcoreRiddle = new QuestRiddleContent("Software development kit developed by Google that allow for mixed reality applications to be built?");
		_questRiddlesContent.Add("Arcore", arcoreRiddle);
		_questProgress.riddlesData.Add("Arcore", false);

		QuestRiddleContent firebaseRiddle = new QuestRiddleContent("Mobile and web application development platform?");
		_questRiddlesContent.Add("Firebase", firebaseRiddle);
		_questProgress.riddlesData.Add("Firebase", false);
	}

	public void CompleteVrGame(int score, QuestVrGameController vrGameController)
	{	
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		childUpdates["users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/vrGameData/score"] = score;
		childUpdates["users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/vrGameData/state"] = true;
		
		_database.UpdateChildrenAsync(childUpdates).ContinueWith(task => {
			if (task.IsCompleted)
			{
				// mark VR game as completed in local storage
				_questProgress.vrGameData.score = score;
				_questProgress.vrGameData.state = true;
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
		// update quest riddle progress data in database
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		childUpdates["users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/riddlesData/" + riddleKey] = true;
		_database.UpdateChildrenAsync(childUpdates).ContinueWith(task => {
			if (task.IsCompleted)
			{
				// mark riddle as complete in local storage
				_questProgress.riddlesData[riddleKey] = true;
				
				
				riddlesController.UpdateRiddlesScreen();
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
	public void CheckInPhoto(QuestPhotoController photoController)
	{
		// update quest riddle progress data in database
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		childUpdates["users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/photoData/state"] = true;
		childUpdates["users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/photoData/imageURL"] = photoController.imageUrl;
		_database.UpdateChildrenAsync(childUpdates).ContinueWith(task => {
			if (task.IsCompleted)
			{
				// mark photocapture as complete in local storage
				_questProgress.photoData.state = true;
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
}

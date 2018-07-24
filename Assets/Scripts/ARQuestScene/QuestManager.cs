using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	Dictionary<string, bool> _questRiddlesProgress;
	Dictionary<string, QuestRiddleData> _questRiddlesData;

	QuestUI _questUi;

	DatabaseReference _database;

	public Dictionary<string, bool> QuestRiddlesProgress
	{
		get { return _questRiddlesProgress; }
	}
	
	public Dictionary<string, QuestRiddleData> QuestRiddlesData
	{
		get { return _questRiddlesData; }
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
				string json = JsonConvert.SerializeObject(_questRiddlesProgress);
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
					_questRiddlesProgress = JsonConvert.DeserializeObject<Dictionary<string, bool>>(snapshot.GetRawJsonValue());
				
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
		
		_questRiddlesProgress = new Dictionary<string, bool>();
		_questRiddlesData = new Dictionary<string, QuestRiddleData>();
		
		QuestRiddleData androidRiddle = new QuestRiddleData("Popular mobile OS that is powered by Google?");
		_questRiddlesData.Add("Android", androidRiddle);
		_questRiddlesProgress.Add("Android", false);

		QuestRiddleData angularRiddle = new QuestRiddleData("TypeScript-based open-source front-end web application platform?");
		_questRiddlesData.Add("Angular", angularRiddle);
		_questRiddlesProgress.Add("Angular", false);

		QuestRiddleData arcoreRiddle = new QuestRiddleData("Software development kit developed by Google that allow for mixed reality applications to be built?");
		_questRiddlesData.Add("Arcore", arcoreRiddle);
		_questRiddlesProgress.Add("Arcore", false);

		QuestRiddleData firebaseRiddle = new QuestRiddleData("Mobile and web application development platform?");
		_questRiddlesData.Add("Firebase", firebaseRiddle);
		_questRiddlesProgress.Add("Firebase", false);
	}

	public void CompleteRiddle(string riddleKey, QuestRiddlesController riddlesController)
	{
		// update quest progress data in database
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		childUpdates["users/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId + "/" + riddleKey] = true;
		_database.UpdateChildrenAsync(childUpdates).ContinueWith(task => {
			if (task.IsCompleted)
			{
				// mark riddle as complete in local storage
				_questRiddlesProgress[riddleKey] = true;
				
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
}
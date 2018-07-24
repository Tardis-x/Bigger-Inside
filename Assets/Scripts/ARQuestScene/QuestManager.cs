﻿using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	QuestProgress questProgress;
	Dictionary<string, QuestRiddleContent> _questRiddlesContent;

	QuestUI _questUi;

	DatabaseReference _database;

	public Dictionary<string, bool> QuestRiddlesProgress
	{
		get { return questProgress.riddlesData; }
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
				string json = JsonConvert.SerializeObject(questProgress);
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
					questProgress = JsonConvert.DeserializeObject<QuestProgress>(snapshot.GetRawJsonValue());
				
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
		questProgress = new QuestProgress();
		
		_questRiddlesContent = new Dictionary<string, QuestRiddleContent>();
		
		QuestRiddleContent androidRiddle = new QuestRiddleContent("Popular mobile OS that is powered by Google?");
		_questRiddlesContent.Add("Android", androidRiddle);
		questProgress.riddlesData.Add("Android", false);

		QuestRiddleContent angularRiddle = new QuestRiddleContent("TypeScript-based open-source front-end web application platform?");
		_questRiddlesContent.Add("Angular", angularRiddle);
		questProgress.riddlesData.Add("Angular", false);

		QuestRiddleContent arcoreRiddle = new QuestRiddleContent("Software development kit developed by Google that allow for mixed reality applications to be built?");
		_questRiddlesContent.Add("Arcore", arcoreRiddle);
		questProgress.riddlesData.Add("Arcore", false);

		QuestRiddleContent firebaseRiddle = new QuestRiddleContent("Mobile and web application development platform?");
		_questRiddlesContent.Add("Firebase", firebaseRiddle);
		questProgress.riddlesData.Add("Firebase", false);
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
				questProgress.riddlesData[riddleKey] = true;
				
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
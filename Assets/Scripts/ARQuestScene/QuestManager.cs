using System.Collections.Generic;
#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies;
#endif
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	const int MaxRiddleScore = 8000;
	const int MaxVrGameScore = 40000;
	const int MaxSocialScore = 40000;

	QuestUI _questUi;
	public QuestFirebaseData firebaseData;

	public QuestProgress QuestProgress { get; set; }

	public Dictionary<string, QuestLeaderboardEntry> QuestLeaderboardData { get; set; }

	public Dictionary<string, QuestRiddleDataFull> QuestRiddlesDataFull { get; set; }
	public bool isQuestActivated;
	int _timesCompleted;
	public Sprite[] riddleImages;

	void Awake()
	{
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hoverboard-firebase.firebaseio.com/");
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
		firebaseData.database.Child("users").Child(firebaseData.currentUserUserId).GetValueAsync().ContinueWith(
			readTask =>
			{
				if (readTask.Result == null)
				{
					var json = JsonConvert.SerializeObject(QuestProgress);
					firebaseData.database.Child("users").Child(firebaseData.currentUserUserId)
						.SetRawJsonValueAsync(json).ContinueWith(writeTask =>
						{
							if (writeTask.IsFaulted)
							{
								Debug.LogError(
									"QuestManager: Failed to write default quest data to firebase realtime database!");
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
								"It's a Quest, where you will have to complete different tasks related to the conference and Google technologies knowledge.\n\nNote: accomplish all tasks as fast as possible to win a greater award!",
								0);
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
						QuestProgress = JsonConvert.DeserializeObject<QuestProgress>(snapshot.GetRawJsonValue());

						Debug.Log("QuestManager: Quest data was successfully set up!");
						_questUi.FadeScreenOut();

						Debug.Log("Updating Info screen");
						if (QuestProgress.allRiddlesCompleted)
						{
							_questUi.ShowUserScorePanel();
						}
						else
						{
							Debug.Log("Default info screen");
							_questUi.ShowInfoPanel("Welcome to the DevFest\nQuest Adventure!",
								"It's a Quest, where you will have to complete different tasks related to the conference and Google technologies knowledge.\n\nNote: accomplish all tasks as fast as possible to win a greater award!",
								0);
						}
					}
#if UNITY_ANDROID
					spinner.Dismiss();
#endif
				}

				//Check if Quest is activated
				CheckIfQuestIsActivated();
				ReadRiddleDataFromQuestProgress();
				UpdateUserScoreInLeaderBoard(false);
			});
	}

	void UiReferenceInitialization()
	{
		var questCanvas = GameObject.Find("QuestCanvas");

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
		QuestProgress = new QuestProgress {userPhotoUrl = firebaseData.userPhotoUrl};

		QuestRiddlesDataFull = new Dictionary<string, QuestRiddleDataFull>();
		Debug.Log("RiddleDataInitialization");
		var riddle1 = new QuestRiddleDataFull(true, "How are new comers called in Google?", "Question #1");
		QuestRiddlesDataFull.Add("Noogler", riddle1);
//		var riddle5 = new QuestRiddleDataFull(false, riddleImages[0]);
//		QuestRiddlesDataFull.Add("Angular", riddle5);
//		var riddle2 = new QuestRiddleDataFull(true, "What was the first google doodle in 1998?");
//		QuestRiddlesDataFull.Add("BurningMan", riddle2);
//		var riddle6 = new QuestRiddleDataFull(false, riddleImages[1]);
//		QuestRiddlesDataFull.Add("Firebase", riddle6);
		var riddle9 = new QuestRiddleDataFull(false, riddleImages[4]);
		QuestRiddlesDataFull.Add("AndroidAuto", riddle9);
		var riddle3 = new QuestRiddleDataFull(true,
			"One day a computer failure stumped Grace Hopper and her team until she opened the machine and found THIS inside!", "Question #2");
		QuestRiddlesDataFull.Add("Bug", riddle3);
		var riddle10 = new QuestRiddleDataFull(false, riddleImages[5]);
		QuestRiddlesDataFull.Add("Dart", riddle10);
		var riddle11 = new QuestRiddleDataFull(true,
			"Enthusiasts-musicians-developers developed a computer called AImus, which could compose music by himself. Answer with two words. How was his first music composition named?", "Question #3");
		QuestRiddlesDataFull.Add("HelloWorld", riddle11);
//		var riddle7 = new QuestRiddleDataFull(false, riddleImages[2]);
//		QuestRiddlesDataFull.Add("GoogleSearch", riddle7);
//		var riddle8 = new QuestRiddleDataFull(false, riddleImages[3]);
//		QuestRiddlesDataFull.Add("Snap", riddle8);
//		var riddle4 = new QuestRiddleDataFull(true,
//			"The most important thing in the programming language is THIS. A language will not succeed without a good THIS. I have recently invented a very good THIS and now I am looking for a suitable language. -- Donald Knuth");
//		QuestRiddlesDataFull.Add("Name", riddle4);
		
		WriteRiddleDataInQuestProgress();
	}

	void LeaderBoardInitialization()
	{
		QuestLeaderboardData = new Dictionary<string, QuestLeaderboardEntry>();
	}


	public void CompleteVrGame(int score, QuestVrGameController vrGameController)
	{
		//Get the global data from FireBase
		var childUpdates = new Dictionary<string, object>();
		firebaseData.database.Child("GlobalQuestData/VRGame").GetValueAsync().ContinueWith(task =>
		{
			var snapshot = task.Result;
			_timesCompleted = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());
			//Calculate and write the score
			QuestProgress.VrGameData.Score = MaxVrGameScore - _timesCompleted * 5;
			QuestProgress.globalScore += QuestProgress.VrGameData.Score;
			_timesCompleted++;
			// update VR progress data in database
			childUpdates["GlobalQuestData/VRGame"] = _timesCompleted;
			childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/VrGameData/Score"] =
				QuestProgress.VrGameData.Score;
			childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/VrGameData/GameScore"] = score;
			childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/VrGameData/State"] = true;
			childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/globalScore"] =
				QuestProgress.globalScore;
			firebaseData.database.UpdateChildrenAsync(childUpdates).ContinueWith(task1 =>
			{
				if (task1.IsCompleted)
				{
					// mark VR game as completed in local storage
					QuestProgress.VrGameData.GameScore = score;
					QuestProgress.VrGameData.State = true;

					//Update local leaderboard
					UpdateUserScoreInLeaderBoard();
					UpdateFirebaseLeaderboard();

					//Refresh the info panel
					_questUi.ShowCongratzPanel(QuestProgress.VrGameData.Score, 2);
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
		var childUpdates = new Dictionary<string, object>();
		firebaseData.database.Child("GlobalQuestData/Social").GetValueAsync().ContinueWith(task =>
		{
			QuestProgress.PhotoData.State = true;

			var snapshot = task.Result;
			_timesCompleted = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());
			QuestProgress.PhotoData.Score = MaxSocialScore - _timesCompleted * 5;
			QuestProgress.globalScore += QuestProgress.PhotoData.Score;
			_timesCompleted++;

			childUpdates["GlobalQuestData/Social"] = _timesCompleted;
			childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/PhotoData/Score"] =
				QuestProgress.PhotoData.Score;
			childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/PhotoData/State"] = true;
			childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/globalScore"] =
				QuestProgress.globalScore;

			firebaseData.database.UpdateChildrenAsync(childUpdates).ContinueWith(task1 =>
			{
				if (task1.IsCompleted)
				{
					UpdateUserScoreInLeaderBoard();
					UpdateFirebaseLeaderboard();

					_questUi.ShowCongratzPanel(QuestProgress.PhotoData.Score, 1);
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
		var childUpdates = new Dictionary<string, object>();
		firebaseData.database.Child("GlobalQuestData/" + riddleKey).GetValueAsync().ContinueWith(task =>
		{
			var snapshot = task.Result;
			_timesCompleted = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());
			QuestRiddlesDataFull[riddleKey].score = MaxRiddleScore - _timesCompleted;
			QuestProgress.RiddlesData[riddleKey].score = MaxRiddleScore - _timesCompleted;
			QuestProgress.globalScore += QuestProgress.RiddlesData[riddleKey].score;
			_timesCompleted++;

			QuestRiddlesDataFull[riddleKey].isCompleted = true;
			QuestProgress.RiddlesData[riddleKey].isCompleted = true;

			childUpdates["GlobalQuestData/" + riddleKey] = _timesCompleted;
			childUpdates[
					"users/" + firebaseData.auth.CurrentUser.DisplayName + "/RiddlesData/" + riddleKey +
					"/isCompleted"] =
				true;
			childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/RiddlesData/" + riddleKey + "/score"]
				=
				QuestProgress.RiddlesData[riddleKey].score;
			childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/globalScore"] =
				QuestProgress.globalScore;
			firebaseData.database.UpdateChildrenAsync(childUpdates).ContinueWith(task1 =>
			{
				if (task1.IsCompleted)
				{
					//Update local score
					UpdateUserScoreInLeaderBoard();
					UpdateFirebaseLeaderboard();

					ReadRiddleDataFromQuestProgress();
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

	public void CompleteAllRiddles()
	{
		var childUpdates = new Dictionary<string, object>();

		QuestProgress.allRiddlesCompleted = true;
		var riddlesScore = QuestProgress.globalScore - QuestProgress.PhotoData.Score - QuestProgress.VrGameData.Score;

		childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/allRiddlesCompleted"] = true;
		firebaseData.database.UpdateChildrenAsync(childUpdates).ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				_questUi.ShowCongratzPanel(riddlesScore, 3);
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

	void GetLeaderboardDataFromFirebase(bool showSpinner = true)
	{
		var spinner = AGProgressDialog.CreateSpinnerDialog("Please wait", "Updating Leaderboard...", AGDialogTheme.Dark);
		if (showSpinner)
		{
#if UNITY_ANDROID
			spinner.Show();
#endif
		}

		//Try to get data from firebase
		firebaseData.database.Child("GlobalQuestData/Leaderboards").GetValueAsync().ContinueWith(readTask =>
		{
			if (readTask.Result == null)
			{
				//Create data in firebase
				var json = JsonConvert.SerializeObject(QuestLeaderboardData);
				firebaseData.database.Child("GlobalQuestData/Leaderboards")
					.SetRawJsonValueAsync(json).ContinueWith(writeTask =>
					{
						if (writeTask.IsFaulted)
						{
							Debug.LogError(
								"QuestManager: Failed to write default leaderboard data to firebase realtime database!");
							Debug.LogError("Error message: " + writeTask.Exception.Message);
						}
						else if (writeTask.IsCompleted)
						{
							Debug.Log("QuestManager: Default leaderboard data was successfully set up!");
						}

						if (showSpinner)
						{
							#if UNITY_ANDROID
                            spinner.Dismiss();
                            #endif
						}
					});
			}
			else
			{
				if (readTask.IsFaulted)
				{
					Debug.LogError(
						"QuestManager: Failed to retrieve leaderboard data from firebase realtime database!");
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

				if (showSpinner)
				{
#if UNITY_ANDROID
					spinner.Dismiss();
#endif
				}
			}
		});
	}

	public void UpdateUserScoreInLeaderBoard(bool showSpinner = true)
	{
		GetLeaderboardDataFromFirebase(showSpinner);
		if (!QuestLeaderboardData.ContainsKey(firebaseData.currentUserUserId))
		{
			QuestLeaderboardData.Add(firebaseData.currentUserUserId,
				new QuestLeaderboardEntry(QuestProgress.userPhotoUrl, QuestProgress.globalScore));
		}
		else
		{
			QuestLeaderboardData[firebaseData.currentUserUserId] =
				new QuestLeaderboardEntry(QuestProgress.userPhotoUrl, QuestProgress.globalScore);
		}
	}

	void UpdateFirebaseLeaderboard()
	{
		var json = JsonConvert.SerializeObject(QuestLeaderboardData[firebaseData.auth.CurrentUser.DisplayName]);
		firebaseData.database.Child("GlobalQuestData/Leaderboards/" + firebaseData.auth.CurrentUser.DisplayName)
			.SetRawJsonValueAsync(json);
	}

	public void CheckInPhoto(QuestPhotoController photoController)
	{
		var childUpdates = new Dictionary<string, object>();

		childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/PhotoData/imageURLSpeaker"] =
			QuestProgress.PhotoData.ImgUrlSpeaker;

		childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/PhotoData/imageURLFriend"] =
			QuestProgress.PhotoData.ImgUrlFriend;

		firebaseData.database.UpdateChildrenAsync(childUpdates).ContinueWith(task =>
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

	public void CheckIfQuestIsActivated()
	{
		firebaseData.database.Child("GlobalQuestData/QuestActivation").GetValueAsync().ContinueWith(task =>
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
			QuestProgress.RiddlesData.Add(fullRiddle.Key, riddle);
		}

		Debug.Log("RiddleDataInitialization3");
	}

	public void ReadRiddleDataFromQuestProgress()
	{
		foreach (var riddle in QuestProgress.RiddlesData)
		{
			QuestRiddlesDataFull[riddle.Key].score = riddle.Value.score;
			QuestRiddlesDataFull[riddle.Key].isCompleted = riddle.Value.isCompleted;
		}
	}

	public void CompleteGoogleColorsRiddle()
	{
		var childUpdates = new Dictionary<string, object>();

		QuestProgress.isGoogleColorsCompleted = true;
		//Refresh the info panel
		_questUi.OnChangeInfoButtonClicked();

		childUpdates["users/" + firebaseData.auth.CurrentUser.DisplayName + "/isGoogleColorsCompleted"] = true;

		firebaseData.database.UpdateChildrenAsync(childUpdates).ContinueWith(task1 =>
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
}
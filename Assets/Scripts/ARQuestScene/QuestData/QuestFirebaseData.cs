using System;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class QuestFirebaseData : MonoBehaviour
{
	public DatabaseReference database;
	public FirebaseAuth auth;
	public Uri userPhotoUrl;
	public string currentUserUserId;

	void Awake()
	{
		database = FirebaseDatabase.DefaultInstance.RootReference;
		auth = FirebaseAuth.DefaultInstance;
		currentUserUserId = auth.CurrentUser.DisplayName;
		userPhotoUrl = auth.CurrentUser.PhotoUrl;
	}
}

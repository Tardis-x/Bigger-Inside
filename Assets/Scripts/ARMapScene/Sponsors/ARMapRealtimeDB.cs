using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class ARMapRealtimeDB : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Delegates
    //--------------------------------------------------------------------- 
    
    public delegate void OnSuccess(List<SponsorModel> sponsorModels);
    public delegate void OnError();
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    private const string PATH_BASE = "ARMap";
    private const string PATH_SPONSORS = PATH_BASE + "/Sponsors";

    private DatabaseReference _sponsorsReference;
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(Constants.REALTIME_DATABASE_URL);
      _sponsorsReference = FirebaseDatabase.DefaultInstance.RootReference.Child(PATH_SPONSORS);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void LoadSponsorData(OnSuccess onSuccess, OnError onError)
    {
      _sponsorsReference.GetValueAsync().ContinueWith(
        task =>
        {
          if (task.IsCompleted)
          {
            var snapshot = task.Result;
            var sponsorModels = JsonConvert.DeserializeObject<List<SponsorModel>>(snapshot.GetRawJsonValue());
            onSuccess(sponsorModels);
          }
          else
          {
            onError();
          }
        });
    }
  }
}
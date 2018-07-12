using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Firebase.Storage;

namespace ua.org.gdg.devfest
{
  public class API : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private ScrollableListScript _listScript;
    [SerializeField] private SpeechScript _speechScript;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      WWW scheduleRequest = new WWW(SCHEDULE_URL);
      WWW sessionRequest = new WWW(SESSIONS_URL);
      WWW speakerRequest = new WWW(SPEAKERS_URL);

      StartCoroutine(OnScheduleResponse(scheduleRequest));
      StartCoroutine(OnSessionResponse(sessionRequest));
      StartCoroutine(OnSpeakerResponse(speakerRequest));
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OnClick()
    {
      Request(Hall.Expo, 0);
      _listScript.Show();
    }

    public void Request(Hall hall, int day)
    {
      _listScript.AddContent(FirestoreHelper
        .ComposeScheduleForHall(hall, day, _schedule, _items, _speakers, _speechScript));
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const string SCHEDULE_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/schedule";

    private const string SESSIONS_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/sessions?pageSize=40";

    private const string SPEAKERS_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/speakers?pageSize=40";


    private FirebaseStorage _storage;
    private StorageReference _imageReference;
    private Schedule _schedule;
    private List<SessionItem> _items;
    private Dictionary<string, Speaker> _speakers;

    private IEnumerator OnScheduleResponse(WWW req)
    {
      yield return req;

      JsonSchedule jSchedule = JsonConvert.DeserializeObject<JsonSchedule>(req.text);
      _schedule = FirestoreHelper.ParseSchedule(jSchedule);
    }

    private IEnumerator OnSessionResponse(WWW req)
    {
      yield return req;

      SessionTable st = JsonConvert.DeserializeObject<SessionTable>(req.text);
      _items = FirestoreHelper.ParseSessions(st);
    }

    private IEnumerator OnPhotoResponse(WWW req, Speaker speaker)
    {
      yield return req;

      speaker.Photo = req.texture;
    }

    private IEnumerator OnSpeakerResponse(WWW req)
    {
      yield return req;

      JsonSpeakersTable st = JsonConvert.DeserializeObject<JsonSpeakersTable>(req.text);
      _speakers = FirestoreHelper.ParseSpeakers(st);

//      foreach (var speaker in _speakers.Values)
//      {
//        string url = speaker.PhotoUrl;
//        WWW photoRequest = new WWW(url);
//        StartCoroutine(OnPhotoResponse(photoRequest, speaker));
//      }
    }
  }
}
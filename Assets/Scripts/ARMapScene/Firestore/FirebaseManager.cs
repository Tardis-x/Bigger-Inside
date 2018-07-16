using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.XR.WSA.Persistence;

namespace ua.org.gdg.devfest
{
  public class FirebaseManager : Singleton<FirebaseManager>
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private SpeechScript _speechScript;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      WWW scheduleRequest = new WWW(SCHEDULE_URL + "/" + SCHEDULE_DAY_1_NAME);
      WWW sessionRequest = new WWW(SESSIONS_URL);
      WWW speakerRequest = new WWW(SPEAKERS_URL);

      StartCoroutine(OnScheduleResponse(scheduleRequest));
      StartCoroutine(OnSessionResponse(sessionRequest));
      StartCoroutine(OnSpeakerResponse(speakerRequest));
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void RequestData(out ScheduleDay schedule, out Dictionary<string, Speaker> speakers,
      out List<SessionItem> sessions)
    {
      schedule = _schedule;
      speakers = _speakers;
      sessions = _sessions;
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

    private const string SCHEDULE_DAY_1_NAME = "2016-09-09";

    private ScheduleDay _schedule;
    private List<SessionItem> _sessions;
    private Dictionary<string, Speaker> _speakers;
    private List<ScheduleItemUiModel> _scheduleModels;

    private IEnumerator OnScheduleResponse(WWW req)
    {
      yield return req;

      JsonDocument jScheduleDay = JsonConvert.DeserializeObject<JsonDocument>(req.text);
      _schedule = FirestoreHelper.ParseSchedule(jScheduleDay);
    }

    private IEnumerator OnSessionResponse(WWW req)
    {
      yield return req;

      SessionTable st = JsonConvert.DeserializeObject<SessionTable>(req.text);
      _sessions = FirestoreHelper.ParseSessions(st);
    }

    private IEnumerator OnSpeakerResponse(WWW req)
    {
      yield return req;

      JsonSpeakersTable st = JsonConvert.DeserializeObject<JsonSpeakersTable>(req.text);
      _speakers = FirestoreHelper.ParseSpeakers(st);
    }

    private void MapModels()
    {
      
    }
  }
}
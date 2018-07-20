using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

namespace ua.org.gdg.devfest
{
  public class FirebaseManager : Singleton<FirebaseManager>
  {
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _scheduleRequest = new WWW(SCHEDULE_URL + "/" + SCHEDULE_DAY_1_NAME);
      StartCoroutine(OnScheduleResponse(_scheduleRequest));
      _sessionRequest = new WWW(SESSIONS_URL);
      StartCoroutine(OnSessionResponse(_sessionRequest));
      _speakerRequest = new WWW(SPEAKERS_URL);
      StartCoroutine(OnSpeakerResponse(_speakerRequest));
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public bool RequestHallSchedule(string hall, out List<ScheduleItemUiModel> schedule)
    {
      schedule = _scheduleModels[hall];
      return _modelsMapped;
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    //URLs
    private const string SCHEDULE_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/schedule";

    private const string SESSIONS_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/sessions?pageSize=40";

    private const string SPEAKERS_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/speakers?pageSize=40";

    private const string SCHEDULE_DAY_1_NAME = "2016-09-09";
    
    //Requests
    private WWW _scheduleRequest, _sessionRequest, _speakerRequest;
    
    //Halls
    private const string HALL_EXPO = "Expo";
    private const string HALL_CONFERENCE = "Conference";
    private const string HALL_WORKSHOPS = "Workshops";
    
    //Data
    private ScheduleDay _daySchedule;
    private Dictionary<int, SessionItem> _sessions;
    private Dictionary<string, Speaker> _speakers;
    private Dictionary<string, List<ScheduleItemUiModel>> _scheduleModels;
    private bool _scheduleParsed, _sessionsParsed, _speakersParsed;
    private bool _modelsMapped;
    
    private IEnumerator OnScheduleResponse(WWW req)
    {
      yield return req;
      
      JsonDocument jScheduleDay = JsonConvert.DeserializeObject<JsonDocument>(req.text);
      _daySchedule = FirestoreHelper.ParseSchedule(jScheduleDay);
      _scheduleParsed = true;
      
      if(AreAllRequestsFinished()) MapModels();
    }

    private IEnumerator OnSessionResponse(WWW req)
    {
      yield return req;

      SessionTable st = JsonConvert.DeserializeObject<SessionTable>(req.text);
      _sessions = FirestoreHelper.ParseSessions(st);
      _sessionsParsed = true;
      
      if(AreAllRequestsFinished()) MapModels();
    }

    private IEnumerator OnSpeakerResponse(WWW req)
    {
      yield return req;

      JsonSpeakersTable st = JsonConvert.DeserializeObject<JsonSpeakersTable>(req.text);
      _speakers = FirestoreHelper.ParseSpeakers(st);
      _speakersParsed = true;
      
      if(AreAllRequestsFinished()) MapModels();
    }

    private void MapModels()
    {
      _scheduleModels = new Dictionary<string, List<ScheduleItemUiModel>>();
      _scheduleModels.Add(HALL_EXPO, ComposeScheduleForHall(HALL_EXPO));
      _scheduleModels.Add(HALL_CONFERENCE, ComposeScheduleForHall(HALL_CONFERENCE));
      _scheduleModels.Add(HALL_WORKSHOPS, ComposeScheduleForHall(HALL_WORKSHOPS));
      _modelsMapped = true;
    }
    
    private List<ScheduleItemUiModel> ComposeScheduleForHall(string h)
    {
      // Get all sessions from schedule
      var sList = _daySchedule.Timeslots.SelectMany(x => x.Sessions).Where(s => s.Hall == h).ToList();
      var result = new List<ScheduleItemUiModel>();

      for (int i = 0; i < sList.Count; i++)
      {
        SessionItem s = _sessions[sList[i].Items[0]];
        
        ScheduleItemDescriptionUiModel description = new ScheduleItemDescriptionUiModel
        {
          Complexity = s.Complexity,
          DateReadable = _daySchedule.DateReadable,
          Description = s.Description,
          EndTime = _daySchedule.Timeslots[i].EndTime,
          StartTime = _daySchedule.Timeslots[i].StartTime,
          Hall = sList[i].Hall,
          Language = s.Language,
          Speaker = s.Speakers.Count > 0 ? _speakers[s.Speakers[0]] : null,
          Tag = s.Tag,
          Title = s.Title,
          ImageUrl = s.ImageUrl
        };
        ScheduleItemUiModel model = new ScheduleItemUiModel
        {
          Description = description,
          ImageUrl = s.ImageUrl,
          Speaker = description.Speaker,
          Tag = s.Tag,
          Title = s.Title,
          StartTime = description.StartTime,
          EndTime = description.EndTime
        };
        result.Add(model);
      }

      return result;
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private bool AreAllRequestsFinished()
    {
      return _sessionsParsed && _speakersParsed && _scheduleParsed;
    }
  }
}
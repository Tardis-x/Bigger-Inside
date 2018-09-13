using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

namespace ua.org.gdg.devfest
{
  public class FirestoreManager : Singleton<FirestoreManager>
  {
    //URLs
    private const string SCHEDULE_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/generatedSchedule";
    private const string SESSIONS_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/sessions?pageSize=40";
    private const string SPEAKERS_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/speakers?pageSize=40";

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _scheduleRequest = new WWW(SCHEDULE_URL);
      StartCoroutine(OnScheduleResponse(_scheduleRequest));
      _sessionRequest = new WWW(SESSIONS_URL);
      StartCoroutine(OnSessionResponse(_sessionRequest));
      _speakerRequest = new WWW(SPEAKERS_URL);
      StartCoroutine(OnSpeakerResponse(_speakerRequest));
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public bool RequestFullSchedule(int day, out List<TimeslotModel> schedule)
    {
      if (!AreAllRequestsFinished)
      {
        schedule = null;
        return false;
      }

      schedule = ComposeFullSchedule(day);
      return true;
    }

    public bool RequestFullSchedule(int day, string hall, out List<TimeslotModel> schedule)
    {
      if (!AreAllRequestsFinished)
      {
        schedule = null;
        return false;
      }

      schedule = ComposeFullSchedule(day, hall);
      return true;
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
    
    //Requests
    private WWW _scheduleRequest, _sessionRequest, _speakerRequest;

    //Data
    private Schedule _schedule;
    private Dictionary<int, SessionItem> _sessions;
    private Dictionary<string, Speaker> _speakers;
    private List<TimeslotModel> _fullSchedule;
    private bool _scheduleParsed, _sessionsParsed, _speakersParsed;

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private IEnumerator OnScheduleResponse(WWW req)
    {
      yield return req;

      JsonSchedule schedule = JsonConvert.DeserializeObject<JsonSchedule>(req.text);
      _schedule = FirestoreHelper.ParseSchedule(schedule);
      _scheduleParsed = true;
    }

    private IEnumerator OnSessionResponse(WWW req)
    {
      yield return req;

      SessionTable st = JsonConvert.DeserializeObject<SessionTable>(req.text);
      _sessions = FirestoreHelper.ParseSessions(st);
      _sessionsParsed = true;
    }

    private IEnumerator OnSpeakerResponse(WWW req)
    {
      yield return req;

      JsonSpeakersTable st = JsonConvert.DeserializeObject<JsonSpeakersTable>(req.text);
      _speakers = FirestoreHelper.ParseSpeakers(st);
      _speakersParsed = true;
    }

    private List<TimeslotModel> ComposeFullSchedule(int day)
    {
      var schedule = new List<TimeslotModel>();

      foreach (var ts in _schedule.Days[day - 1].Timeslots)
      {
        var items = ts.Sessions.SelectMany(s => s.Items).ToList();
        string timespan = GetTimespanText(ts.StartTime, ts.EndTime);

        List<SpeechItemModel> speeches = new List<SpeechItemModel>();

        foreach (var item in items)
        {
          if(!_sessions.ContainsKey(item)) continue;
          
          var session = _sessions[item];

          var speaker = session.Speakers.Count > 0 ?
            session.Speakers[0] != null ? 
              _speakers.ContainsKey(session.Speakers[0]) ? 
                _speakers[session.Speakers[0]] : null
            : null : null;

          speeches.Add(new SpeechItemModel
          {
            Timespan = timespan,
            Tag = session.Tag,
            Title = session.Title,
            Speaker = speaker,
            Description = new ScheduleItemDescriptionUiModel
            {
              EndTime = ts.EndTime,
              StartTime = ts.StartTime,
              Tag = session.Tag,
              Description = session.Description,
              Title = session.Title,
              Speaker = speaker,
              Complexity = session.Complexity,
              Hall = ts.Sessions.First(s => s.Items.Contains(item)).Hall,
              Language = session.Language,
              DateReadable = _schedule.Days[day].DateReadable,
              ImageUrl = session.ImageUrl
            }
          });
        }

        schedule.Add(new TimeslotModel
        {
          Items = speeches,
          EndTime = ts.EndTime,
          StartTime = ts.StartTime
        });
      }

      return schedule;
    }

    private List<TimeslotModel> ComposeFullSchedule(int day, string hall)
    {
      var schedule = new List<TimeslotModel>();

      foreach (var ts in _schedule.Days[day - 1].Timeslots)
      {
        var sessions = ts.Sessions.Where(x => x.Hall == hall).ToList();
        var items = sessions.SelectMany(s => s.Items).ToList();
        string timespan = GetTimespanText(ts.StartTime, ts.EndTime);

        List<SpeechItemModel> speeches = new List<SpeechItemModel>();

        for (int i = 0; i < items.Count; i++)
        {
          if(!_sessions.ContainsKey(items[i])) continue;
          
          var session = _sessions[items[i]];

          var speaker = session.Speakers.Count > 0 ?
            session.Speakers[0] != null ? 
              _speakers.ContainsKey(session.Speakers[0]) ? 
                _speakers[session.Speakers[0]] : null
              : null : null;

          speeches.Add(new SpeechItemModel
          {
            Timespan = timespan,
            Tag = session.Tag,
            Title = session.Title,
            Speaker = speaker,
            Description = new ScheduleItemDescriptionUiModel
            {
              EndTime = ts.EndTime,
              StartTime = ts.StartTime,
              Tag = session.Tag,
              Description = session.Description,
              Title = session.Title,
              Speaker = speaker,
              Complexity = session.Complexity,
              Hall = sessions.First(s => s.Items.Contains(items[i])).Hall,
              Language = session.Language,
              DateReadable = _schedule.Days[day].DateReadable,
              ImageUrl = session.ImageUrl
            }
          });
        }

        schedule.Add(new TimeslotModel
        {
          Items = speeches,
          EndTime = ts.EndTime,
          StartTime = ts.StartTime
        });
      }

      return schedule;
    }

    private string GetTimespanText(string startTime, string endTime)
    {
      int startHour = Convert.ToInt32(startTime.Split(':')[0]);
      int endHour = Convert.ToInt32(endTime.Split(':')[0]);
      int startMinute = Convert.ToInt32(startTime.Split(':')[1]);
      int endMinute = Convert.ToInt32(endTime.Split(':')[1]);

      int hourSpan = endHour - startHour;
      int minuteSpan = endMinute - startMinute;

      if (minuteSpan < 0)
      {
        hourSpan--;
        minuteSpan = 60 + minuteSpan;
      }

      string timespanText = "";

      if (hourSpan == 1) timespanText += "1 hour";
      if (hourSpan > 1) timespanText += hourSpan + " hours";
      if (timespanText != "") timespanText += " ";
      if (minuteSpan > 0) timespanText += minuteSpan + " mins";

      return timespanText;
    }

    private bool AreAllRequestsFinished
    {
      get { return _sessionsParsed && _speakersParsed && _scheduleParsed; }
    }
  }
}
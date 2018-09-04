﻿using System;
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
      _scheduleRequest = new WWW(SCHEDULE_URL);// + "/" + SCHEDULE_DAY_1_NAME);
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

    public bool RequestFullSchedule(int day, out List<TimeslotModel> schedule)
    {
      if (!_modelsMapped)
      {
        schedule = null;
        return false;
      }
      schedule = ComposeFullSchedule(day);
      return true;
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
    private const string SCHEDULE_DAY_2_NAME = "2016-09-10";
    
    //Requests
    private WWW _scheduleRequest, _sessionRequest, _speakerRequest;
    
    //Halls
    private const string HALL_EXPO = "Expo";
    private const string HALL_CONFERENCE = "Conference";
    private const string HALL_WORKSHOPS = "Workshops";
    
    //Data
    private ScheduleDay _daySchedule;
    private Schedule _schedule;
    private Dictionary<int, SessionItem> _sessions;
    private Dictionary<string, Speaker> _speakers;
    private Dictionary<string, List<ScheduleItemUiModel>> _scheduleModels;
    private List<TimeslotModel> _fullSchedule;
    private bool _scheduleParsed, _sessionsParsed, _speakersParsed;
    private bool _modelsMapped;
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    private IEnumerator OnScheduleResponse(WWW req)
    {
      yield return req;
      
      JsonSchedule schedule = JsonConvert.DeserializeObject<JsonSchedule>(req.text);
      _schedule = FirestoreHelper.ParseSchedule(schedule);
      _daySchedule = _schedule.Days[0];
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
      _scheduleModels = new Dictionary<string, List<ScheduleItemUiModel>>
      {
        {HALL_EXPO, ComposeScheduleForHall(HALL_EXPO)},
        {HALL_CONFERENCE, ComposeScheduleForHall(HALL_CONFERENCE)},
        {HALL_WORKSHOPS, ComposeScheduleForHall(HALL_WORKSHOPS)}
      };
      
      _modelsMapped = true;
    }

    private List<TimeslotModel> ComposeFullSchedule(int day)
    {
      var schedule = new List<TimeslotModel>();

      foreach (var ts in _schedule.Days[day - 1].Timeslots)
      {
        var items = ts.Sessions.SelectMany(s => s.Items);
        string timespan = GetTimespanText(ts.StartTime, ts.EndTime);
        
        List<SpeechItemModel> speeches = new List<SpeechItemModel>();
        
        foreach (var item in items)
        {
          var session = _sessions[item];
          
          speeches.Add(new SpeechItemModel
          {
            Timespan = timespan,
            Tag = session.Tag,
            Title = session.Title,
            Speaker = session.Speakers.Count > 0 ? _speakers[session.Speakers[0]] : null,
            Description = new ScheduleItemDescriptionUiModel
            {
              EndTime = ts.EndTime,
              StartTime = ts.StartTime,
              Tag = session.Tag,
              Description = session.Description,
              Title = session.Title,
              Speaker = session.Speakers.Count > 0 ? _speakers[session.Speakers[0]] : null,
              Complexity = session.Complexity,
              Hall = ts.Sessions.First(s => s.Items.Contains(item)).Hall,
              Language = session.Language,
              DateReadable = _daySchedule.DateReadable,
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
      if(minuteSpan > 0) timespanText += minuteSpan + " mins";

      return timespanText;
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

    private bool AreAllRequestsFinished()
    {
      return _sessionsParsed && _speakersParsed && _scheduleParsed;
    }
  }
}
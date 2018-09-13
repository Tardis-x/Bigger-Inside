using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace ua.org.gdg.devfest
{
  public class FirestoreManager : Singleton<FirestoreManager>
  {
    //URLs
    private const string SCHEDULE_URL =
      "https://firestore.googleapis.com/v1beta1/projects/hoverboard-v2-dev/databases/(default)/documents/generatedSchedule";

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _scheduleRequest = new WWW(SCHEDULE_URL);
      StartCoroutine(OnScheduleResponse(_scheduleRequest));
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public bool RequestFullSchedule(int day, out List<TimeslotModel> schedule)
    {
      if (!_scheduleParsed)
      {
        schedule = null;
        return false;
      }

      schedule = ComposeFullSchedule(day);
      return true;
    }

    public bool RequestFullSchedule(int day, string hall, out List<TimeslotModel> schedule)
    {
      if (!_scheduleParsed)
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
    private bool _scheduleParsed;

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

    private List<TimeslotModel> ComposeFullSchedule(int day)
    {
      var schedule = new List<TimeslotModel>();

      foreach (var ts in _schedule.Days[day - 1].Timeslots)
      {
        List<SpeechItemModel> speeches = new List<SpeechItemModel>();

        foreach (var item in ts.Sessions)
        {
          var speaker = item.Speakers != null ? 
            item.Speakers.Count > 0 ? 
              item.Speakers[0] : null : null;
          
          speeches.Add(new SpeechItemModel
          {
            Timespan = GetTimespanText(item.Duration.Hours, item.Duration.Minutes),
            Tag = item.Tag,
            Title = item.Title,
            Speaker = speaker,
            Description = new ScheduleItemDescriptionUiModel
            {
              EndTime = item.EndTime,
              StartTime = item.StartTime,
              Tag = item.Tag,
              Description = item.Description,
              Title = item.Title,
              Speaker = speaker,
              Complexity = item.Complexity,
              Hall = item.Hall,
              Language = item.Language,
              DateReadable = item.DateReadable
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
        List<SpeechItemModel> speeches = new List<SpeechItemModel>();

        var items = ts.Sessions.Where(s => s.Hall == hall);
        
        foreach (var item in items)
        {
          var speaker = item.Speakers != null ? 
            item.Speakers.Count > 0 ? 
              item.Speakers[0] : null : null;
          
          speeches.Add(new SpeechItemModel
          {
            Timespan = GetTimespanText(item.Duration.Hours, item.Duration.Minutes),
            Tag = item.Tag,
            Title = item.Title,
            Speaker = speaker,
            Description = new ScheduleItemDescriptionUiModel
            {
              EndTime = item.EndTime,
              StartTime = item.StartTime,
              Tag = item.Tag,
              Description = item.Description,
              Title = item.Title,
              Speaker = speaker,
              Complexity = item.Complexity,
              Hall = item.Hall,
              Language = item.Language,
              DateReadable = item.DateReadable
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

    private string GetTimespanText(int hrs, int mins)
    {
      var b = new StringBuilder();
      if (hrs > 0) b.Append(hrs);
      b.Append(mins);

      return b.ToString();
    }
  }
}
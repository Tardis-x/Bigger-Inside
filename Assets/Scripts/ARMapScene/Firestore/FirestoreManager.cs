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
    private const string SCHEDULE_URL = Credentials.FIREBASE_URL;

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

    public bool RequestFullSchedule(out List<TimeslotModel> day1, out List<TimeslotModel> day2)
    {
      if (!_scheduleParsed)
      {
        day1 = null;
        day2 = null;
        return false;
      }

      day1 = ComposeFullSchedule(1);
      day2 = ComposeFullSchedule(2);
      return true;
    }
    
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
      if (_schedule == null)
      {
        _scheduleRequest = new WWW(SCHEDULE_URL);
        StartCoroutine(OnScheduleResponse(_scheduleRequest));
        return null;
      }
      
      var schedule = new List<TimeslotModel>();

      foreach (var ts in _schedule.Days[day - 1].Timeslots)
      {
        var items = ts.Sessions;
        
        var speeches = (from item in items
          
          let speaker = item.Speakers != null ?
            item.Speakers.Count > 0 ? 
              item.Speakers[0] : null : null
          
          select new SpeechItemModel
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
          }).ToList();

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
        var items = ts.Sessions.Where(s => s.Hall == hall);

        var speeches = (from item in items
          
        let speaker = item.Speakers != null ?
          item.Speakers.Count > 0 ? 
            item.Speakers[0] : null : null
          
        select new SpeechItemModel
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
        }).ToList();

        schedule.Add(new TimeslotModel
        {
          Items = speeches,
          EndTime = ts.EndTime,
          StartTime = ts.StartTime
        });
      }

      return schedule;
    }

    private static string GetTimespanText(int hrs, int mins)
    {
      var b = new StringBuilder();
      
      if (hrs > 0)
      {
        b.Append(hrs);
        b.Append(hrs > 1 ? " hours " : " hour ");
      }

      if (mins <= 0) return b.ToString();
      
      b.Append(mins);
      b.Append(" mins");

      return b.ToString();
    }
  }
}
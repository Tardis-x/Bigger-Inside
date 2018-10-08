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
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private WWW _scheduleRequest;
    
    private Dictionary<int, SessionItem> _sessions;
    private Dictionary<string, Speaker> _speakers;
    
    private List<TimeslotModel> _fullSchedule;
    private Schedule _schedule;
    
    private bool _scheduleParsed;
    
    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public bool Error { get; private set; }


    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void OnEnable()
    {
      if (_scheduleParsed) return;

      Error = false;
      _scheduleRequest = new WWW(Credentials.FIREBASE_URL);
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
    // Helpers
    //---------------------------------------------------------------------

    private IEnumerator OnScheduleResponse(WWW req)
    {
      yield return req;

      if (!string.IsNullOrEmpty(req.error))
      {
        HandleRequestError();
      }
      else
      {
        var schedule = JsonConvert.DeserializeObject<JsonSchedule>(req.text);
        
        _schedule = FirestoreHelper.ParseSchedule(schedule);
        FirestoreCache.SaveSchedule(_schedule);
        
        OnScheduleExist();
      }
    }

    private void HandleRequestError()
    {
      var isScheduleCached = GetScheduleFromCache();

      if (isScheduleCached) return;
      
      Utils.ShowMessage("No internet connection");
      Error = true;
    }

    private bool GetScheduleFromCache()
    {
      _schedule = FirestoreCache.GetSchedule();

      if (_schedule == null) return false;
      
      OnScheduleExist();
      return true;
    }

    private void OnScheduleExist()
    {
      Error = false;
      _scheduleParsed = true;
    }

    private List<TimeslotModel> ComposeFullSchedule(int day)
    {
      if (_schedule == null)
      {
        _scheduleRequest = new WWW(Credentials.FIREBASE_URL);
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
              item.Speakers : null : null
          
          select new SpeechItemModel
          {
            Timespan = GetTimespanText(item.Duration.Hours, item.Duration.Minutes),
            MainTag = item.Tags == null ? "General" : item.Tags[0],
            Tags = item.Tags,
            Title = item.Title,
            Speakers = speaker == null ? null : speaker.ToArray(),
            Description = new ScheduleItemDescriptionUiModel
            {
              EndTime = item.EndTime,
              StartTime = item.StartTime,
              MainTag = item.Tags == null ? "General" : item.Tags[0],
              Tags = item.Tags,
              Description = item.Description,
              Title = item.Title,
              Speakers = speaker == null ? null : speaker.ToArray(),
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
            item.Speakers : null : null
          
        select new SpeechItemModel
        {
          Timespan = GetTimespanText(item.Duration.Hours, item.Duration.Minutes),
          MainTag = item.Tags == null ? "General" : item.Tags[0],
          Title = item.Title,
          Speakers = speaker == null ? null : speaker.ToArray(),
          Description = new ScheduleItemDescriptionUiModel
          {
            EndTime = item.EndTime,
            StartTime = item.StartTime,
            MainTag = item.Tags == null ? "General" : item.Tags[0],
            Description = item.Description,
            Title = item.Title,
            Speakers = speaker == null ? null : speaker.ToArray(),
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
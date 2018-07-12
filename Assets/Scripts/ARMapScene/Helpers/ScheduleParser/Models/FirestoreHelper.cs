using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase.Storage;

namespace ua.org.gdg.devfest
{
  public class FirestoreHelper
  {
    public static Schedule ParseSchedule(JsonSchedule js)
    {
      Schedule sch = new Schedule();

      foreach (var day in js.documents)
      {
        ScheduleDay newDay = new ScheduleDay();
        newDay.Date = day.fields.date.stringValue;
        newDay.DateReadable = day.fields.dateReadable.stringValue;
        
        foreach (var timeslot in day.fields.timeslots.arrayValue.values)
        {
          Timeslot ts = new Timeslot();

          ts.EndTime = timeslot.mapValue.fields.endTime.stringValue;
          ts.StartTime = timeslot.mapValue.fields.startTime.stringValue;

          foreach (var tsSession in timeslot.mapValue.fields.sessions.arrayValue.values)
          {
            Session session = new Session();

            foreach (var item in tsSession.mapValue.fields.items.arrayValue.values)
            {
              session.Items.Add(Convert.ToInt32(item.integerValue));
            }

            session.SetHall(Array.IndexOf(timeslot.mapValue.fields.sessions.arrayValue.values.ToArray(), tsSession));
            ts.Sessions.Add(session);
          }

          newDay.Timeslots.Add(ts);
        }

        sch.Days.Add(newDay);
      }

      return sch;
    }
    
    public static List<SessionItem> ParseSessions(SessionTable st)
    {
      List<SessionItem> result = new List<SessionItem>();
      
      foreach (var s in st.documents)
      {
        SessionItem item = new SessionItem();
        item.Title = s.fields.title.stringValue;
        item.Name = Convert.ToInt32(s.name.Split('/').Last());
        item.Tag = s.fields.tags == null? "General" : s.fields.tags.arrayValue.values.First().stringValue;
        item.ImageUrl = s.fields.image != null ? s.fields.image.stringValue : null;
        
        if (s.fields.speakers != null)
        {
          foreach (var speaker in s.fields.speakers.arrayValue.values)
          {
            item.Speakers.Add(speaker.stringValue);
          }
        }

        result.Add(item);
      }

      return result;
    }

    public static Dictionary<string, Speaker> ParseSpeakers(JsonSpeakersTable jst)
    {
      Dictionary<string, Speaker> speakers = new Dictionary<string, Speaker>();

      foreach (var js in jst.documents)
      {
        Speaker s = new Speaker();
        s.Name = js.fields.name.stringValue;
        s.Company = js.fields.company.stringValue;
        s.Country = js.fields.country.stringValue;
        s.PhotoUrl = js.fields.photoUrl.stringValue;
        
        speakers.Add(js.name.Split('/').Last(), s);
      }

      return speakers;
    }

    public static List<RectTransform> ComposeScheduleForHall(Hall h, int day, Schedule sch,
      List<SessionItem> sessions, Dictionary<string, Speaker> speakers, SpeechScript ss)
    {
      // Get all sessions from schedule
      var sList = sch.Days[day].Timeslots.SelectMany(x => x.Sessions).Where(s => s.Hall == h).ToList();
      var result = new List<RectTransform>();

      for (int i = 0; i < sList.Count; i++)
      {
        SessionItem s = sessions.Find(x => sList[i].Items.Contains(x.Name));
        result.Add(ss.GetInstance(sch.Days[day].Timeslots[i].StartTime,
          sch.Days[day].Timeslots[i].EndTime, s.Title, s.Tag, 
          s.Speakers.Count > 0 ? speakers[s.Speakers[0]] : null, s.ImageUrl ?? ""));
      }

      return result;
    }
  }
}
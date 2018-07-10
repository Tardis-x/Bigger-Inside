using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    
    public static Dictionary<int, string> ParseSessions(SessionTable st)
    {
      Dictionary<int, string> result = new Dictionary<int, string>();
      
      foreach (var s in st.documents)
      {
//        SessionItem item = new SessionItem();
//        item.Title = s.fields.title.stringValue;
//        item.Name = Convert.ToInt32(s.name.Split('/').Last());
        result.Add(Convert.ToInt32(s.name.Split('/').Last()), s.fields.title.stringValue);
      }

      return result;
    }

    public static List<RectTransform> ComposeScheduleForHall(Hall h, int day, Schedule sch,
      Dictionary<int, string> sessions, SpeechScript ss)
    {
      var sList = sch.Days[day].Timeslots.SelectMany(x => x.Sessions).Where(s => s.Hall == h).ToList();
      var result = new List<RectTransform>();

      for (int i = 0; i < sList.Count; i++)
      {
        result.Add(ss.GetInstance(sch.Days[day].Timeslots[i].StartTime,
          sch.Days[day].Timeslots[i].EndTime, sessions[sList[i].Items[0]]));
      }

      return result;
    }
  }
}
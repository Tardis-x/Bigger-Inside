using System;
using System.Collections.Generic;
using System.Linq;

namespace ua.org.gdg.devfest
{
  public class FirestoreHelper
  {
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public static Schedule ParseSchedule(JsonSchedule sch)
    {
      Schedule result = new Schedule();

      foreach (var day in sch.documents)
      {
        result.Days.Add(ParseScheduleDay(day));
      }

      return result;
    }

    public static Dictionary<int, SessionItem> ParseSessions(SessionTable st)
    {
      Dictionary<int, SessionItem> result = new Dictionary<int, SessionItem>();

      foreach (var s in st.documents)
      {
        SessionItem item = new SessionItem();
        item.Title = s.fields.title.stringValue;
        item.Id = Convert.ToInt32(s.name.Split('/').Last());
        item.Tag = s.fields.tags == null ? "General" : s.fields.tags.arrayValue.values.First().stringValue;
        item.ImageUrl = s.fields.image != null ? s.fields.image.stringValue : null;
        item.Complexity = s.fields.complexity != null ? s.fields.complexity.stringValue : "";
        item.Language = s.fields.language != null ? s.fields.language.stringValue : "";
        item.Description = s.fields.description.stringValue;

        if (s.fields.speakers != null)
        {
          foreach (var speaker in s.fields.speakers.arrayValue.values)
          {
            item.Speakers.Add(speaker.stringValue);
          }
        }

        result.Add(item.Id, item);
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
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private static ScheduleDay ParseScheduleDay(JsonDocument jday)
    {
      ScheduleDay day = new ScheduleDay();

      day.Date = jday.fields.date.stringValue;
      day.DateReadable = jday.fields.dateReadable.stringValue;

      foreach (var timeslot in jday.fields.timeslots.arrayValue.values)
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

          session.Extend = tsSession.mapValue.fields.extend != null
            ? Convert.ToInt32(tsSession.mapValue.fields.extend.integerValue)
            : 1;
          session.SetHall(Array.IndexOf(timeslot.mapValue.fields.sessions.arrayValue.values.ToArray(), tsSession));
          ts.Sessions.Add(session);
        }

        day.Timeslots.Add(ts);
      }

      return day;
    }
  }
}
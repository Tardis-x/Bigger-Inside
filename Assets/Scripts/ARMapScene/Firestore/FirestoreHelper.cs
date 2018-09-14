using System;
using System.Text.RegularExpressions;

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
          foreach (var item in tsSession.mapValue.fields.items.arrayValue.values)
          {
            ts.Sessions.Add(ParseSessionItem(item));
          }
        }

        day.Timeslots.Add(ts);
      }

      return day;
    }

    private static SessionItem ParseSessionItem(JsonValue<JsonSessionItem> jSession)
    {
      var session = new SessionItem
      {
        Id = jSession.mapValue.fields.id != null ?
          jSession.mapValue.fields.id.stringValue : null,
        Description = jSession.mapValue.fields.description != null ?
          Regex.Replace(jSession.mapValue.fields.description.stringValue,
            @"\p{Cs}|[\u2600-\u27ff]", "●").Replace("●●", "●") : null,
        Title = jSession.mapValue.fields.title != null ?
          jSession.mapValue.fields.title.stringValue : null,
        Tag = jSession.mapValue.fields.mainTag != null ?
          jSession.mapValue.fields.mainTag.stringValue : null,
        Complexity = jSession.mapValue.fields.complexity != null ? 
          jSession.mapValue.fields.complexity.stringValue : null,
        Language = jSession.mapValue.fields.language != null ? 
          jSession.mapValue.fields.language.stringValue : null,
        Hall = jSession.mapValue.fields.track.mapValue.fields.title.stringValue,
        StartTime = jSession.mapValue.fields.startTime.stringValue,
        EndTime = jSession.mapValue.fields.endTime.stringValue,
        DateReadable = jSession.mapValue.fields.dateReadable.stringValue,
        Duration = new Duration
        {
          Hours = Convert.ToInt32(jSession.mapValue.fields.duration.mapValue.fields.hh.integerValue),
          Minutes = Convert.ToInt32(jSession.mapValue.fields.duration.mapValue.fields.mm.integerValue)
        }
      };

      if (jSession.mapValue.fields.speakers.arrayValue.values == null) return session;
      
      foreach (var speaker in jSession.mapValue.fields.speakers.arrayValue.values)
      {
        session.Speakers.Add(ParseSpeaker(speaker));
      }

      return session;
    }

    private static Speaker ParseSpeaker(JsonValue<JsonSpeakerFields> jSpeaker)
    {
      return new Speaker
      {
        Id = jSpeaker.mapValue.fields.id.stringValue,
        PhotoUrl = jSpeaker.mapValue.fields.photoUrl.stringValue,
        Company = jSpeaker.mapValue.fields.company.stringValue,
        Country = jSpeaker.mapValue.fields.country.stringValue,
        Name = jSpeaker.mapValue.fields.name.stringValue
      };
    }
  }
}
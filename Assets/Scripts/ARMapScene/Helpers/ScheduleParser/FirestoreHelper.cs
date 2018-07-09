using System;

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
  }
}
using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class Schedule
  {
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------
    public List<ScheduleDay> Days { get; set; }

    public Schedule()
    {
      Days = new List<ScheduleDay>();
    }
  }
}
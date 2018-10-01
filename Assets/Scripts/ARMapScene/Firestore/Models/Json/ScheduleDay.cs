using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class ScheduleDay
  {
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------
    
    public string Date { get; set; }
    public string DateReadable { get; set; }
    public List<Timeslot> Timeslots { get; set; }

    public ScheduleDay()
    {
      Timeslots = new List<Timeslot>();
    }
  }
}
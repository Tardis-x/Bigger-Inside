using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  public class Timeslot
  {
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------
    
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public List<Session> Sessions { get; set; }

    public Timeslot()
    {
      Sessions = new List<Session>();
    }
  }
}
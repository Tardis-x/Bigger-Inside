using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class Timeslot
  {
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------
    
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public List<SessionItem> Sessions { get; set; }

    public Timeslot()
    {
      Sessions = new List<SessionItem>();
    }
  }
}
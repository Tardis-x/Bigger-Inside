using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  public class Session
  {
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------
    
    public Hall Hall { get; set; }
    public List<int> Items { get; set; }

    public Session()
    {
      Items = new List<int>();
    }

    public void SetHall(int hall)
    {
      switch (hall)
      {
          case 0:
            Hall = Hall.Expo;
            break;
          case 1:
            Hall = Hall.Conference;
            break;
          case 2:
            Hall = Hall.Workshops;
            break;
          default :
            throw new Exception("Unknown hall.");
      }
    }
  }
}
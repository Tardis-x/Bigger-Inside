using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  public class Session
  {
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public int Extend { get; set; }
    public string Hall { get; set; }
    public List<Tag> Tags { get; set; }
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
          Hall = "Stage 1";
          break;
        case 1:
          Hall = "Stage 2";
          break;
        case 2:
          Hall = "Stage 3";
          break;
        case 3:
          Hall = "Workshops hall";
          break;
        default:
          throw new Exception("Unknown hall.");
      }
    }
  }
}
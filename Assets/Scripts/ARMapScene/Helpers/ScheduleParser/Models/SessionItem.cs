using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  public class SessionItem
  {
    public int Name { get; set; }
    public string Title { get; set; }
    public string Tag { get; set; }
    public List<string> Speakers { get; set; }

    public SessionItem()
    {
      Speakers = new List<string>();
    }
  }
}
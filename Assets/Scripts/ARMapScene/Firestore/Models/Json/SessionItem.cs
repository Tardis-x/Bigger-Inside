using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  public class SessionItem
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Tag { get; set; }
    public List<string> Speakers { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public string Complexity { get; set; }
    public string Language { get; set; }

    public SessionItem()
    {
      Speakers = new List<string>();
    }
  }
}
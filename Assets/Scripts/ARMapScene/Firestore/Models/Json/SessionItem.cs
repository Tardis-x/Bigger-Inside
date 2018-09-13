using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  public class SessionItem
  {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Tag { get; set; }
    public List<Speaker> Speakers { get; set; }
    public string Description { get; set; }
    public string Complexity { get; set; }
    public string Language { get; set; }
    public string Hall { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public Duration Duration { get; set; }
    public string DateReadable { get; set; }

    public SessionItem()
    {
      Speakers = new List<Speaker>();
    }
  }
}
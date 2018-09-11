using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  public class TimeslotModel
  {
    public string StartTime;
    public string EndTime;
    public List<SpeechItemModel> Items;

    public TimeslotModel()
    {
      Items = new List<SpeechItemModel>();
    }
  }
}
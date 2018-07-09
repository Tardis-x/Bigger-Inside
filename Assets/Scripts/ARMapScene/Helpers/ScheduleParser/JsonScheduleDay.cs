using System;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonScheduleDay
  {
    public string name;
    public ScheduleDayFields fields;
    public string createTime;
    public string updateTime;
  }
}
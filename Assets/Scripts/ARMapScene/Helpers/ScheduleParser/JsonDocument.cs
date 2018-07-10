using System;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonDocument
  {
    public string name;
    public ScheduleDayFields fields;
    public string createTime;
    public string updateTime;
  }
}
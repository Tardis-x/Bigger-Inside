using System;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonTimeslot
  {
    public StringValue startTime;
    public StringValue endTime;
    public JsonArray<JsonTimeslotSession> sessions;
  }
}
using System;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class ScheduleDayFields
  {
    public JsonArray<JsonTimeslot> timeslots;
    public JsonArray<JsonTrack> tracks;
    public StringValue date;
    public StringValue dateReadable;
  }
}
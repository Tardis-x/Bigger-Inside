using System;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonSessionItem
  {
    public StringValue id;
    public StringValue title;
    public StringValue startTime;
    public StringValue endTime;
    public JsonTag tags;
    public StringValue dateReadable;
    public StringValue description;
    public StringValue language;
    public StringValue complexity;
    public JsonArray<JsonSpeakerFields> speakers;
    public JsonValue<JsonTrack> track;
    public JsonValue<JsonDurationFields> duration;
  }
}
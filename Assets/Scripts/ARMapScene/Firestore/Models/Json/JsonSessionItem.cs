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
    public StringValue mainTag;
    public StringValue dateReadable;
    public StringValue description;
    public StringValue language;
    public StringValue complexity;
    public JsonArray<JsonSpeakerFields> speakers;
    public Value<JsonTrack> track;
    public Value<JsonDurationFields> duration;
  }
}
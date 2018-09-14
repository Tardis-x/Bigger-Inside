using System;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonTimeslotSession
  {
    public JsonArray<JsonSessionItem> items;
  }
}
using System;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonArray<T>
  {
    public JsonArrayValue<T> arrayValue;
  }
}
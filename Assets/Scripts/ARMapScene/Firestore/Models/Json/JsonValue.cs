using System;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonValue<T>
  {
    public JsonMapValue<T> mapValue;
  }
}
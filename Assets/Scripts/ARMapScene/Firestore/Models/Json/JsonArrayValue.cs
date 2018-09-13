using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonArrayValue<T>
  {
    public List<JsonValue<T>> values;
  }
}
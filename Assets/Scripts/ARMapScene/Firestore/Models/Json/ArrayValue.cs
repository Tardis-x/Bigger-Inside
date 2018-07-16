using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class ArrayValue<T>
  {
    public List<Value<T>> values;
  }
}
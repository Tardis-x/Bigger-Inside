using System;
using System.Collections.Generic;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonTag
  {
    public TagsArray arrayValue;
  }

  [Serializable]
  public class TagsArray
  {
    public List<StringValue> values;
  }
}
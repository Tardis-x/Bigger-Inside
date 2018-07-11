using System;



namespace ua.org.gdg.devfest
{
  [Serializable]
  public class JsonSessionFields
  {
    public StringValue title;
    public JsonTag tags;
    public JsonTag speakers;
  }
}
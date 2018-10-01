using System;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class Speaker
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Company { get; set; }
    public string Country { get; set; }
    public string PhotoUrl { get; set; }
  }
}
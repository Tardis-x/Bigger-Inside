using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class SponsorModel
  {
    [SerializeField] private string _id;
    [SerializeField] private string _name;

    public string Id
    {
      get { return _id; }
      set { _id = value; }
    }

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
  }
}
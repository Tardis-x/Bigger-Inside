using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [Serializable]
  public class SponsorLogo
  {
    [SerializeField] private string _name;
    [SerializeField] private Sprite _logo;
    [SerializeField] private bool _hideName;

    public Sprite Logo
    {
      get { return _logo; }
      set { _logo = value; }
    }

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public bool HideName
    {
      get { return _hideName; }
      set { _hideName = value; }
    }
  }
}
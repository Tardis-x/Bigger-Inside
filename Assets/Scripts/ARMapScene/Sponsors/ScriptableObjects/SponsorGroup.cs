using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "AR Map/Sponsor Group")]
  public class SponsorGroup : ScriptableObject
  {
    [SerializeField] private List<SponsorModel> _sponsorModels;
    [SerializeField] private List<string> _sponsorKeys;
    
    public List<SponsorModel> SponsorModels
    {
      get { return _sponsorModels; }
      set
      {
        _sponsorModels = value; 
      }
    }

    public List<string> SponsorKeys
    {
      get { return _sponsorKeys; }
    }
  }
}
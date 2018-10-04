using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "AR Map/Sponsor Logo")]
  public class SponsorLogoData : ScriptableObject
  {
    [SerializeField] private List<SponsorLogo> _sponsorLogos;

    public List<SponsorLogo> SponsorLogos
    {
      get { return _sponsorLogos; }
    }
  }
}
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class InfoCoinElement : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private string _sponsorId;
    
    //---------------------------------------------------------------------
    // Property
    //---------------------------------------------------------------------

    public string SponsorId
    {
      get { return _sponsorId; }
    }
  }
}
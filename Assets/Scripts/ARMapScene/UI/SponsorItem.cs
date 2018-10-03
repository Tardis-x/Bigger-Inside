using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class SponsorItem : MonoBehaviour
  {
    
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Text _sponsorName;
    [SerializeField] private Image _logo;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void SetSponsorModel(SponsorModel sponsorModel)
    {
      _sponsorName.text = sponsorModel.Name;
    }
    
    public void SetSponsorLogo(SponsorLogo sponsorLogo)
    {
      _sponsorName.gameObject.SetActive(!sponsorLogo.HideName);

      _logo.sprite = sponsorLogo.Logo;
      _logo.gameObject.SetActive(sponsorLogo.Logo != null);
    }
  }
}
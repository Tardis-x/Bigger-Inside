using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class NavigationResolver : MonoBehaviour
  {

    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    
    [Header("Markers")] 
    [SerializeField] private GameObject _wcMarker;
    [SerializeField] private GameObject _pressWallMarker;
    [SerializeField] private GameObject _partnersZoneMarker;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void SetupNavigationTarget(NavigationTargets navigationTarget)
    {
      switch (navigationTarget)
      {
          case NavigationTargets.WC:
            _wcMarker.SetActive(true);
            break;
          case NavigationTargets.PartnersZone:
            _pressWallMarker.SetActive(true);
            break;
          case NavigationTargets.PressWall:
            _partnersZoneMarker.SetActive(true);
            break;
      }
    }

  }
}

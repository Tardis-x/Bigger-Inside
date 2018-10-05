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
            SetActiveWcMarker();
            break;
          case NavigationTargets.PartnersZone:
            SetActivePartnersZoneMarker();
            break;
          case NavigationTargets.PressWall:
            SetActivePressWallMarker();
            break;
          case NavigationTargets.None:
            DeactivateMarkers();
            break;
          default:
            DeactivateMarkers();
            break;
      }
    }
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private void SetActiveWcMarker()
    {
      _wcMarker.SetActive(true);
      _pressWallMarker.SetActive(false);
      _partnersZoneMarker.SetActive(false);
    }

    private void SetActivePressWallMarker()
    {
      _wcMarker.SetActive(false);
      _pressWallMarker.SetActive(true);
      _partnersZoneMarker.SetActive(false);
    }

    private void SetActivePartnersZoneMarker()
    {
      _wcMarker.SetActive(false);
      _pressWallMarker.SetActive(false);
      _partnersZoneMarker.SetActive(true);
    }

    private void DeactivateMarkers()
    {
      _wcMarker.SetActive(false);
      _pressWallMarker.SetActive(false);
      _partnersZoneMarker.SetActive(false);
    }
  }
}

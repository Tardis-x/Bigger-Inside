using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class NavigationResolver : MonoBehaviour
  {

    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("UI")] 
    [SerializeField] private GameObject _icons;

    [Space] 
    [Header("Markers")] 
    [SerializeField] private GameObject _wcMarker;
    [SerializeField] private GameObject _hallMarker;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void SetupNavigationTarget(NavigationTargets navigationTarget)
    {
      _icons.SetActive(false);
      
      switch (navigationTarget)
      {
          case NavigationTargets.WC:
            _wcMarker.SetActive(true);
            break;
          case NavigationTargets.Hall:
            _hallMarker.SetActive(true);
            break;
      }
    }

  }
}

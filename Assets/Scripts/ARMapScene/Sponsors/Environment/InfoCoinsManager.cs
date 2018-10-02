using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class InfoCoinsManager : MonoBehaviour
  {

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private ISelectable _lastSelected;
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OnCoinGroupSelected(InfoCoinGroup infoCoinGroup)
    {
      UpdateSelection(infoCoinGroup);
    }
    
    public void OnCoinSelected(InfoCoin infoCoin)
    {
      UpdateSelection(infoCoin);
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void UpdateSelection(ISelectable selectable)
    {
      if (_lastSelected != null) _lastSelected.Deselect();
      _lastSelected = selectable;
    }
  }
}
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

      if (infoCoinGroup.SponsorWithNameList.Count == 0)
      {
        PanelManager.Instance.ShowCoinNamePanel("Partners Area"); 
      }
      else
      {
        PanelManager.Instance.ShowInfoCoinGroupPanel(infoCoinGroup);
      }
    }
    
    public void OnCoinSelected(InfoCoin infoCoin)
    {
      UpdateSelection(infoCoin);

      if (infoCoin.HasSchedule)
      {
        PanelManager.Instance.ShowCoinSchedulePanel(infoCoin.Name, infoCoin.ButtonName);
      }
      else
      {
        PanelManager.Instance.ShowCoinNamePanel(infoCoin.Name);
      }
    }

    public void DeselectLastItem()
    {
      if (_lastSelected != null) _lastSelected.Deselect();
      _lastSelected = null;
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
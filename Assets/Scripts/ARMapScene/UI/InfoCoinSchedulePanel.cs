using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class InfoCoinSchedulePanel : MonoBehaviour
  {

    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Text _buttonText;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private string _hallName;
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public void OpenPanel(string hall)
    {
      _hallName = hall;
      _buttonText.text = string.Format("Schedule ({0})", hall); 
      
      gameObject.SetActive(true);
    }

    public void ShowSchedule()
    {
      PanelManager.Instance.ShowSchedulePanel(_hallName);
    }
    
    
  }
}
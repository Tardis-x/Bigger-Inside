using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class InfoCoinNamePanel : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Text _coinNameText;
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OpenPanel(string coinName)
    {
      _coinNameText.text = coinName;
      gameObject.SetActive(true);
    }
  }
}
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class TowerPriceScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("UI")] [SerializeField] private Text _priceText;
    [SerializeField] private DragAndDropTower _buyButton;

    [Space] [Header("Variables")] [SerializeField]
    private IntReference _towerPrice;

    [SerializeField] private IntReference _money;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void OnEnable()
    {
      _priceText.text = _towerPrice.Value.ToString();
      UpdatePanel();
    }

    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnUIUpdateRequested()
    {
      UpdatePanel();
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private void UpdatePanel()
    {
      bool canAfford = _money.Value >= _towerPrice.Value;
      _buyButton.Interactable = canAfford;
      _priceText.color = canAfford ? Color.white : Color.red;
    }
  }
}
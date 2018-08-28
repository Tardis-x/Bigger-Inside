using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class UIEventListener : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Text")] 
    [SerializeField] private Text _moneyText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _enemiesLeftText;

    [Space] 
    [Header("Panels")] 
    [SerializeField] private TowerUpgradePanelScript _upgradePanel;
    [SerializeField] private GameObject _towerPanel;
    [SerializeField] private GameObject _moneyPanel;
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private GameObject _playButton;
    
    [Space]
    [Header("Public Variables")]
    [SerializeField] private IntReference _money;
    [SerializeField] private IntReference _score;
    [SerializeField] private IntReference _enemiesLeft;

    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnTowerSelected(GameObject tower)
    {
      var towerScript = tower.GetComponent<TowerScript>();
      _upgradePanel.SelectedTower = towerScript;
      _upgradePanel.SetUpgradeCostText(towerScript.UpgradeCost);
      _upgradePanel.SetSellCostText(towerScript.SellCost);
      
      _towerPanel.SetActive(false);
      _upgradePanel.gameObject.SetActive(true);
    }

    public void OnTowerDeselected()
    {
      _towerPanel.SetActive(true);
      _upgradePanel.gameObject.SetActive(false);
    }

    public void OnUIUpdateRequested()
    {
      UpdateUI();
    }

    public void OnGameOver()
    {
      
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void UpdateUI()
    {
      _moneyText.text = _money.Value.ToString();
      _scoreText.text = _score.Value.ToString();
      _enemiesLeftText.text = _enemiesLeft.Value.ToString();
    }
  }
}
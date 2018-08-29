using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
  public class UIEventListener : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Stats panels")] 
    [SerializeField] private StatsPanel _moneyPanel;
    [SerializeField] private StatsPanel _scorePanel;
    [SerializeField] private StatsPanel _enemiesLeftPanel;

    [Space] 
    [Header("Panels")] 
    [SerializeField] private TowerUpgradePanelScript _upgradePanel;
    [SerializeField] private RectTransform _towerPanel;
    [SerializeField] private HallUnlockPanelScript _hallUnlockPanel;
    [SerializeField] private TutorialPanelScript _tutorialPanel;
    [SerializeField] private TDGameOverPanelScript _gameOverPanel;

    [Space]
    [Header("Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _replayButton;
    
    [Space]
    [Header("Public Variables")]
    [SerializeField] private IntReference _money;
    [SerializeField] private IntReference _score;
    [SerializeField] private IntReference _enemiesLeft;

    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnPlayButtonClick()
    {
      _tutorialPanel.ShowPanel(true);
      _playButton.gameObject.SetActive(false);
    }
    
    public void OnTowerSelected(GameObject tower)
    {
      var towerScript = tower.GetComponent<TowerScript>();
      _upgradePanel.SelectedTower = towerScript;
      _upgradePanel.SetUpgradeCostText(towerScript.UpgradeCost);
      _upgradePanel.SetSellCostText(towerScript.SellCost);
      _towerPanel.gameObject.SetActive(false);
      _upgradePanel.gameObject.SetActive(true);
      _upgradePanel.UpdatePanel();
    }

    public void OnTowerDeselected()
    {
      _towerPanel.gameObject.SetActive(true);
      _upgradePanel.gameObject.SetActive(false);
    }

    public void OnUIUpdateRequested()
    {
      _moneyPanel.SetText(_money.Value.ToString());
      _scorePanel.SetText(_score.Value.ToString());
      _enemiesLeftPanel.SetText(_enemiesLeft.Value.ToString());
    }
    
    public void OnHallUnlocked(int hallNumber)
    {
      _hallUnlockPanel.gameObject.SetActive(true);
      _hallUnlockPanel.OnHallUnlocked(hallNumber);
    }

    public void OnTrackableFound()
    {
      _playButton.gameObject.SetActive(true);
    }
    
    public void OnGameStart()
    {
      UIToPlayMode();
    }

    public void OnGameOver()
    {
     _gameOverPanel.ShowPanel(_score);
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------

    private void UIToPlayMode()
    {
      ShowStats();
      _towerPanel.gameObject.SetActive(true);
      _playButton.gameObject.SetActive(false);
    }

    private void ShowStats()
    {
      _moneyPanel.SetActive(true);
      _scorePanel.SetActive(true);
      _enemiesLeftPanel.SetActive(true);
    }
  }
}
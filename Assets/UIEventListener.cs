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
    [SerializeField]
    private TowerUpgradePanelScript _upgradePanel;

    [SerializeField] private RectTransform _towerPanel;

    [SerializeField] private IntReference _money;

    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnCreepDisappeared(GameObject creep)
    {
      var creepEnemyScript = creep.GetComponent<EnemyScript>();

      if (creepEnemyScript.Happy)
      {
        var score = int.Parse(_scoreText.text) + 1;
        var money = int.Parse(_moneyText.text) + creepEnemyScript.Money;

        _scoreText.text = score.ToString();
        _moneyText.text = money.ToString();
      }
      else
      {
        var enemiesLeft = int.Parse(_enemiesLeftText.text) - 1;
        _enemiesLeftText.text = enemiesLeft.ToString();
      }
    }

    public void OnTowerSelected(GameObject tower)
    {
      var towerScript = tower.GetComponent<TowerScript>();
      _upgradePanel.SelectedTower = towerScript;
      _upgradePanel.SetUpgradeCostText(towerScript.UpgradeCost);
      _upgradePanel.SetSellCostText(towerScript.SellCost);
      
      _towerPanel.gameObject.SetActive(false);
      _upgradePanel.gameObject.SetActive(true);
    }

    public void OnTowerDeselected()
    {
      _towerPanel.gameObject.SetActive(true);
      _upgradePanel.gameObject.SetActive(false);
    }

    public void OnMoneyChanged()
    {
      _moneyText.text = _money.Value.ToString();
    }
  }
}
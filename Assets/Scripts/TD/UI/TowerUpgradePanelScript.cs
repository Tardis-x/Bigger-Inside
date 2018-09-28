using System;
using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class TowerUpgradePanelScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		
		[Header("Events")]
		[SerializeField] private GameEvent _towerDeselected;
		[SerializeField] private IntGameEvent _moneyEvent;
		[SerializeField] private IntGameEvent _audioEvent;

		[Space]
		[Header("UI")]
		[SerializeField] private Text _upgradeCostText;
		[SerializeField] private Text _sellCostText;
		[SerializeField] private Button _upgradeButon;

		[Space]
		[Header("Variables")]
		[SerializeField]
		private IntReference _money;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void OnEnable()
		{
			UpdatePanel();
		}

		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------

		public void OnUIUpdateRequested()
		{
			UpdatePanel();
		}
		
		public void OnUpgradeButtonClick()
		{
			if (!_canUpgrade) return;

			if (SelectedTower.LevelUp())
			{
				_audioEvent.Raise((int) Sound.UpgradeTower);
				_moneyEvent.Raise(-SelectedTower.UpgradeCost);
			}
		}

		public void OnrSellButtonClick()
		{
			_moneyEvent.Raise(SelectedTower.SellCost);
			SelectedTower.Disable();
			SelectedTower.Sell();
			SelectedTower = null;
			_audioEvent.Raise((int) Sound.SellTower);
			_towerDeselected.Raise();
		}

		public void OnCancelButtonClick()
		{
			SelectedTower.Disable();
			SelectedTower = null;
			_towerDeselected.Raise();
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		[NonSerialized] public TowerScript SelectedTower;
		
		public void SetUpgradeCostText(int cost)
		{
			_upgradeCostText.text = cost.ToString();
		}

		public void SetSellCostText(int cost)
		{
			_sellCostText.text = cost.ToString();
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		public void UpdatePanel()
		{
			bool canAfford = _money.Value >= SelectedTower.UpgradeCost;
			_canUpgrade = canAfford;
			_upgradeCostText.color = canAfford ? Color.white : Color.red;
			
			_upgradeButon.gameObject.SetActive(SelectedTower.Level < SelectedTower.MaxLevel);
			_upgradeCostText.gameObject.SetActive(SelectedTower.Level < SelectedTower.MaxLevel);
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private bool _canUpgrade;
	}
}
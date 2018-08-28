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
			
			SelectedTower.LevelUp();
			_moneyEvent.Raise(-SelectedTower.UpgradeCost);
		}

		public void OnrSellButtonClick()
		{
			_moneyEvent.Raise(SelectedTower.SellCost);
			SelectedTower.Disable();
			SelectedTower.Sell();
			SelectedTower = null;
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

		private void UpdatePanel()
		{
			bool canAfford = _money.Value >= SelectedTower.UpgradeCost;
			_canUpgrade = canAfford;
			_upgradeCostText.color = canAfford ? Color.white : Color.red;
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private bool _canUpgrade;
	}
}
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

		[SerializeField] private GameEvent _towerDeselected;
		[SerializeField] private IntGameEvent _moneyEvent;
		[SerializeField] private Text _upgradeCostText;
		[SerializeField] private Text _sellCostText;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		[NonSerialized] public TowerScript SelectedTower;

		public void OnUpgradeButtonClick()
		{
			SelectedTower.LevelUp();
			_moneyEvent.Raise(-SelectedTower.UpgradeCost);
		}

		public void OnrSellButtonClick()
		{
			//TODO: Sell tower
			_moneyEvent.Raise(SelectedTower.SellCost);
		}

		public void OnCancelButtonClick()
		{
			SelectedTower.Disable();
			SelectedTower = null;
			_towerDeselected.Raise();
		}

		public void SetUpgradeCostText(int cost)
		{
			_upgradeCostText.text = cost.ToString();
		}

		public void SetSellCostText(int cost)
		{
			_sellCostText.text = cost.ToString();
		}
	}
}
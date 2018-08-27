using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class TutorialPanelScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private GameObject _towersTip;
		[SerializeField] private GameObject _enemyTip;
		[SerializeField] private GameObject _veganTip;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ShowTowerTip()
		{
			_towersTip.SetActive(true);
			_enemyTip.SetActive(false);
			_veganTip.SetActive(false);
		}

		public void ShowEnemyTip()
		{
			_towersTip.SetActive(false);
			_enemyTip.SetActive(true);
			_veganTip.SetActive(false);
		}

		public void ShowVeganTip()
		{
			_towersTip.SetActive(false);
			_enemyTip.SetActive(false);
			_veganTip.SetActive(true);
		}

		public void ShowPanel(bool value)
		{
			gameObject.SetActive(value);
		}
	}
}
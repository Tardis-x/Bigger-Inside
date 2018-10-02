using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class TutorialPanelScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[Header("UI")]
		[SerializeField] private GameObject _towersTip;
		[SerializeField] private GameObject _enemyTip;
		[SerializeField] private GameObject _veganTip1;
		[SerializeField] private GameObject _veganTip2;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ShowTowerTip()
		{
			_towersTip.SetActive(true);
			_enemyTip.SetActive(false);
			_veganTip1.SetActive(false);
			_veganTip2.SetActive(false);
		}

		public void ShowEnemyTip()
		{
			_towersTip.SetActive(false);
			_enemyTip.SetActive(true);
			_veganTip1.SetActive(false);
			_veganTip2.SetActive(false);
		}

		public void ShowVeganTip1()
		{
			_towersTip.SetActive(false);
			_enemyTip.SetActive(false);
			_veganTip1.SetActive(true);
			_veganTip2.SetActive(false);
		}
		
		public void ShowVeganTip2()
		{
			_towersTip.SetActive(false);
			_enemyTip.SetActive(false);
			_veganTip1.SetActive(false);
			_veganTip2.SetActive(true);
			PlayerPrefsHandler.SetTutorState(true);
		}

		public void ShowPanel(bool value)
		{
			gameObject.SetActive(value);
		}
	}
}
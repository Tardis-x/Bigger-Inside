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
		[SerializeField] private GameObject _veganTip;

		[Space] 
		[Header("Events")]
		[SerializeField] private GameEvent _gameStart;
		
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
			PlayerPrefsHandler.SetTutorState(true);
		}

		public void ShowPanel(bool value)
		{
			if(!value) _gameStart.Raise();
			gameObject.SetActive(value);
		}
	}
}
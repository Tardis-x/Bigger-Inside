using UnityEngine;
using UnityEngine.UI;

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
		[SerializeField] private Button _veganTipNextButton;
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

		public void ShowVeganTip1()
		{
			_towersTip.SetActive(false);
			_enemyTip.SetActive(false);
			_veganTip.SetActive(true);
		}
		
		public void ShowVeganTip2()
		{
			_towersTip.SetActive(false);
			_enemyTip.SetActive(false);
			_veganTipNextButton.onClick.RemoveAllListeners();
			_veganTipNextButton.onClick.AddListener(VeganTipButtonOnClick);
			PlayerPrefsHandler.SetTutorState(true);
		}

		public void ShowPanel(bool value)
		{
			gameObject.SetActive(value);
		}

		public void VeganTipButtonOnClick()
		{
		  _gameStart.Raise();
			ShowPanel(false);
		}
	}
}
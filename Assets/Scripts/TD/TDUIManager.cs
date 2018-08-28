using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class TDUIManager : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private HallUnlockPanelScript _hallUnlockPanel;

		public void OnHallUnlocked(int hallNumber)
		{
			if(hallNumber > 0)	_hallUnlockPanel.gameObject.SetActive(true);
			_hallUnlockPanel.OnHallUnlocked(hallNumber);
		}

	}
}
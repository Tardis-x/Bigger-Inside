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
			// Don't show first hall announcement
			if (hallNumber == 1) return;
			
			_hallUnlockPanel.gameObject.SetActive(true);
			_hallUnlockPanel.OnHallUnlocked(hallNumber);
		}

	}
}
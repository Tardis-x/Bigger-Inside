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
			_hallUnlockPanel.gameObject.SetActive(true);
			_hallUnlockPanel.OnHallUnlocked(hallNumber);
		}

	}
}
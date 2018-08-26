using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class TowerPriceScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Text _priceText;
		[SerializeField] private IntReference _towerPrice;

		private void OnEnable()
		{
			_priceText.text = _towerPrice.Value.ToString();
		}
	}
}
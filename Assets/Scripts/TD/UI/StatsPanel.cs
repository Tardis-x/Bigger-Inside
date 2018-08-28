using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class StatsPanel : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private Text _text;
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void SetText(string text)
		{
			_text.text = text;
		}

		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}
	}
}
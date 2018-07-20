namespace AndroidGoodiesExamples
{
	using UnityEngine;

	public class AndroidGoodiesDemoController : MonoBehaviour
	{
		public GameObject uiPanel;
		public GameObject appInteractionPanel;
		public GameObject sharePanel;
		public GameObject hardwarePanel;
		public GameObject otherStuffPanel;
		public GameObject textInfoPanel;

		public void OnUI()
		{
			uiPanel.SetActive(true);
		}

		public void OnOpenApps()
		{
			appInteractionPanel.SetActive(true);
		}

		public void OnShare()
		{
			sharePanel.SetActive(true);
		}

		public void OnHardware()
		{
			hardwarePanel.SetActive(true);
		}

		public void OnTextInfo()
		{
			textInfoPanel.SetActive(true);
		}

		public void OnOtherStuff()
		{
			otherStuffPanel.SetActive(true);
		}
	}
}
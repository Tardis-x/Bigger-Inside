using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class ARMapTip : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[Header("Cards")]
		[SerializeField] private GameObject _badgeCard;
		[SerializeField] private GameObject _arCoreCard;
		[SerializeField] private GameObject _noArCoreCard;

		[Space] [Header("Text")] [SerializeField]
		private Text _arCoreTipText;

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void ShowPanel()
		{
			gameObject.SetActive(true);
			_badgeCard.SetActive(true);
		}

		public void ShowArCoreCard()
		{
			if (ARCoreHelper.CheckArCoreSupport())
			{
				_arCoreCard.SetActive(true);
				
#if UNITY_IOS
				_arCoreTipText.text = "Your phone has AR Kit support. Just aim camera at any horizontal surface, "+
"wait a bit for AR Core magic and place the venue map by tapping the surface";
#endif
				
#if UNITY_ANDROID
				_arCoreTipText.text = "Your phone has AR Core support. Just aim camera at any horizontal surface, " +
				                      "wait a bit for AR Core magic and place the venue map by tapping the surface";
#endif

			}
			else
			{
				_noArCoreCard.SetActive(true);
			}
		}
	}
}
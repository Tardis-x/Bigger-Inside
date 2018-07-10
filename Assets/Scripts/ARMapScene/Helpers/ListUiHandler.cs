using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class ListUiHandler : MonoBehaviour
	{

		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[Header("Buttons")] [SerializeField] private Button _addItemButton;
		[SerializeField] private Button _clearListButton;
		[SerializeField] private Button _hideListButton;
		[SerializeField] private Button _showListButton;

		[Space] [Header("List")] [SerializeField]
		private RectTransform _scrollableList;

		[Space] [Header("Item")] [SerializeField]
		private RectTransform _item;

		[SerializeField] private string _itemTime;
		[SerializeField] private string _itemName;

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private ScrollableListScript _listScript;
		private SpeechScript _speechScript;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Awake()
		{
			_listScript = _scrollableList.GetComponent<ScrollableListScript>();
			_speechScript = _item.GetComponent<SpeechScript>();
			_addItemButton.onClick.AddListener(AddItemButtonOnClick);
			_clearListButton.onClick.AddListener(ClearListButtonOnClick);
			_hideListButton.onClick.AddListener(HideListButtonOnClick);
			_showListButton.onClick.AddListener(ShowListButtonOnClick);
		}

		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private void AddItemButtonOnClick()
		{
			//_listScript.AddContentItem(_showScript.GetInstance(_itemTime, _itemName));
		}

		private void ClearListButtonOnClick()
		{
			_listScript.ClearContent();
		}

		private void HideListButtonOnClick()
		{
			_listScript.Disable();
		}

		private void ShowListButtonOnClick()
		{
			_listScript.Enable();
		}
	}
}
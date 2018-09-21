using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class HallNameToCanvas : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private Text _hallNameInstance;
		
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private float _offset = 0.3f;		
		[SerializeField] private GameObject _hallNamePrefab;
		[SerializeField] private Canvas _canvas;
		[SerializeField] private string _hallName;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void OnDestroy()
		{
			if(_hallNameInstance != null) Destroy(_hallNameInstance.gameObject);
		}

		private void Update()
		{
			if (_hallNameInstance != null)
			{
				_hallNameInstance.transform.position = Camera.main.WorldToScreenPoint(
					Vector3.up * 0 + transform.position);
			}
		}

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void SetValue(string value)
		{
			if (_hallNameInstance == null)
			{
				InstantiateHpHallName();
			}

			_hallNameInstance.text = value;
		}

		public void OnTrackingFound()
		{
			SetValue(_hallName);
		}

		public void OnTrackingLost()
		{
			OnDestroy();
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private void InstantiateHpHallName()
		{
			if(_hallNameInstance != null) Destroy(_hallNameInstance);
			
			var hallName = Instantiate(_hallNamePrefab);
			hallName.transform.SetParent(_canvas.transform, false);
			hallName.transform.SetAsFirstSibling();
			hallName.transform.position = Camera.main.WorldToScreenPoint(Vector3.up * _offset + transform.position);
			_hallNameInstance = hallName.GetComponent<Text>();
		}
	}
}
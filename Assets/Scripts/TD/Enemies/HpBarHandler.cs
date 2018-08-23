using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class HpBarHandler : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private Slider _hpBarInstance;
		private GameObject _happyIconInstance;
		
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		[SerializeField] private float _offset = 0.3f;		
		[SerializeField] private GameObject _hpBarPrefab;
		[SerializeField] private GameObject _happyIconPrefab;
		
		//---------------------------------------------------------------------
		// Property
		//---------------------------------------------------------------------
		
		public Canvas Canvas { get; set; }
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void OnDestroy()
		{
			if(_hpBarInstance != null) Destroy(_hpBarInstance.gameObject);
			if(_happyIconInstance != null) Destroy(_happyIconInstance.gameObject);
		}

		private void Update()
		{
			if (_hpBarInstance != null)
			{
				_hpBarInstance.transform.position = Camera.main.WorldToScreenPoint(
					Vector3.up * _offset + transform.position);
			}

			if (_happyIconInstance != null)
			{
				_happyIconInstance.transform.position = Camera.main.WorldToScreenPoint(
					Vector3.up * _offset + transform.position);
			}
		}

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void SetValue(float value)
		{
			if (_hpBarInstance == null)
			{
				InstantiateHpBar();
			}

			_hpBarInstance.value = value;
		}

		public void Fed()
		{
			if(_hpBarInstance != null) _hpBarInstance.gameObject.SetActive(false);
			InstantiateHappyIcon();
		}
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private void InstantiateHpBar()
		{
			var hpBar = Instantiate(_hpBarPrefab);
			hpBar.transform.SetParent(Canvas.transform, false);
			hpBar.transform.position = Camera.main.WorldToScreenPoint(Vector3.up * _offset + transform.position);
			_hpBarInstance = hpBar.GetComponent<Slider>();
		}

		private void InstantiateHappyIcon()
		{
			_happyIconInstance = Instantiate(_happyIconPrefab);
			_happyIconInstance.transform.SetParent(Canvas.transform, false);
			_happyIconInstance.transform.position = Camera.main.WorldToScreenPoint(
				Vector3.up * _offset + transform.position);
		}
	}
}
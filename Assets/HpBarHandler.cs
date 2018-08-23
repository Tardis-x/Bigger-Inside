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
		
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		[SerializeField] private float _offset = 0.3f;		
		[SerializeField] private GameObject _hpBarPrefab;
		
		//---------------------------------------------------------------------
		// Property
		//---------------------------------------------------------------------
		
		public Canvas Canvas { get; set; }
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			Canvas = FindObjectOfType<Canvas>();
		}

		private void OnDestroy()
		{
			if(_hpBarInstance != null) Destroy(_hpBarInstance.gameObject);
		}

		private void Update()
		{
			if (_hpBarInstance == null) return;
			
			_hpBarInstance.transform.position = Camera.main.WorldToScreenPoint(
				Vector3.up * _offset + transform.position);
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
	}
}
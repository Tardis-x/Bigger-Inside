using UnityEngine;

namespace ua.org.gdg.devfest
{

	public class LoadingCircle : MonoBehaviour
	{

		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private float _rotateSpeed = 200f;

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private RectTransform _rectComponent;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		// Use this for initialization
		void Start()
		{
			_rectComponent = GetComponent<RectTransform>();
		}

		// Update is called once per frame
		void Update()
		{
			_rectComponent.Rotate(0f, 0f, -_rotateSpeed * Time.deltaTime);
		}
	}
}

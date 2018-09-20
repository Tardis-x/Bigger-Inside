using UnityEngine;
using UnityEngine.UI;

namespace ua.org.gdg.devfest
{
	public class ClampScrollRectVelocity : MonoBehaviour
	{
		private const int CLAMP = 10;
		
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private ScrollRect _scrollRect;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Start()
		{
			_scrollRect = GetComponent<ScrollRect>();
		}

		private void Update()
		{
			if (Mathf.Abs(_scrollRect.velocity.x) < CLAMP) _scrollRect.velocity = new Vector2(0, _scrollRect.velocity.y);
			if (Mathf.Abs(_scrollRect.velocity.y) < CLAMP) _scrollRect.velocity = new Vector2(_scrollRect.velocity.x, 0);
		}
	}
}
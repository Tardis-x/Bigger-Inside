using UnityEngine;

namespace Lean.Touch
{
	// This script allows you to translate the current GameObject relative to the camera
	public class LeanTranslate : MonoBehaviour
	{
		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with IsOverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
		public int RequiredFingerCount;

		[Tooltip("Does translation require an object to be selected?")]
		public LeanSelectable RequiredSelectable;

		[Tooltip("The camera the translation will be calculated using (None = MainCamera)")]
		public Camera Camera;
		
		private Vector3 _anchor;
		
		[Header("Offset")] 
		[SerializeField] private Vector3 _offset;

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Start();
		}
#endif

		protected virtual void Start()
		{
			if (RequiredSelectable == null)
			{
				RequiredSelectable = GetComponent<LeanSelectable>();
			}
			_anchor = transform.position;
		}

		protected virtual void Update()
		{
			// Get the fingers we want to use
			var fingers = LeanSelectable.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount, RequiredSelectable);

			// Calculate the screenDelta value based on these fingers
			var screenDelta = LeanGesture.GetScreenDelta(fingers);

			// Perform the translation
			if (transform is RectTransform)
			{
				TranslateUI(screenDelta);
			}
			else
			{
				Translate(screenDelta);
			}
		}

		protected virtual void TranslateUI(Vector2 screenDelta)
		{
			// Screen position of the transform
			var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera, transform.position);

			// Add the deltaPosition
			screenPoint += screenDelta;

			// Convert back to world space
			var worldPoint = default(Vector3);

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, screenPoint, Camera, out worldPoint) == true)
			{
				transform.position = worldPoint;
			}
		}

		protected virtual void Translate(Vector2 screenDelta)
		{
			// Make sure the camera exists
			var camera = LeanTouch.GetCamera(Camera, gameObject);

			if (camera != null)
			{
				// Screen position of the transform
				var screenPoint = camera.WorldToScreenPoint(transform.position);

				// Add the deltaPosition
				screenPoint += (Vector3)screenDelta;
				
 			  var scale = transform.localScale.x / .3f;

				var pos = camera.ScreenToWorldPoint(screenPoint);
				pos.x = Mathf.Clamp(pos.x, (_anchor.x - _offset.x) * scale, (_anchor.x + _offset.x) * scale);
				pos.y = _anchor.y;
				pos.z = Mathf.Clamp(pos.z, (_anchor.z - _offset.z) * scale, (_anchor.z + _offset.z) * scale);

				transform.position = pos;
			}
		}
	}
}
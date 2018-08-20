using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class DragAndDropTower : MonoBehaviour
	{
		private void Update()
		{
			if (Input.touchCount > 0)
			{
				var touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
				{
					var touchedPos =
						Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));					
					var newPos = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime * 10);
					
					transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
				}
			}
		}
	}
}
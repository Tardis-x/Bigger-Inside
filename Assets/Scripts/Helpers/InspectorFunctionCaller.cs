using UnityEngine;
using UnityEngine.Events;

namespace ua.org.gdg.devfest
{
	public class InspectorFunctionCaller : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] public UnityEvent ButtonPressedEvent;
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		public void OnButtonPressed()
		{
				ButtonPressedEvent.Invoke();
		}
	}
}
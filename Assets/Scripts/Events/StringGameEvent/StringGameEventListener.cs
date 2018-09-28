using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class StringGameEventListener : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		public StringGameEvent Event;
		public StringUnityEvent Response;

		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------
		
		private void OnEnable()
		{
			Event.RegisterListener(this);
		}

		private void OnDisable()
		{
			Event.UnRegisterListener(this);
		}
		
		//---------------------------------------------------------------------
		// Events
		//---------------------------------------------------------------------

		public void OnEventRaised(string param)
		{
			Response.Invoke(param);
		}
	}
}

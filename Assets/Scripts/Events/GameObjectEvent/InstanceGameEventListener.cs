using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class InstanceGameEventListener : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		public InstanceGameEvent Event;
		public UnityEventWithInstance Response;

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

		public void OnEventRaised(GameObject raiser)
		{
			Response.Invoke(raiser);
		}
	}
}

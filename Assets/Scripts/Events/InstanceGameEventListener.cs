using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class InstanceGameEventListener : MonoBehaviour
	{
		public InstanceGameEvent Event;
		public UnityEventWithInstance Response;

		private void OnEnable()
		{
			Event.RegisterListener(this);
		}

		private void OnDisable()
		{
			Event.UnRegisterListener(this);
		}

		public void OnEventRaised(GameObject raiser)
		{
			Response.Invoke(raiser);
		}
	}
}

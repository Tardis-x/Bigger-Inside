using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class IntGameEventListener : MonoBehaviour
	{
		public IntGameEvent Event;
		public IntUnityEvent Response;

		private void OnEnable()
		{
			Event.RegisterListener(this);
		}

		private void OnDisable()
		{
			Event.UnRegisterListener(this);
		}

		public void OnEventRaised(int param)
		{
			Response.Invoke(param);
		}
	}
}

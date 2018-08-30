using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[CreateAssetMenu(menuName = "Events/InstanceGameEvent")]
	public class InstanceGameEvent : ScriptableObject
	{
		private List<InstanceGameEventListener> _listeners = new List<InstanceGameEventListener>();

		public void Raise(GameObject raiser)
		{
			for (var i = _listeners.Count - 1; i >= 0; i--)
			{
				_listeners[i].OnEventRaised(raiser);
			}
		}

		public void RegisterListener(InstanceGameEventListener listener)
		{
			_listeners.Add(listener);
		}

		public void UnRegisterListener(InstanceGameEventListener listener)
		{
			_listeners.Remove(listener);
		}
	}
}
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[CreateAssetMenu(menuName = "Events/IntGameEvent")]
	public class IntGameEvent : ScriptableObject
	{
		private List<IntGameEventListener> _listeners = new List<IntGameEventListener>();

		public void Raise(int param)
		{
			for (var i = _listeners.Count - 1; i >= 0; i--)
			{
				_listeners[i].OnEventRaised(param);
			}
		}

		public void RegisterListener(IntGameEventListener listener)
		{
			_listeners.Add(listener);
		}

		public void UnRegisterListener(IntGameEventListener listener)
		{
			_listeners.Remove(listener);
		}
	}
}
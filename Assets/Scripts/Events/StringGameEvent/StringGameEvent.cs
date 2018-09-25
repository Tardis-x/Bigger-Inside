using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[CreateAssetMenu(menuName = "Events/StringGameEvent")]
	public class StringGameEvent : ScriptableObject
	{
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------
		
		private List<StringGameEventListener> _listeners = new List<StringGameEventListener>();

		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------
		
		public void Raise(string param)
		{
			for (var i = _listeners.Count - 1; i >= 0; i--)
			{
				_listeners[i].OnEventRaised(param);
			}
		}

		public void RegisterListener(StringGameEventListener listener)
		{
			_listeners.Add(listener);
		}

		public void UnRegisterListener(StringGameEventListener listener)
		{
			_listeners.Remove(listener);
		}
	}
}
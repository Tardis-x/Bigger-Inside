﻿using UnityEngine;
using UnityEngine.Events;

namespace ua.org.gdg.devfest
{
	public class GameEventListener : MonoBehaviour
	{
		public GameEvent Event;
		public UnityEvent Response;

		private void OnEnable()
		{
			Event.RegisterListener(this);
		}

		private void OnDisable()
		{
			Event.UnRegisterListener(this);
		}

		public void OnEventRaised()
		{
			Response.Invoke();
		}
	}
}

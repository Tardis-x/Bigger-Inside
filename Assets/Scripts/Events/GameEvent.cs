using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
    [CreateAssetMenu(menuName = "Events/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> _listeners = new List<GameEventListener>();

        public void Raise()
        {
            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnRegisterListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }
    }
}

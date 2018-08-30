using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "Variables/IntVariable")]
  public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
  {
    [SerializeField] private int _initialValue;

    [NonSerialized] public int RuntimeValue;

    public int InitialValue
    {
      get { return _initialValue; }
    }

    public void ResetValue()
    {
      RuntimeValue = _initialValue;
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
      RuntimeValue = _initialValue;
    }
  }
}
using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(fileName = "FloatVariable", menuName = "Variables/FloatVariable")]
  public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
  {
    [SerializeField] private float _initialValue;

    [NonSerialized] public float RuntimeValue;

    public float InitialValue
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
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class PersistentSingleton<T> : MonoBehaviour where T : Component
  {
    private static T _instance;

    [SuppressMessage("ReSharper", "InvertIf")]
    public static T Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = FindObjectOfType<T>();

          if (_instance == null)
          {
            var obj = new GameObject {hideFlags = HideFlags.HideAndDontSave};
            _instance = obj.AddComponent<T>();
          }
        }

        return _instance;
      }
    }

    protected virtual void Awake()
    {
      if (_instance == null)
      {
        _instance = this as T;
        DontDestroyOnLoad(gameObject);
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }
}
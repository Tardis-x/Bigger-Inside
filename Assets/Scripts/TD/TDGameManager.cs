using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class TDGameManager : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    
    [SerializeField] private GameEvent _levelUp;
    [SerializeField] private FloatReference _timing;
    [SerializeField] private IntReference _level;

    [Space]
    [Header("Events")]
    [SerializeField] private GameEvent _onEndDrag;
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void OnPause()
    {
      Time.timeScale = 0;
    }

    public void OnResume()
    {
      Time.timeScale = 1;
    }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      StartCoroutine(LevelUpCoroutine(_timing));
    }
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private IEnumerator LevelUpCoroutine(FloatReference timing)
    {
      while (true)
      {
        yield return new WaitForSeconds(timing.Value);
        _level.Value++;
        _levelUp.Raise();
      }
    }
  }
}
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
        _levelUp.Raise();
        _level.Value++;
      }
    }
  }
}
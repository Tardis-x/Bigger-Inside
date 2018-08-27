using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class TDGameManager : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    
    [SerializeField] private FloatReference _timing;
    [SerializeField] private IntReference _level;
    [SerializeField] private IntReference _money;
    [SerializeField] private ObjectClick _objectClick;

    [Space]
    [Header("Events")]
    [SerializeField] private GameEvent _onEndDrag;
    [SerializeField] private GameEvent _levelUp;
    [SerializeField] private GameEvent _moneyChanged;
    
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

    public void OnMoneyChanged(int amount)
    {
      _money.Value += amount;
      _moneyChanged.Raise();
    }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      StartCoroutine(LevelUpCoroutine(_timing));
      _objectClick.IsInteractable = true;
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
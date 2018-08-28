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
    [SerializeField] private ObjectClick _objectClick;

    [Space]
    [Header("Events")]
    [SerializeField] private GameEvent _onEndDrag;
    [SerializeField] private GameEvent _levelUp;
    [SerializeField] private GameEvent _uiUpdateRequested;

    [Space] 
    [Header("Public Variables")] 
    [SerializeField] private IntReference _money;
    [SerializeField] private IntReference _score;
    [SerializeField] private IntReference _enemiesLeft;
    
    
    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnGameStart()
    {
      StartCoroutine(LevelUpCoroutine(_timing));
    }

    public void OnGameOver()
    {
      
    }
    
    public void OnPause()
    {
      Time.timeScale = 0;
    }

    public void OnResume()
    {
      Time.timeScale = 1;
    }

    public void OnMoneyEvent(int amount)
    {
      _money.Value += amount;
      _uiUpdateRequested.Raise();
    }

    public void OnCreepDisappeared(GameObject creep)
    {
      var creepEnemyScript = creep.GetComponent<EnemyScript>();

      if (creepEnemyScript.Happy)
      {
        _score.Value += 1;
        _money.Value += creepEnemyScript.Money;
      }
      else
      {
        _enemiesLeft.Value -= 1;
      }
      
      _uiUpdateRequested.Raise();
    }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _objectClick.IsInteractable = true;
      SetMoney(_money);
    }
    
    //---------------------------------------------------------------------
    // Helpers
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

    private void SetMoney(int amount)
    {
      _money.Value = amount;
      _uiUpdateRequested.Raise();
    }
  }
}
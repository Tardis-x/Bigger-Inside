using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameEvent _gameOver;
    [SerializeField] private GameEvent _gameStart;

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
      ResumeGame();
    }

    public void OnGameOver()
    {
      PauseGame();
    }
    
    public void OnPause()
    {
      PauseGame();
    }

    public void OnResume()
    {
      ResumeGame();
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
      
      if(_enemiesLeft == 0) _gameOver.Raise();
      
      _uiUpdateRequested.Raise();
    }

    public void OnRestart()
    {
      SceneManager.LoadScene("TDScene");
    }

    public void GoToMainMenu()
    {
      SceneManager.LoadScene(Scenes.SCENE_MENU);
    }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _objectClick.IsInteractable = true;
      SetMoney(_money);
    }

    private void Awake()
    {
      ResetVariables();
      Time.timeScale = 1;
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        GoToMainMenu();
      }
    }

    private void OnDestroy()
    {
      Time.timeScale = 1;
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    private void ResetVariables()
    {
      _score.Variable.ResetValue();
      _money.Variable.ResetValue();
      _enemiesLeft.Variable.ResetValue();
      _level.Variable.ResetValue();
    }
    
    public void ClearPrefs()
    {
      PlayerPrefs.DeleteAll();
    }
    
    private void PauseGame()
    {
      Time.timeScale = 0;
    }

    private void ResumeGame()
    {
      Time.timeScale = 1;
    }
    
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
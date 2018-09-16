using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SpawnPointScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    
    [Header("Scriptable Objects")]
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private IntReference _gameLevel;
    
    [Space]
    [Header("Nodes")]
    [SerializeField] private Node _startNode;
    [SerializeField] private Node _happyExitNode;

    [Space] 
    [Header("UI")] 
    [SerializeField] private Canvas _canvas;
    
    [Space] 
    [Header("Parameters")] 
    [SerializeField] private IntReference _hallNumber;
    [SerializeField] private IntGameEvent _hallUnlockedGameEvent;

    [Space] 
    [Header("Crowd")] 
    [SerializeField] private GameObject _lockedCrowd;
    [SerializeField] private GameObject _unlockedCrowd;
    [SerializeField] private Animator _speaker;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private bool _unlocked;
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void Unlock()
    {
      if (_unlocked) return;
      
      _unlocked = _gameLevel >= _spawnPoint.Level;

      if (_unlocked)
      {
        if(_hallNumber > 1) _hallUnlockedGameEvent.Raise(_hallNumber);
        _lockedCrowd.SetActive(false);
        _unlockedCrowd.SetActive(true);
        StartSpawning();
      }
    }

    public void OnPrepareSpawnPoints(GameObject canvas)
    {
      _canvas = canvas.GetComponent<Canvas>();
    }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      _speaker.Play("Talking", -1, Random.value);
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    private void StartSpawning()
    {
      foreach (var spawn in _spawnPoint.SpawnList.EnemySpawns)
      {
        StartCoroutine(Spawn(spawn.Enemy, spawn.Frequency));
      }
    }
    
    private IEnumerator Spawn(EnemyScript enemy, float frequency)
    {
      while (true)
      {
        yield return new WaitForSeconds(frequency);
        var instance = enemy.GetInstance(_gameLevel.Value, transform, _startNode, _happyExitNode);
        instance.GetComponent<HpBarHandler>().Canvas = _canvas;
      }
    }
  }
}
using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SpawnPointScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private IntReference _gameLevel;
    [SerializeField] private Node _startNode;

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    [SerializeField] private bool _unlocked;
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void Unlock()
    {
      if (_unlocked) return;
      
      _unlocked = _gameLevel >= _spawnPoint.Level;
      
      if(_unlocked) StartSpawning();
    }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      Unlock();
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
        enemy.GetInstance(_gameLevel.Value, transform, _startNode);
      }
    }
  }
}
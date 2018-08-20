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
    [SerializeField] private bool _keepSpawning;
    [SerializeField] private IntReference _level;
    [SerializeField] private Node _startNode;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------
    
    private void Start()
    {
      foreach (var spawn in _spawnPoint.SpawnList.EnemySpawns)
      {
        StartCoroutine(Spawn(spawn.Enemy, spawn.Frequency));
      }
    }
    
    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
    
    private IEnumerator Spawn(EnemyScript enemy, float frequency)
    {
      while (true)
      {
        yield return new WaitForSeconds(frequency);
        if(_keepSpawning) enemy.GetInstance(_level.Value, transform, _startNode);
      }
    }
  }
}
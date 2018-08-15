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
    // Internal
    //---------------------------------------------------------------------
    
    private IEnumerator Spawn(EnemyScript enemy, float frequency)
    {
      while (_keepSpawning)
      {
        yield return new WaitForSeconds(frequency);
        Instantiate(enemy, transform);
      }
    }
  }
}
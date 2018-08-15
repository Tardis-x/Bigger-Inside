using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "TowerDefence/Enemies/SpawnPoint")]
  public class SpawnPoint : ScriptableObject
  {
    [SerializeField] public SpawnList SpawnList;
  }
}
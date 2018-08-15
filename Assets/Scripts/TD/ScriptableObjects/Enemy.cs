using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "TowerDefence/Enemies/Enemy")]
  public class Enemy : ScriptableObject
  {
    [SerializeField] private IntReference _hp;
    [SerializeField] private IntReference _money;
    [SerializeField] private FloatReference _speed;
    [SerializeField] private IntReference _moneyPerLevel;
    [SerializeField] private IntReference _hpPerLevel;
    [SerializeField] private Resistances _resistances;
  }
}
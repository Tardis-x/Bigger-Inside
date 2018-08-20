using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "TowerDefence/Enemies/Enemy")]
  public class Enemy : ScriptableObject
  {
    [SerializeField] public IntReference HP;
    [SerializeField] public IntReference Money;
    [SerializeField] public FloatReference MoveSpeed;
    [SerializeField] public IntReference MoneyPerLevel;
    [SerializeField] public IntReference HPPerLevel;
    [SerializeField] public EnemyType Type;
    [SerializeField] public Resistances Resistances;
  }
}
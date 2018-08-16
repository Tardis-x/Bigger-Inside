using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "TowerDefence/Missiles/Missile")]
  public class Missile : ScriptableObject
  {
    [SerializeField] public IntReference Damage;
    [SerializeField] public MissileType Type;
  }
}
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "TowerDefence/Missiles/Missile")]
  public class Missile : ScriptableObject
  {
    [SerializeField] public FloatReference Damage;
    [SerializeField] public MissileType Type;
  }
}
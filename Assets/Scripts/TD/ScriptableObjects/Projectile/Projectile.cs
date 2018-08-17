using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "TowerDefence/Projectiles/Projectile")]
  public class Projectile : ScriptableObject
  {
    [SerializeField] public FloatReference Damage;
    [SerializeField] public FloatReference Speed;
    [SerializeField] public ProjectileType Type;
  }
}
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CreateAssetMenu(menuName = "TowerDefence/Missile")]
  public class Missile : ScriptableObject
  {
    [SerializeField] private IntReference _damage;

    [SerializeField] private MissileType _type;
  }
}
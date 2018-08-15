using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [Serializable]
  //[CreateAssetMenu(menuName = "TowerDefence/Enemies/Resistance")]
  public class Resistance// : ScriptableObject
  {
    [SerializeField] public MissileType Type;
    [SerializeField] public FloatReference Amount;
  }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [Serializable]
  //[CreateAssetMenu(menuName = "TowerDefence/Enemies/Resistance")]
  public class Resistances// : ScriptableObject
  {
    [Serializable]
    public class Resistance
    {
      [SerializeField] public MissileType Type;
      [SerializeField] public FloatReference Amount;
    }

    [SerializeField] public List<Resistance> ResistancesList;
  }
}
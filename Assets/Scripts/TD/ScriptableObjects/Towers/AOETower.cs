using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[Serializable]
	[CreateAssetMenu(menuName = "TowerDefence/AOE Tower")]
	public class AOETower : Tower
	{
		[SerializeField] public FloatReference SlowAmount;
		[SerializeField] public FloatReference SlowAmouontPerLevel;
	}
}
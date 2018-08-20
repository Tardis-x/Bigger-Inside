using System;
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[Serializable]
	[CreateAssetMenu(menuName = "TowerDefence/Tower")]
	public class Tower : ScriptableObject
	{
		[SerializeField] public FloatReference Cooldown;
		[SerializeField] public FloatReference CDRPerLevel;
		[SerializeField] public FloatReference Range;
		[SerializeField] public FloatReference RangePerLevel;
		[SerializeField] public IntReference Level;
		[SerializeField] public IntReference MaxLevel;
		[SerializeField] public IntReference Cost;
		[SerializeField] public IntReference UpgradeCost;
		[SerializeField] public List<Enemy> IgnoredEnemies;
	}
}
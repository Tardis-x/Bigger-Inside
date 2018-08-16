using System;
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
		[SerializeField] public ProjectileScript Projectile;
		[SerializeField] public IntReference Level;
		[SerializeField] public IntReference Cost;
		[SerializeField] public IntReference UpgradeCost;
	}
}
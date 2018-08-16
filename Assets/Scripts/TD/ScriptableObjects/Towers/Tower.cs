using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[Serializable]
	[CreateAssetMenu(menuName = "TowerDefence/Tower")]
	public class Tower : ScriptableObject
	{
		[SerializeField] public FloatReference Cooldown;
		[SerializeField] public FloatReference Range;
		[SerializeField] public Missile Missile;
		[SerializeField] public IntReference Cost;
		[SerializeField] public IntReference UpgradeCost;
	}
}
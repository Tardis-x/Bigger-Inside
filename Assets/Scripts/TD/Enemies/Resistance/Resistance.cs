using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[Serializable]
	public class Resistance
	{
		[SerializeField] public ProjectileType Type;
		[SerializeField] public FloatReference Amount;
	}
}
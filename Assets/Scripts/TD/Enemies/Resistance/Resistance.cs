using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[Serializable]
	public class Resistance
	{
		[SerializeField] public MissileType Type;
		[SerializeField] public FloatReference Amount;
	}
}
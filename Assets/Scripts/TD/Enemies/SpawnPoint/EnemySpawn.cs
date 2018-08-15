using System;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[Serializable]
	public class EnemySpawn
	{
		[SerializeField] public EnemyScript Enemy;
		[SerializeField] public FloatReference Frequency;
	}
}
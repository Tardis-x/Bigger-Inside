using System;
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	[Serializable]
	public class SpawnList
	{
		[SerializeField] public List<EnemySpawn> EnemySpawns;
	}
}

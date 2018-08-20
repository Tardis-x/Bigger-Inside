using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class AOETowerScript : TowerScript
	{
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------
		
		private new void Awake()
		{
			base.Awake();
			StartCoroutine(Shoot());
		}
		
		private void OnTriggerEnter(Collider other)
		{
			var enemy = other.GetComponent<EnemyScript>();

			if (enemy != null && !Tower.IgnoredEnemyTypes.Contains(enemy.Type))
			{
				TargetsInRange.Add(enemy);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			var enemy = other.GetComponent<EnemyScript>();
			
			if (enemy != null)
			{
				RemoveTarget(enemy);
			}
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------
		
		private IEnumerator Shoot()
		{
			while (true)
			{
				// If there are any targets in range
				if(TargetsInRange.Count > 0)
				{
					// Shoot each of them
					foreach (var target in TargetsInRange)
					{
						Projectile.Shoot(target, Gun);
					}
					
					// And cooldown
					yield return new WaitForSeconds(Cooldown);
				}
				yield return new WaitForSeconds(.1f);
			}
		}
	}
}
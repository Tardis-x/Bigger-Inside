using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class AOETowerScript : TowerScript
	{
		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private float _slowAmount;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------
		
		private new void Awake()
		{
			base.Awake();
			_slowAmount = (Tower as AOETower).SlowAmount;
			StartCoroutine(Shoot());
		}
		
		private void OnTriggerEnter(Collider other)
		{
			var enemy = other.GetComponent<EnemyScript>();

			if (enemy != null && !Tower.IgnoredEnemies.Contains(enemy.Type))
			{
				TargetsInRange.Add(enemy);
				
				enemy.SetSpeed(ReduceSpeed(enemy.Speed, _slowAmount));
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
		// Public
		//---------------------------------------------------------------------

		public new void LevelUp()
		{
			base.LevelUp();
			_slowAmount += (Tower as AOETower).SlowAmouontPerLevel;
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private float ReduceSpeed(float speed, float amount)
		{
			return speed * (1 - amount * .01f);
		}

		private IEnumerator Shoot()
		{
			while (true)
			{
				if(TargetsInRange.Count > 0)
				{
					foreach (var target in TargetsInRange)
					{
						Projectile.Shoot(target, Gun);
					}
					
					yield return new WaitForSeconds(Cooldown);
				}
				yield return new WaitForSeconds(.1f);
			}
		}
	}
}
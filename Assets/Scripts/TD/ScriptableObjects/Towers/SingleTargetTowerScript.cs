using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class SingleTargetTowerScript : TowerScript
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

			if (enemy != null)
			{
				TargetsInRange.Add(enemy);

				if (_target == null) _target = enemy;
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
		// Internal
		//---------------------------------------------------------------------
		
		private EnemyScript _target;
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------
		
		private void AquireNewTarget()
		{
			_target = TargetsInRange.Count > 0 ? TargetsInRange[0] : null;
		}

		private new void RemoveTarget(EnemyScript target)
		{
			if (TargetsInRange.Contains(target))
			{
				TargetsInRange.Remove(target);
				if(target == _target) AquireNewTarget();
			}
		}

		private IEnumerator Shoot()
		{
			while(true)
			{
				if (_target != null)
				{
					Projectile.Shoot(_target, Gun);
					yield return new WaitForSeconds(Cooldown);
				}
				yield return new WaitForSeconds(.1f);
			}
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------
		
		public new void OnEnemyDie(GameObject enemy)
		{
			var enemyScript = enemy.GetComponent<EnemyScript>();
			RemoveTarget(enemyScript);
		}
	}
}
using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class AOETowerScript : TowerScript
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------

		[SerializeField] private GameObject _AOEMesh;
				
		[Space]
		[Header("Event")]
		[SerializeField] private IntGameEvent _audioEvent;
		[SerializeField] private Sound _soundType;
		
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
			_slowAmount = ((AOETower) Tower).SlowAmount;
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
				enemy.SetSpeed(enemy.Speed);
				RemoveTarget(enemy);
			}
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public new void LevelUp()
		{
			base.LevelUp();
			_slowAmount += ((AOETower) Tower).SlowAmouontPerLevel;
		}

		public void SetAOEVisible(bool value)
		{
			_AOEMesh.SetActive(value);
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
					for (int i = TargetsInRange.Count - 1; i >= 0; i--)
					{
						TargetsInRange[i].GetShot(Projectile);
					}
					_audioEvent.Raise((int) _soundType);
					
					yield return new WaitForSeconds(Cooldown);
				}
				yield return new WaitForSeconds(.1f);
			}
		}
	}
}
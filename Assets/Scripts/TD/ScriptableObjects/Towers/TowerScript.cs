using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class TowerScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private Tower _tower;
		[SerializeField] private Transform _gun;
		[SerializeField] private List<GameObject> _levels;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Awake()
		{
			var rangeCollider = GetComponent<SphereCollider>();
			rangeCollider.radius = _tower.Range;
			StartCoroutine(Shoot());
			_level = _tower.Level;
			_cooldown = _tower.Cooldown.Value;
			SetLevelMesh(_level);
		}
		
		private void OnTriggerEnter(Collider other)
		{
			// Get enemy script
			var enemy = other.GetComponent<EnemyScript>();
			
			// If it's enemy
			if (enemy != null)
			{
				// It's got in range
				_targetsInRange.Add(enemy);
				
				// If no targets aquired aquire enemy
				if (_target == null) _target = enemy;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			// Get enemy script
			var enemy = other.GetComponent<EnemyScript>();
			
			// If it's enemy
			if (enemy != null)
			{
				// It's got out of range
				RemoveTarget(enemy);
			}
		}

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		[SerializeField] private EnemyScript _target;
		[SerializeField] private List<EnemyScript> _targetsInRange = new List<EnemyScript>();
		private int _level = 0;
		private GameObject _currentMesh;
		private float _cooldown;

		private void AquireNewTarget()
		{
			// If there are enemies in range shoot first to enter
			// If not - no target can be aquired
			_target = _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
		}

		private void RemoveTarget(EnemyScript target)
		{
			// If target is in range
			if (_targetsInRange.Contains(target))
			{
				// Remove it
				_targetsInRange.Remove(target);
				// If it's also current target need to aquire a new one
				if(target == _target) AquireNewTarget();
			}
		}

		private IEnumerator Shoot()
		{
			while (true)
			{
				// If there's valid target
				if (_target != null)
				{
						// Shoot it
						_tower.Projectile.Shoot(_target, _gun);
						// And cooldown
						yield return new WaitForSeconds(_cooldown);
				}
				yield return new WaitForSeconds(.1f);
			}
		}

		private void SetLevelMesh(int level)
		{
			if (level > _tower.MaxLevel.Value) return;
			
			if(_currentMesh != null) _currentMesh.SetActive(false);
			_currentMesh = _levels[level];
			_currentMesh.SetActive(true);
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void LevelUp()
		{
			_level++;
			SetLevelMesh(_level);
			_cooldown -= _tower.CDRPerLevel.Value;
			_tower.Projectile.LevelUp();
		}

		public void OnEnemyDie(GameObject enemy)
		{
			var enemyScript = enemy.GetComponent<EnemyScript>();
			RemoveTarget(enemyScript);
		}
	}
}
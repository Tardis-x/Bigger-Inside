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
			_currentMesh = _levels[_level];
			_cooldown = _tower.Cooldown;
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
				_targetsInRange.Remove(enemy);
				
				// If current target left the range need to aquire anuther one
				if(enemy == _target) AquireNewTarget();
			}
		}

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private EnemyScript _target;
		private List<EnemyScript> _targetsInRange = new List<EnemyScript>();
		private int _level = 0;
		private GameObject _currentMesh;
		private float _cooldown;

		private void AquireNewTarget()
		{
			// If there are enemies in range shoot first to enter
			// If not - no target can be aquired
			_target = _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
		}

		private IEnumerator Shoot()
		{
			while (true)
			{
				// If there's valid target
				if (_target != null)
				{
					// And it's not dead
					if (!_target.IsDead)
					{
						// Shoot it
						_tower.Projectile.Shoot(_target, _gun);
						// And cooldown
						yield return new WaitForSeconds(_cooldown);
					}
					// If it's dead
					else
					{
						// It's not target anymore, therefore it's not in range
						_targetsInRange.Remove(_target);
						// So, need to aquire new target
						AquireNewTarget();
					}
				}
				yield return new WaitForSeconds(.1f);
			}
		}

		private void SetLevelMesh(int level)
		{
			_currentMesh.SetActive(false);
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
	}
}
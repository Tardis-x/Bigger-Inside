using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class TowerScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private Tower _tower;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void Awake()
		{
			var rangeCollider = GetComponent<SphereCollider>();
			rangeCollider.radius = _tower.Range;
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

		[SerializeField] private EnemyScript _target;
		[SerializeField] private List<EnemyScript> _targetsInRange = new List<EnemyScript>();

		private void AquireNewTarget()
		{
			// If there are enemies in range shoot first to enter
			// If not - no target can be aquired
			_target = _targetsInRange.Count > 0 ? _targetsInRange[0] : null;
		}
	}
}
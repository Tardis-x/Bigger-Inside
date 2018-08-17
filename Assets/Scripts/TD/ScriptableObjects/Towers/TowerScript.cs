using System;
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
		
		[SerializeField] protected Tower Tower;
		[SerializeField] protected Transform Gun;
		[SerializeField] protected List<GameObject> _levels;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		protected void Awake()
		{
			var rangeCollider = GetComponent<SphereCollider>();
			rangeCollider.radius = Tower.Range;
			_level = Tower.Level;
			Cooldown = Tower.Cooldown.Value;
			SetLevelMesh(_level);
		}

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		[SerializeField] protected List<EnemyScript> TargetsInRange = new List<EnemyScript>();
		private int _level;
		private GameObject _currentMesh;
		protected float Cooldown;

		protected void RemoveTarget(EnemyScript target)
		{
			if (TargetsInRange.Contains(target)) TargetsInRange.Remove(target);
		}

		private void SetLevelMesh(int level)
		{
			if (level > Tower.MaxLevel.Value) return;
			
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
			Cooldown -= Tower.CDRPerLevel.Value;
			Tower.Projectile.LevelUp();
		}

		public void OnEnemyDie(GameObject enemy)
		{
			var enemyScript = enemy.GetComponent<EnemyScript>();
			RemoveTarget(enemyScript);
		}
	}
}
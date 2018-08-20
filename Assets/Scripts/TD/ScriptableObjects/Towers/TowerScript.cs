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
		[SerializeField] protected List<GameObject> LevelMeshes;
		[SerializeField] protected ProjectileScript Projectile;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		protected void Awake()
		{
			SetRange(Tower.Range);
			_level = Tower.Level;
			Cooldown = Tower.Cooldown;
			SetLevelMesh(_level);
		}

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		protected List<EnemyScript> TargetsInRange = new List<EnemyScript>();
		private int _level;
		private GameObject _currentMesh;
		protected float Cooldown;
		protected float Range;
		

		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------
		
		protected void RemoveTarget(EnemyScript target)
		{
			if (TargetsInRange.Contains(target)) TargetsInRange.Remove(target);
		}

		private void SetRange(float range)
		{
			var rangeCollider = GetComponent<SphereCollider>();
			rangeCollider.radius = Range = range;
		}

		private void SetLevelMesh(int level)
		{
			if (level > Tower.MaxLevel) return;
			
			if(_currentMesh != null) _currentMesh.SetActive(false);
			_currentMesh = LevelMeshes[level];
			_currentMesh.SetActive(true);
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public void LevelUp()
		{
			if (_level >= Tower.MaxLevel) return;
			_level++;
			SetLevelMesh(_level);
			Cooldown -= Tower.CDRPerLevel;
			SetRange(Range + Tower.RangePerLevel);
			Projectile.LevelUp();
		}

		public void OnEnemyDie(GameObject enemy)
		{
			var enemyScript = enemy.GetComponent<EnemyScript>();
			RemoveTarget(enemyScript);
		}
	}
}
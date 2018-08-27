using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class TowerScript : InteractableObject
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] protected Tower Tower;
		[SerializeField] protected Transform Gun;
		[SerializeField] protected List<GameObject> LevelMeshes;
		[SerializeField] protected ProjectileScript Projectile;
		[SerializeField] protected ParticleSystem TowerSelectParticles;
		[SerializeField] protected InstanceGameEvent TowerSelectedEvent;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		protected void Awake()
		{
			SetRange(Tower.Range);
			Level = Tower.Level;
			Cooldown = Tower.Cooldown;
			SetLevelMesh(Level);
		}

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		protected List<EnemyScript> TargetsInRange = new List<EnemyScript>();
		protected int Level;
		private GameObject _currentMesh;
		protected float Cooldown;
		protected float Range;
		private bool _selected;
		

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
			if (Level >= Tower.MaxLevel) return;
			Level++;
			SetLevelMesh(Level);
			Cooldown -= Tower.CDRPerLevel;
			SetRange(Range + Tower.RangePerLevel);
			Projectile.LevelUp();
		}

		public void OnEnemyDie(GameObject enemy)
		{
			var enemyScript = enemy.GetComponent<EnemyScript>();
			RemoveTarget(enemyScript);
		}

		public override void Interact()
		{
			if(_selected) return;
			
			_selected = true;
			TowerSelectParticles.Play();
			TowerSelectedEvent.Raise(gameObject);
		}

		public override void Disable()
		{
			_selected = false;
			TowerSelectParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		
		//---------------------------------------------------------------------
		// Properties
		//---------------------------------------------------------------------

		public int Cost
		{
			get { return Tower.Cost; }
		}
		
		public int UpgradeCost
		{
			get { return Tower.UpgradeCost; }
		}

		public int SellCost
		{
			get { return Tower.Cost / 2; }
		}
	}
}
﻿using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class ProjectileScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private Projectile _projectile;
		[SerializeField] private ParticleSystem _particleEffects;
		
		[Space]
		[Header("Event")]
		[SerializeField] protected IntGameEvent AudioEvent;
		[SerializeField] protected Sound SoundType;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void FixedUpdate()
		{
			if(Target != null) MoveToTarget();
			else SelfDestroy();
		}

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		protected EnemyScript Target;
		protected int Level;
		protected Projectile Projectile
		{
			get { return _projectile; }
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		protected virtual bool TryToShoot(Collider target)
		{
			if (Target == null) return false;
			
			var enemy = target.GetComponent<EnemyScript>();

			if (enemy == null) return false;
			
			if (enemy == Target)
			{
				Target.GetShot(this);
				AudioEvent.Raise((int) Sound.Eat);
				SelfDestroy();
				return true;
			}

			return false;
		}
		
		protected void MoveToTarget()
		{
			transform.position =
				Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * Projectile.Speed);
		}

		protected void SelfDestroy()
		{
			if (_particleEffects != null)
			{
				_particleEffects.Play();
				GetComponent<MeshRenderer>().enabled = false;
				Destroy(gameObject, 1.1f);
			}
			else
			{
				Destroy(gameObject);
			}
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public ProjectileScript GetInstance(Transform position, int level)
		{
			var instance = Instantiate(this, position.position, position.rotation);
			instance.Level = level;
			return instance;
		}
		
		public ProjectileScript GetInstance(int level)
		{
			var instance = Instantiate(this);
			instance.Level = level;
			return instance;
		}

		public void Shoot(EnemyScript target, Transform position)
		{
			var projectile = GetInstance(position, Level);
			projectile.Target = target;
		}

		public void LevelUp()
		{
			Level++;
		}
		
		//---------------------------------------------------------------------
		// Properties
		//---------------------------------------------------------------------

		public float Damage
		{
			get { return Projectile.Damage + Projectile.DamagePerLevel * Level; }
		}

		public ProjectileType Type
		{
			get { return Projectile.Type; }
		}
	}
}
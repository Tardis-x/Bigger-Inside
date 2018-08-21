using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class ProjectileScript : MonoBehaviour
	{
		//---------------------------------------------------------------------
		// Editor
		//---------------------------------------------------------------------
		
		[SerializeField] private Projectile _projectile;
		
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void OnEnable()
		{
			_damage = Projectile.Damage;
		}

		private void FixedUpdate()
		{
			if(_target != null) MoveToTarget();
			else SelfDestroy();
		}

		private void OnTriggerEnter(Collider other)
		{
			if(_target == null) return;
			
			// Get Enemy script 
			var enemy = other.GetComponent<EnemyScript>();
			
			if(enemy == null) return;
			
			if (enemy == _target)
			{
				_target.GetShot(this);
				SelfDestroy();
			}
		}

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		private EnemyScript _target;
		private float _damage;
		private int _level;
		private Projectile Projectile
		{
			get { return _projectile; }
		}
		
		//---------------------------------------------------------------------
		// Helpers
		//---------------------------------------------------------------------

		private void MoveToTarget()
		{
			transform.position =
				Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * Projectile.Speed);
		}

		private void SelfDestroy()
		{
			Destroy(gameObject);
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public ProjectileScript GetInstance(Transform position, int level)
		{
			var instance = Instantiate(this, position.position, position.rotation);
			instance._level = level;
			return instance;
		}

		public void Shoot(EnemyScript target, Transform position)
		{
			var projectile = GetInstance(position, _level);
			projectile._target = target;
		}

		public void LevelUp()
		{
			_level++;
		}
		
		//---------------------------------------------------------------------
		// Properties
		//---------------------------------------------------------------------

		public float Damage
		{
			get { return _damage + Projectile.DamagePerLevel * _level; }
		}

		public ProjectileType Type
		{
			get { return Projectile.Type; }
		}
	}
}
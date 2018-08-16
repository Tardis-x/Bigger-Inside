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
				_target.GetShot(_projectile);
				SelfDestroy();
			}
		}

		//---------------------------------------------------------------------
		// Internal
		//---------------------------------------------------------------------

		[SerializeField] private EnemyScript _target;

		private void MoveToTarget()
		{
			transform.position =
				Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _projectile.Speed);
		}

		private void SelfDestroy()
		{
			Destroy(gameObject);
		}
		
		//---------------------------------------------------------------------
		// Public
		//---------------------------------------------------------------------

		public ProjectileScript GetInstance(Transform position)
		{
			return Instantiate(this, position.position, position.rotation);
		}

		public void Shoot(EnemyScript target, Transform position)
		{
			var projectile = GetInstance(position);
			projectile._target = target;
		}
		
		//---------------------------------------------------------------------
		// Properties
		//---------------------------------------------------------------------

		public float Damage
		{
			get { return _projectile.Damage.Value; }
		}

		public ProjectileType Type
		{
			get { return _projectile.Type; }
		}
	}
}
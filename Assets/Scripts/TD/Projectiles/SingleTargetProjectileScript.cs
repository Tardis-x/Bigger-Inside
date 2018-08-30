using UnityEngine;

namespace ua.org.gdg.devfest
{
	public class SingleTargetProjectileScript : ProjectileScript
	{
		//---------------------------------------------------------------------
		// Messages
		//---------------------------------------------------------------------

		private void OnTriggerEnter(Collider other)
		{
			TryToShoot(other);
		}
	}
}
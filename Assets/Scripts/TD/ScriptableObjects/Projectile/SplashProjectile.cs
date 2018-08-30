using UnityEngine;

namespace ua.org.gdg.devfest
{
	[CreateAssetMenu(menuName = "TowerDefence/Projectiles/SplashProjectile")]
	public class SplashProjectile : Projectile
	{
		[SerializeField] public FloatReference SplashRadius;
		[SerializeField] public FloatReference SplashRadiusPerLevel;
	}
}
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SplashProjectileScript : ProjectileScript
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private LayerMask enemiesLayer;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const int ENEMIES_LAYER_INDEX = 12;
    private float _splashRadius;

    private float SplashRadius
    {
      get { return _splashRadius + ((SplashProjectile) Projectile).SplashRadiusPerLevel * Level; }
      set { _splashRadius = value; }
    }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private new void OnEnable()
    {
      base.OnEnable();
      SplashRadius = ((SplashProjectile) Projectile).SplashRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!TryToShoot(other)) return;

      var hitEnemies = GetHitEnemies(SplashRadius);

      foreach (var enemyCollider in hitEnemies)
      {
         if(TryToShoot(enemyCollider)) Debug.Log(enemyCollider.gameObject.name + " got splash damage!");
      }

      SelfDestroy();
    }

    //---------------------------------------------------------------------
    // Helpers
    //---------------------------------------------------------------------
 
    private Collider[] GetHitEnemies(float radius)
    {
      return Physics.OverlapSphere(transform.localPosition, radius, enemiesLayer);
    }
  }
}
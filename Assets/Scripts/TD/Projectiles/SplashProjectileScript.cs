using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SplashProjectileScript : ProjectileScript
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private LayerMask _enemiesLayer;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private float SplashRadius
    {
      get { return ((SplashProjectile) Projectile).SplashRadius
                   + ((SplashProjectile) Projectile).SplashRadiusPerLevel * Level; }
    }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

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
      return Physics.OverlapSphere(transform.localPosition, radius, _enemiesLayer);
    }
  }
}
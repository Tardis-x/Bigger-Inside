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
      if (!base.TryToShoot(other)) return;

      var hitEnemies = GetHitEnemies(SplashRadius);

      foreach (var enemyCollider in hitEnemies)
      {
        if(enemyCollider == other) continue;
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
    
    protected override bool TryToShoot(Collider target)
    {
      var enemy = target.GetComponent<EnemyScript>();

      if (enemy == null) return false;
			
      enemy.GetShot(this);
      AudioEvent.Raise((int) SoundType);
      
      SelfDestroy();
      return true;
    }
  }
}
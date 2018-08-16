using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class EnemyScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Enemy _enemy;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private void Die()
    {
      Destroy(gameObject);
      IsDead = true;
    }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      HP = _enemy.HP.Value;
      Money = _enemy.Money.Value;
      IsDead = false;
    }
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public void LevelUp()
    {
      HP += _enemy.HPPerLevel;
      Money += _enemy.MoneyPerLevel;
    }

    public EnemyScript GetInstance(int level, Transform parent)
    {
      var instance = Instantiate(this, parent);
      instance.HP += _enemy.HPPerLevel.Value * level;
      instance.Money += _enemy.MoneyPerLevel.Value * level;
      return instance;
    }

    public void GetShot(Projectile projectile)
    {
      HP -= (int) Mathf.Round(projectile.Damage);
      if (HP <= 0) Die();
    }
    
    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public int HP; //{ get; private set; }

    public int Money;// { get; private set; }
    
    public bool IsDead { get; private set; }
  }
}
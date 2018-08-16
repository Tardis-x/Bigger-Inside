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
    }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      HP = _enemy.HP.Value;
      Money = _enemy.Money.Value;
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

    public void GetShot(Missile missile)
    {
      HP -= (int) Mathf.Round(missile.Damage);
      if (HP <= 0) Die();
    }
    
    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public int HP; //{ get; private set; }

    public int Money;// { get; private set; }
  }
}
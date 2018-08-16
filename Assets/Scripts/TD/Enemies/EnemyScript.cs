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

    private int _hp;
    private int _money;
    
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
    
    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public int HP
    {
      get { return _hp; }
      private set { _hp = value; }
    }
    
    public int Money
    {
      get { return _money; }
      private set { _money = value; }
    }
  }
}
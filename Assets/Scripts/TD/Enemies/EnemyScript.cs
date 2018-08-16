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
      _hp = _enemy.HP.Value;
      _money = _enemy.Money.Value;
    }
    
    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
    
    public void LevelUp()
    {
      _hp += _enemy.HPPerLevel;
      _money += _enemy.MoneyPerLevel;
    }

    public EnemyScript GetInstance(int level, Transform parent)
    {
      var instance = Instantiate(this, parent);
      instance._hp += _enemy.HPPerLevel.Value * level;
      instance._money += _enemy.MoneyPerLevel.Value * level;
      return instance;
    }
  }
}
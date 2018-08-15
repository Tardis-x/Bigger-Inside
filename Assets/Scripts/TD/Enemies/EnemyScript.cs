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
    // Public
    //---------------------------------------------------------------------

    public void SetLevel(int level)
    {
      _enemy.HP.Value += _enemy.HPPerLevel * level;
      _enemy.Money.Value += _enemy.MoneyPerLevel * level;
    }
  }
}
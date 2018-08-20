using UnityEngine;
using UnityEngine.AI;

namespace ua.org.gdg.devfest
{
  public class EnemyScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Enemy _enemy;
    [SerializeField] private InstanceGameEvent _dieEvent;
    [SerializeField] private Agent _agent;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const int DEAD_ENEMIES_LAYER = 11;

    //---------------------------------------------------------------------
    // Heplers
    //---------------------------------------------------------------------

    private void Die()
    {
      _dieEvent.Raise(gameObject);
      gameObject.layer = DEAD_ENEMIES_LAYER;
      _agent.Die();
    }

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      HP = _enemy.HP.Value;
      Money = _enemy.Money.Value;
      _agent.SetSpeed(_enemy.MoveSpeed.Value);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void LevelUp()
    {
      HP += _enemy.HPPerLevel;
      Money += _enemy.MoneyPerLevel;
    }

    public EnemyScript GetInstance(int level, Transform position, Node startDestinationNode)
    {
      var instance = Instantiate(this, position.position, position.rotation, position.parent);
      instance.HP += _enemy.HPPerLevel.Value * level;
      instance.Money += _enemy.MoneyPerLevel.Value * level;
      instance._agent.Initialize(startDestinationNode);
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

    public int HP { get; private set; }

    public int Money { get; private set; }
  }
}
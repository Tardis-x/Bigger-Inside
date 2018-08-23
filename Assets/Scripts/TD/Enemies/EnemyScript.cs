using System.Collections;
using System.Linq;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class EnemyScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [Header("Components")]
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Agent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private HpBarHandler _hpBarHandler;
    
    [Space]
    [Header("Materials")]
    [SerializeField] private Material _fadeMaterial;
    
    [Space]
    [Header("Events")]
    [SerializeField] private InstanceGameEvent _dieEvent;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const int DEAD_ENEMIES_LAYER = 11;
    private float _maxHP;

    //---------------------------------------------------------------------
    // Heplers
    //---------------------------------------------------------------------

    private void Fed()
    {
      _dieEvent.Raise(gameObject);
      gameObject.layer = DEAD_ENEMIES_LAYER;
      _agent.Fed();
      _hpBarHandler.Fed();
    }

    private float CalculateDamageCoefficient(float resist)
    {
      return 1 - resist * .01f;
    }
    
    private void ChangeMaterialToFade()
    {
      _renderer.material = _fadeMaterial;
    }

    private void UpdateHPBar()
    {
      var normalizedHP = 1 - HP / _maxHP;
      _hpBarHandler.SetValue(normalizedHP);
    }
		
    private IEnumerator LerpMeshRendererColor(Renderer targetMeshRender, float lerpDuration, 
      Color startLerp, Color targetLerp)
    {
      var lerpStartTime = Time.time;
      var lerping = true;
      while (lerping)
      {
        yield return new WaitForEndOfFrame();
        var lerpProgress = Time.time - lerpStartTime;
        if (targetMeshRender != null)
        {
          targetMeshRender.material.color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
        }
        else
        {
          lerping = false;
        }
				
        if (lerpProgress >= lerpDuration)
        {
          lerping = false;
        }
      }
      DestroyGameObject();
    }

    private void DestroyGameObject()
    {
      Destroy(gameObject);
    }
  

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      HP = _enemy.HP.Value;
      _maxHP = _enemy.HP.Value;
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
      _maxHP += _enemy.HPPerLevel;
    }

    public EnemyScript GetInstance(int level, Transform position, Node startDestinationNode, Node happyExitNode)
    {
      var instance = Instantiate(this, position.position, position.rotation, position.parent);
      instance.HP += _enemy.HPPerLevel.Value * level;
      instance.Money += _enemy.MoneyPerLevel.Value * level;
      instance._agent.Initialize(startDestinationNode);
      instance._agent.HappyExitNode = happyExitNode;
      return instance;
    }

    public void GetShot(ProjectileScript projectile)
    {
      float dmg = projectile.Damage;
      var resist = _enemy.Resistances.ResistancesList.FirstOrDefault(x => x.Type == projectile.Type);
      
      if (resist != null) dmg *= CalculateDamageCoefficient(resist.Amount);
      
      HP -= (int) Mathf.Round(dmg);
      UpdateHPBar();
      
      Debug.Log(gameObject.name + " taken " + dmg + " damage from " + projectile.Type + " projectile. HP left: " + HP);
      
      if (HP <= 0) Fed();
    }

    public void SetSpeed(float speed)
    {
      _agent.SetSpeed(speed);
    }
    
    public void Disappear()
    {
      gameObject.layer = DEAD_ENEMIES_LAYER;
      
      _animator.SetTrigger("Idle");

      if (_renderer == null)
      {
        DestroyGameObject();
        return;
      }
			
      ChangeMaterialToFade();
			
      var meshColor = _renderer.material.color;
      var invisibleColor = meshColor;
      invisibleColor.a = 0;

      StartCoroutine(LerpMeshRendererColor(_renderer, 1.5f, meshColor, invisibleColor));
    }

    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public int HP { get; private set; }

    public int Money { get; private set; }
    
    public float Speed
    {
      get { return _enemy.MoveSpeed; }
    }

    public Enemy Type
    {
      get { return _enemy; }
    }
  }
}
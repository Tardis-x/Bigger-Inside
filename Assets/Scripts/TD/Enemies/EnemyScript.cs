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
    [SerializeField] private ParticleSystem _slowEffect;
    [SerializeField] private ParticleSystem _coinsParticleSystem;
    
    [Space]
    [Header("Events")]
    [SerializeField] private InstanceGameEvent _dieEvent;
    [SerializeField] private InstanceGameEvent _onCreepDisappeared;
    
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const int DEAD_ENEMIES_LAYER = 11;
    private float _maxHP;

    //---------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------

    public int HP { get; private set; }

    public int Money { get; private set; }

    public bool Happy { get; private set; }
    
    public float Speed
    {
      get { return _enemy.MoveSpeed; }
    }

    public Enemy Type
    {
      get { return _enemy; }
    }
    
    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      HP = _enemy.HP.Value;
      _maxHP = _enemy.HP.Value;
      Money = _enemy.Money.Value;
      Happy = false;
      _agent.SetSpeed(_enemy.MoveSpeed.Value);
    }

    private void OnDestroy()
    {
      _onCreepDisappeared.Raise(gameObject);
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
      var dmg = projectile.Damage;
      var resist = _enemy.Resistances.ResistancesList.FirstOrDefault(x => x.Type == projectile.Type);
      
      if (resist != null) dmg *= CalculateDamageCoefficient(resist.Amount);
      
      HP -= (int) Mathf.Round(dmg);
      
      UpdateHPBar();
      
      if (HP <= 1) Fed();
    }

    public void SetSpeed(float speed)
    {
      _agent.SetSpeed(speed);
      if(speed < _enemy.MoveSpeed) _slowEffect.Play();
      else _slowEffect.Stop();
    }
    
    public void Disappear()
    {
      gameObject.layer = DEAD_ENEMIES_LAYER;
      
      _animator.SetTrigger("Idle");
      
      if(Happy) _coinsParticleSystem.Play();

      if (_renderer == null)
      {
        Destroy(gameObject, 1.1f);
        return;
      }
			
      ChangeMaterialToFade();
			
      var meshColor = _renderer.material.color;
      var invisibleColor = meshColor;
      invisibleColor.a = 0;

      StartCoroutine(LerpMeshRendererColor(_renderer, 1.5f, meshColor, invisibleColor));
    }

    //---------------------------------------------------------------------
    // Heplers
    //---------------------------------------------------------------------

    private void Fed()
    {
      _dieEvent.Raise(gameObject);
      gameObject.layer = DEAD_ENEMIES_LAYER;
      Happy = true;
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


  }
}
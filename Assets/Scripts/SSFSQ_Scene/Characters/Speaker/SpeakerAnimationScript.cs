using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SpeakerAnimationScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    [Header("Events")]
    [SerializeField] private IntVariable _brainsCount;
    [SerializeField] private IntVariable _starsCount;

    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _speakerTransform;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _moveSpeed;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      DestinateToStart();
    }
    
    //---------------------------------------------------------------------
    // Events
    //---------------------------------------------------------------------

    public void OnGameOver()
    {
      if (_starsCount.RuntimeValue == 0)
      {
        StartBeingScared();
        return;
      }
      
      if(_brainsCount.RuntimeValue == 0)
      {
        Die();
      }
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public bool AnimationBusy { get; private set; }

    public void SetBusy(bool value)
    {
      AnimationBusy = value;
    }
    
    public void DestinateToStart()
    {
      _currentDestination = _startPosition;
      _animator.SetBool("DestinationStart", true);
    }

    public void DestinateToEnd()
    {
      _currentDestination = _endPosition;
      _animator.SetBool("DestinationStart", false);
    }

    public void GoToCurrentDestination()
    {
      if (Vector3.Distance(_speakerTransform.localPosition, _currentDestination.localPosition) < ACCURACY
          && _currentDestination == _endPosition)
      {
        _animator.SetBool("EndPosition", true);
      }

      if (Vector3.Distance(_speakerTransform.localPosition, _currentDestination.localPosition) < ACCURACY
          && _currentDestination == _startPosition)
      {
        _animator.SetBool("StartPosition", true);
      }

      _speakerTransform.LookAt(_currentDestination.position);
      _speakerTransform.transform.position =
        Vector3.MoveTowards(_speakerTransform.transform.position, _currentDestination.position, _moveSpeed * Time.deltaTime);
    }

    public void StartBeingScared()
    {
      LookAtTheCrowd();
      _animator.SetTrigger("StartBeingScared");
      _animator.SetBool("BeScared", true);
    }

    public void StopBeingScared()
    {
      _animator.SetBool("BeScared", false);
    }

    public void Die()
    {
      LookAtTheCrowd();
      _animator.SetTrigger("StartDying");
      _animator.SetBool("BeDead", true);
    }

    public void StopBeingDead()
    {
      _animator.SetBool("BeDead", false);
    }

    public void Answer(bool goodQuestion)
    {
      _animator.SetTrigger("Question");
      if (goodQuestion)
      {
        _animator.SetBool("Answer", true);
      }
      else
      {
        _animator.SetBool("Yell", true);
      }

      SetBusy(true);
    }

    public void Hit()
    {
      _animator.SetTrigger("Question");
      _animator.SetBool("Hit", true);
      SetBusy(true);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const float ACCURACY = .001f;
    private Transform _currentDestination;

    public void LookAtTheCrowd()
    {
      _speakerTransform.LookAt(new Vector3(0, .001f, 1));
    }
  }
}
using System.Collections;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SpeakerAnimationScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

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
    // Public
    //---------------------------------------------------------------------

    public void TurnToCrowdPosition1()
    {
      SetPosition(_startPosition);
      SetRotation(180);
    }

    public void TurnToCrowdPosition2()
    {
      SetPosition(_endPosition);
      SetRotation(180);
    }

    public void DestinateToStart()
    {
      _currentDestination = _startPosition;
    }

    public void DestinateToEnd()
    {
      _currentDestination = _endPosition;
    }

    public void GoToCurrentDestination()
    {
      if (Vector3.Distance(_speakerTransform.localPosition, _endPosition.localPosition) < ACCURACY
          && _currentDestination == _endPosition)
      {
        _animator.SetTrigger("EndPosition");
      }

      if (Vector3.Distance(_speakerTransform.localPosition, _startPosition.localPosition) < ACCURACY
          && _currentDestination == _startPosition)
      {
        _animator.SetTrigger("StartPosition");
      }

      _speakerTransform.LookAt(_currentDestination.position);
      _speakerTransform.transform.position =
        Vector3.MoveTowards(_speakerTransform.transform.position, _currentDestination.position, _moveSpeed);
    }

    public void StartBeingScared()
    {
      SetRotation(180);
      _animator.SetTrigger("StartBeingScared");
      _animator.SetBool("BeScared", true);
    }

    public void StopBeingScared()
    {
      _animator.SetBool("BeScared", false);
    }

    public void Die()
    {
      SetRotation(180);
      _animator.SetTrigger("StartDying");
      _animator.SetBool("BeDead", true);
    }

    public void StopBeingDead()
    {
      _animator.SetBool("BeDead", false);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const float ACCURACY = .001f;
    private Transform _currentDestination;

    private void SetPosition(Transform position)
    {
      _speakerTransform.SetPositionAndRotation(position.position, position.rotation);
    }

    private void SetRotation(float yRotation)
    {
      _speakerTransform.Rotate(Vector3.up, -transform.eulerAngles.y + yRotation);
    }
  }
}
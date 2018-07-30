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
      SetSpeedByDestination(_endPosition, NORMALIZED_TIME_OF_WALKING);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void TurnToCrowdPosition1()
    {
      SetPosition(_startPosition);
      SetSpeedByDestination(_endPosition, NORMALIZED_TIME_OF_WALKING);
      SetRotation(180);
    }
    
    public void TurnToCrowdPosition2()
    {
      SetPosition(_endPosition);
      SetSpeedByDestination(_startPosition, NORMALIZED_TIME_OF_WALKING);
      SetRotation(180);
    }
    
    public void GoToStartPosition()
    {
      if(Vector3.Distance(_speakerTransform.localPosition, _startPosition.localPosition) < ACCURACY) _animator.SetTrigger("StartPosition");
      _speakerTransform.LookAt(_startPosition.position);
      _speakerTransform.transform.position=
        Vector3.MoveTowards(_speakerTransform.transform.position, _startPosition.position, _moveSpeed);
    }
    
    public void GoToEndPosition()
    {
      if(Vector3.Distance(_speakerTransform.localPosition, _endPosition.localPosition) < ACCURACY) _animator.SetTrigger("EndPosition");
      _speakerTransform.LookAt(_endPosition.position);
      _speakerTransform.transform.position =
        Vector3.MoveTowards(_speakerTransform.transform.position, _endPosition.position, _moveSpeed);
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

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private const float NORMALIZED_TIME_OF_WALKING = .5f;
    private const float ACCURACY = .001f;
    
    private void SetSpeedByDestination(Transform destination, float time)
    {
      var distance = Vector3.Distance(_speakerTransform.localPosition, destination.localPosition);
      _moveSpeed = distance / time;
    }
    
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
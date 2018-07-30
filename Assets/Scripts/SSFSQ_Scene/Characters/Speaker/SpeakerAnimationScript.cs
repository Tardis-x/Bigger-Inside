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
    [SerializeField] private Transform _position1;
    [SerializeField] private Transform _position2;
    [SerializeField] private float _moveSpeed;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Start()
    {
      SetSpeedByDestination(_position2, NORMALIZED_TIME_OF_WALKING);
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void TurnToCrowdPosition1()
    {
      SetPosition(_position1);
      SetSpeedByDestination(_position2, NORMALIZED_TIME_OF_WALKING);
      SetRotation(180);
    }
    
    public void TurnToCrowdPosition2()
    {
      SetPosition(_position2);
      SetSpeedByDestination(_position1, NORMALIZED_TIME_OF_WALKING);
      SetRotation(180);
    }

    public void MoveForward()
    {
      _speakerTransform.Translate(Vector3.forward * _moveSpeed);
    }

    public void GoToStartPosition()
    {
      if(_speakerTransform.position.x - _position1.position.x < ACCURACY) _animator.SetTrigger("Homed");
      _speakerTransform.LookAt(_position1.position);
      _speakerTransform.transform.position =
        Vector3.MoveTowards(_speakerTransform.transform.position, _position1.position, _moveSpeed);
    }
    
    public void TurnLeft()
    {
      _speakerTransform.Rotate(Vector3.up, -transform.eulerAngles.y + 90);
    }

    public void TurnRight()
    {
      _speakerTransform.Rotate(Vector3.up, -transform.eulerAngles.y - 90);
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
    private const float ACCURACY = .1f;
    
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
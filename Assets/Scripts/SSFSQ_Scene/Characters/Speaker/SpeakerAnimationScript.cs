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
    // Public
    //---------------------------------------------------------------------

    public void TurnToCrowdPosition1()
    {
      SetPosition(_position1);
      SetRotation(180);
    }
    
    public void TurnToCrowdPosition2()
    {
      SetPosition(_position2);
      SetRotation(180);
    }

    public void MoveFroward()
    {
      _speakerTransform.Translate(Vector3.forward * _moveSpeed);
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
      _animator.SetTrigger("StartBeingScared");
    }

    public void StopBeingScared()
    {
      _animator.SetTrigger("StopBeingScared");
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
   
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
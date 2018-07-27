using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class SpeakerAnimationScript : Singleton<SpeakerAnimationScript>
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _moveSpeed;

    //---------------------------------------------------------------------
    // Messages
    //---------------------------------------------------------------------

    private void Awake()
    {
      _startPosition = _transform.localPosition;
    }

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void WalkAnotherWay()
    {
      _animator.SetBool("WalkOneWay", !_animator.GetBool("WalkOneWay"));
      ResetPosition();
      ResetRotation();
    }

    public void MoveFroward()
    {
      _transform.Translate(Vector3.forward * _moveSpeed);
    }

    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------

    private Vector3 _startPosition;

    private void ResetPosition()
    {
      _transform.localPosition = new Vector3(_transform.localPosition.x, transform.localPosition.y, _startPosition.z);
    }

    private void ResetRotation()
    {
      _transform.Rotate(Vector3.up, -transform.eulerAngles.y + 180);
    }

    private void TurnToWalk()
    {
      var rotation = new Vector3(0, 0, 0);
      rotation.y = _animator.GetBool("WalkOneWay") ? -90 : 90;
      _transform.eulerAngles = rotation;
    }
  }
}
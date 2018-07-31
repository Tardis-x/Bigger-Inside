using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingGloveScript : MonoBehaviour 
{
  //---------------------------------------------------------------------
  // Editor
  //---------------------------------------------------------------------
  
  [SerializeField] private Transform _transform;
  
  //---------------------------------------------------------------------
  // Messages
  //---------------------------------------------------------------------

  private void Start()
  {
    _animator = GetComponentInChildren<Animator>();
  }

  //---------------------------------------------------------------------
  // Internal
  //---------------------------------------------------------------------

  private Animator _animator;

  private readonly Vector3 _offset = new Vector3
  {
    x = -.1356f,
    y = -.1020f,
    z = -.2250f
  };
  
  private void Hit()
  {
    _animator.SetTrigger("Hit");
  }

  private void PrepareToHitObject(Transform obj)
  {
    _transform.SetPositionAndRotation(obj.position, _transform.rotation);
    _transform.localPosition += _offset;
    Appear();
  }
  
  //---------------------------------------------------------------------
  // Public
  //---------------------------------------------------------------------

  public void HitObject(Transform obj)
  {
    PrepareToHitObject(obj);
    Hit();
  }
 

  public void Appear()
  {
    gameObject.SetActive(true);
  }

  public void Disappear()
  {
    gameObject.SetActive(false);
  }
}

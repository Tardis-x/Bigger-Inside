using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ua.org.gdg.devfest
{
  public class CharacterAnimationScript : MonoBehaviour
  {
    //---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------

    [SerializeField] private Animator _animator;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public void StandUpAndAsk()
    {
      _animator.SetTrigger("StandUpAndAsk");
    }

    public void StartThrowing()
    {
      StartCoroutine(Delay(() =>
      {
        _animator.SetTrigger("StartThrowing");
        _animator.SetBool("Throw", true);
      }));
    }

    public void StopThrowing()
    {
      StartCoroutine(Delay(() =>
      { _animator.SetBool("Throw", false); }));
    }

    public void StartBeingScared()
    {
      StartCoroutine(Delay(() =>
      {
        _animator.SetTrigger("StartBeingScared");
        _animator.SetBool("BeScared", true);
      }));
    }

    public void StopBeingScared()
    {
      StartCoroutine(Delay(() =>
        { _animator.SetBool("BeScared", false); }));
    }

    public void GetHit()
    {
      AnimationManager.Instance.BoxingGlove.HitObject(GetComponent<Transform>());
      _animator.SetTrigger("GetHit");
    }

    private IEnumerator Delay(Action action)
    {
      yield return new WaitForSeconds(Random.value);
      action();
    }
  }
}

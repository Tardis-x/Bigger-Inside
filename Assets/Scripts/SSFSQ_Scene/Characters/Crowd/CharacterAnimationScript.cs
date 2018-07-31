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
      }, Random.value * .5f));
    }

    public void StopThrowing()
    {
      StartCoroutine(Delay(() =>
      { _animator.SetBool("Throw", false); }, Random.value + .5f));
    }

    public void StartBeingScared()
    {
      StartCoroutine(Delay(() =>
      {
        _animator.SetTrigger("StartBeingScared");
        _animator.SetBool("BeScared", true);
      }, Random.value * .5f));
    }

    public void StopBeingScared()
    {
      StartCoroutine(Delay(() =>
        { _animator.SetBool("BeScared", false); }, Random.value + .5f));
    }

    public void GetHit()
    {
      AnimationManager.Instance.BoxingGlove.HitObject(GetComponent<Transform>());
      _animator.SetTrigger("GetHit");
    }

    private IEnumerator Delay(Action action, float time)
    {
      yield return new WaitForSeconds(time);
      action();
    }
  }
}

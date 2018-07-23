using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimmationScript : MonoBehaviour 
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
    _animator.SetTrigger("StandUpAndAskTrigger");
  }

  public void SitDown()
  {
    _animator.SetBool("StandUpAndAsk", false);
  }
}

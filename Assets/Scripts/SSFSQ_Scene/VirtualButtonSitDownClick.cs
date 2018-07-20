using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class VirtualButtonSitDownClick : Clickable
  {
    ///---------------------------------------------------------------------
    // Editor
    //---------------------------------------------------------------------
    [SerializeField] private Animator _animator;

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------

    public override void OnClick()
    {
      _animator.SetBool("StandUpAndAsk", false);
    }
  }
}
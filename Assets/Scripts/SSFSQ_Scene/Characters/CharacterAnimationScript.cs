using UnityEngine;

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
      _animator.SetTrigger("StandUpAndAskTrigger");
    }
  }
}

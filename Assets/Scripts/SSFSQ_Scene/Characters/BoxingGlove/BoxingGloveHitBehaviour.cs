using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class BoxingGloveHitBehaviour : StateMachineBehaviour
  {// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      animator.gameObject.GetComponentInParent<BoxingGloveScript>().Disappear();
    }
  }
}
﻿using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class AnswerBehaviour : StateMachineBehaviour
  {
// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      animator.gameObject.GetComponent<SpeakerAnimationScript>().LookAtTheCrowd();
    }
  }
}